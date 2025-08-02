using System.Diagnostics;
using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;


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

        foreach ((RefRO<NetworkId> NetworkId, Entity entity)
            in SystemAPI.Query<RefRO<NetworkId>>()
            .WithNone<NetworkStreamInGame>()
            .WithEntityAccess())
            {
                //sets itself as ingame
                entityCommandBuffer.AddComponent<NetworkStreamInGame>(entity);
                UnityEngine.Debug.Log("Setting Client as InGame"); //idk why but it refuses to work with normal using UnityEngine at the top
                
                //asks the server to be set as ingame
                Entity rpcEntity = entityCommandBuffer.CreateEntity();
                entityCommandBuffer.AddComponent(rpcEntity, new GoInGameRequestRpc());
                entityCommandBuffer.AddComponent(rpcEntity, new SendRpcCommandRequest());
            }

        entityCommandBuffer.Playback(state.EntityManager);
    }   
}

public struct GoInGameRequestRpc : IRpcCommand
{

}