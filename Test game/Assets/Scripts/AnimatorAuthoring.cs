using UnityEngine;
using Unity.NetCode;
using Unity.Entities;

public class AnimatorAuthoring : MonoBehaviour
{
    public Animator playerAnimator;
    public class Baker : Baker<AnimatorAuthoring>
	{
    		public override void Bake(AnimatorAuthoring authoring)
    		{
                var animatorEntity = GetEntity(authoring.playerAnimator.gameObject, TransformUsageFlags.None);

                var controlledEntity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(controlledEntity, new AnimatorAuthoringComponent
                {
                    playerAnimatorEntity = animatorEntity
                });
        }
	}
}

public struct AnimatorAuthoringComponent : IComponentData 
{
    public Entity playerAnimatorEntity;
} 
