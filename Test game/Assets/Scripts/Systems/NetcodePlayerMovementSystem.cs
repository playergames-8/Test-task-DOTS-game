using UnityEngine;
using Unity.NetCode;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;

[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
partial struct NetcodePlayerMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRO<NetcodePlayerInput> netcodePlayerInput, RefRW<LocalTransform> localTransform)
            in SystemAPI.Query<RefRO<NetcodePlayerInput>, RefRW<LocalTransform>>()
            .WithAll<Simulate>()) 
            {
            //applying movement to player
            float moveSpeed = 10f;
            float3 moveVector = new float3(netcodePlayerInput.ValueRO.inputVector.x, 0, netcodePlayerInput.ValueRO.inputVector.y);
            localTransform.ValueRW.Position += moveVector * moveSpeed * SystemAPI.Time.DeltaTime;
            }

    }

    
}
