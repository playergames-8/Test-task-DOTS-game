using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

public enum AnimationState : byte
{
    Idle,
    Running
}

[GhostComponent(PrefabType = GhostPrefabType.All)]
public struct PlayerAnimationState : IComponentData
{
    public AnimationState Value;
}
