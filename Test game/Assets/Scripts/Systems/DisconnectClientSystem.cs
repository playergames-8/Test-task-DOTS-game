using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;

[UpdateInGroup(typeof(SimulationSystemGroup))]
partial struct DisconnectClientSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
        foreach (var (pendingDisconnectTag, entity) in SystemAPI.Query<RefRW<PendingDisconnectTag>>().WithEntityAccess())
        {
            entityCommandBuffer.RemoveComponent<PendingDisconnectTag>(entity);
            entityCommandBuffer.AddComponent<NetworkStreamRequestDisconnect>(entity);
        }
        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
    }

}

public struct PendingDisconnectTag : IComponentData
{

}
