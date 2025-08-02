using UnityEngine;
using Unity.NetCode;
using Unity.Entities;

public class MyValueAuthoring : MonoBehaviour
{
    public class Baker : Baker<MyValueAuthoring>
    {
        public override void Bake(MyValueAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new MyValue());
        }
    }
}

public struct MyValue : IComponentData
{
    [GhostField] public int value;
}
