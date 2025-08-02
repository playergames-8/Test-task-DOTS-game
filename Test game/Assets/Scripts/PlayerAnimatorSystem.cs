using UnityEngine;
using Unity.Entities;

public partial class PlayerAnimatorSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var em = EntityManager;

        foreach (var (animRef, animState) in SystemAPI.Query<AnimatorAuthoringComponent, PlayerAnimationState>())
        {
            if (em.HasComponent<Animator>(animRef.playerAnimatorEntity))
            {
                var animator = em.GetComponentObject<Animator>(animRef.playerAnimatorEntity);
                animator.SetBool("IsRunning", animState.Value == AnimationState.Running);
            }
        }
    }
}
