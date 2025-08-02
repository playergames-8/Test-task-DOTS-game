using Unity.Entities;
using UnityEngine;

public class EntitiesRefrencesAuthoring : MonoBehaviour
{
    public GameObject playerPrefabGameObject;
    public Animator playerAnimator;

    public class Baker : Baker<EntitiesRefrencesAuthoring>
    {
        public override void Bake(EntitiesRefrencesAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EntitiesReferences
            {
                playerPrefabEntity = GetEntity(authoring.playerPrefabGameObject, TransformUsageFlags.Dynamic)

            });
        }
    }
}
public struct EntitiesReferences : IComponentData
{
    public Entity playerPrefabEntity;
    public Entity playerAnimatorEntity;
}