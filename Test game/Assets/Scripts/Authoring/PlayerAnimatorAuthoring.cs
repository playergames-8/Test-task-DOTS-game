using UnityEngine;
using Unity.NetCode;
using Unity.Entities;

public class PlayerGameObjectPrefab : IComponentData
{
    public GameObject value;
}

public class PlayerAnimatorReference : ICleanupComponentData
{
    public Animator value;
}

public class PlayerAnimatorAuthoring : MonoBehaviour
{
    public GameObject PlayerGameObjectPrefab;

    public class Baker : Baker<PlayerAnimatorAuthoring>
	{
    		public override void Bake(PlayerAnimatorAuthoring authoring)
    		{
        		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        		AddComponentObject(entity, new PlayerGameObjectPrefab());
    		}
	}
}

