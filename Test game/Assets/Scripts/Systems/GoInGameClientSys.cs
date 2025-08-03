using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;


[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
partial struct GoInGameClientSys : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<NetworkId>();
    }

    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (var ( networkId, entity)
            in SystemAPI.Query<RefRO<NetworkId>>()
            .WithNone<NetworkStreamInGame>()
            .WithEntityAccess())
            {
                //sets itself as ingame
                entityCommandBuffer.AddComponent<NetworkStreamInGame>(entity);
                Debug.Log("Setting Client as InGame"); //idk why but it refuses to work with normal using UnityEngine at the top
                
                //asks the server to be set as ingame
                Entity rpcEntity = entityCommandBuffer.CreateEntity();
                entityCommandBuffer.AddComponent(rpcEntity, new GoInGameRequestRpc());
                entityCommandBuffer.AddComponent(rpcEntity, new SendRpcCommandRequest());
            }

        //listen for rejection from server
        foreach (var (rpc, entity) in SystemAPI.Query<RefRO<ConnectionRejectedRpc>>().WithEntityAccess())
        {
            Debug.LogWarning("Connection rejected by server: game is full.");

            entityCommandBuffer.DestroyEntity(entity);

            foreach (var (conn, connectionEntity) in SystemAPI.Query<RefRO<NetworkStreamConnection>>().WithEntityAccess())
            {
                Debug.Log("Client disconnecting...");
                state.EntityManager.DestroyEntity(connectionEntity);
            }
        }

        entityCommandBuffer.Playback(state.EntityManager);
    }   
}

public struct GoInGameRequestRpc : IRpcCommand
{

}