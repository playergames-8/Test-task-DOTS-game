using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using UnityEditor.MemoryProfiler;
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
        var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);


        foreach (var ( networkId, entity)
            in SystemAPI.Query<RefRO<NetworkId>>()
            .WithNone<NetworkStreamInGame>()
            .WithEntityAccess())
            {
                //sets itself as ingame
                entityCommandBuffer.AddComponent<NetworkStreamInGame>(entity);
                Debug.Log("Setting Client as InGame");
                
                //asks the server to be set as ingame
                Entity rpcEntity = entityCommandBuffer.CreateEntity();
                entityCommandBuffer.AddComponent(rpcEntity, new GoInGameRequestRpc());
                entityCommandBuffer.AddComponent(rpcEntity, new SendRpcCommandRequest());
            }

        

        //listen for rejection from server
        foreach (var (rpc, rpcEntity) in SystemAPI.Query<RefRO<ConnectionRejectedRpc>>().WithEntityAccess())
        {
            Debug.LogWarning("Connection rejected by server: game is full.");
            entityCommandBuffer.DestroyEntity(rpcEntity);
        }

        entityCommandBuffer.Playback(state.EntityManager);
    }   
}

public struct GoInGameRequestRpc : IRpcCommand
{

}

//NetworkConnection.State connectionState = m_ConnectionList.GetConnectionState(connectionId);
//if (connectionState != NetworkConnection.State.Disconnecting)
//{
//    return connectionState;
//}