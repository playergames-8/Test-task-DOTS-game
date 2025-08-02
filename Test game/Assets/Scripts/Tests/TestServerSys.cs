using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;


[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct TestServerSys : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach ((RefRO<SimpleRPC> rpc, RefRO<ReceiveRpcCommandRequest> reciveRequest, Entity entity)
            in SystemAPI.Query<RefRO<SimpleRPC>, RefRO<ReceiveRpcCommandRequest>>().WithEntityAccess())
            {
  
                Debug.Log("Received RPC: " + rpc.ValueRO.value + ": : " + reciveRequest.ValueRO.SourceConnection);
                entityCommandBuffer.DestroyEntity(entity);

            }
        entityCommandBuffer.Playback(state.EntityManager);
    }

}
