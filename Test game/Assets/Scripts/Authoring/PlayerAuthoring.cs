using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Player());
            AddComponent(entity, new PlayerAnimationState
            {
                Value = AnimationState.Idle // start idle
            });
        }
    }
}

public struct Player : IComponentData
{

}
