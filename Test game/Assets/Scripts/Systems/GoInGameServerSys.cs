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

        int inGamePlayerCount = 0;
        foreach (var networkStreamInGame in SystemAPI.Query<RefRO<NetworkStreamInGame>>())
        {
            inGamePlayerCount++;
        }

        //looking for clients with connection rpc request
        foreach ((RefRO<ReceiveRpcCommandRequest> receiveRpcCommandRequest, Entity entity)
            in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>>()
            .WithAll<GoInGameRequestRpc>()
            .WithEntityAccess())
            {
                //setting as ingame
                entityCommandBuffer.AddComponent<NetworkStreamInGame>(receiveRpcCommandRequest.ValueRO.SourceConnection);
                Debug.Log("Client Connected to Server!");

                if (inGamePlayerCount >= 2)
                {
                    Debug.LogWarning("Max players (2) reached on server. Connection rejected.");
                    
                    //sending rejection rpc to player
                    entityCommandBuffer.AddComponent<ConnectionRejectedRpc>(entity);
                    entityCommandBuffer.AddComponent<SendRpcCommandRequest>(entity, new SendRpcCommandRequest
                    {
                        TargetConnection = receiveRpcCommandRequest.ValueRO.SourceConnection
                    });
                    Debug.LogWarning("Sent RPC rejection to client: " + receiveRpcCommandRequest.ValueRO.SourceConnection);

                    entityCommandBuffer.DestroyEntity(entity);
                    continue;
                }

                //setting as ingame
                entityCommandBuffer.AddComponent<NetworkStreamInGame>(receiveRpcCommandRequest.ValueRO.SourceConnection);
                Debug.Log("Client Connected to Server!");

                //spawning player 1 and 2 depending on entry order
                NetworkId networkId = SystemAPI.GetComponent<NetworkId>(receiveRpcCommandRequest.ValueRO.SourceConnection);
                Entity playerEntity;
                if (networkId.Value == 1)
                {
                    playerEntity = entityCommandBuffer.Instantiate(entitiesReferences.playerPrefabEntity);
                    entityCommandBuffer.SetComponent(playerEntity, LocalTransform.FromPosition(new float3(-3, 0, 0)));
                }
                else
                {
                    playerEntity = entityCommandBuffer.Instantiate(entitiesReferences.player2PrefabEntity);
                    entityCommandBuffer.SetComponent(playerEntity, LocalTransform.FromPosition(new float3(3, 0, 0)));
                }




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
public struct ConnectionRejectedRpc : IRpcCommand
{
}
