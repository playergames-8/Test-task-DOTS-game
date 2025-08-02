using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
using Unity.Transforms;
using Unity.Mathematics;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct GoInGameServerSys : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EntitiesReferences>();
        state.RequireForUpdate<NetworkId>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        EntitiesReferences entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();

        //connecting all clients who have a request rpc and setting them as ingame
        foreach ((RefRO<ReceiveRpcCommandRequest> receiveRpcCommandRequest, Entity entity)
            in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>>()
            .WithAll<GoInGameRequestRpc>()
            .WithEntityAccess())
            {
                entityCommandBuffer.AddComponent<NetworkStreamInGame>(receiveRpcCommandRequest.ValueRO.SourceConnection);

                Debug.Log("Client Connected to Server!");

                NetworkId networkId = SystemAPI.GetComponent<NetworkId>(receiveRpcCommandRequest.ValueRO.SourceConnection);
                //spawning player at random range to avoid collisions with other players
                Entity playerEntity;
                if (networkId.Value == 1)
                {
                    playerEntity = entityCommandBuffer.Instantiate(entitiesReferences.playerPrefabEntity);
                }
                else
                {
                    playerEntity = entityCommandBuffer.Instantiate(entitiesReferences.player2PrefabEntity);
                }

                entityCommandBuffer.SetComponent(playerEntity, LocalTransform.FromPosition(new float3(UnityEngine.Random.Range(-5, 5), 0, 0)));



                //assigning ghost ownership to the connecting client
                entityCommandBuffer.AddComponent(playerEntity, new GhostOwner
                {
                    NetworkId = networkId.Value
                });

                //cleanup disconnected player
                entityCommandBuffer.AppendToBuffer(receiveRpcCommandRequest.ValueRO.SourceConnection, new LinkedEntityGroup
                {
                    Value = playerEntity
                });

                entityCommandBuffer.DestroyEntity(entity);
            }
        entityCommandBuffer.Playback(state.EntityManager);
    }

}
