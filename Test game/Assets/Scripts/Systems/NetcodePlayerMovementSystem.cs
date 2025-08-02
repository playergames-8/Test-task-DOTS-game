using UnityEngine;
using Unity.NetCode;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.VisualScripting;

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
        foreach (
            var (netcodePlayerInput, localTransform, playerAnimationState) // had to change this to var coz it was mad at me when I added RefRW<PlayerAnimationState>
            in SystemAPI.Query<RefRO<NetcodePlayerInput>, RefRW<LocalTransform>, RefRW<PlayerAnimationState>>()
                .WithAll<Simulate>())
        {
            //applying movement to player
            float moveSpeed = 10f;
            float3 moveVector = new float3(netcodePlayerInput.ValueRO.inputVector.x, 0, netcodePlayerInput.ValueRO.inputVector.y);
            localTransform.ValueRW.Position += moveVector * moveSpeed * SystemAPI.Time.DeltaTime;

            playerAnimationState.ValueRW.Value = math.lengthsq(moveVector) > 0.01f
                ? AnimationState.Running
                : AnimationState.Idle;
        }

    }

    
}
