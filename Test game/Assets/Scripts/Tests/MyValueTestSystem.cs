using Unity.Burst;
using Unity.Entities;
using UnityEngine;

partial struct MyValueTestSystem : ISystem
{

    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
      foreach ((RefRO<MyValue> myValue, Entity entity)
            in SystemAPI.Query<RefRO<MyValue>>().WithEntityAccess())
        {
            Debug.Log("My value: " + myValue.ValueRO.value + " :: " + entity + " :: " + state.World);
        }  
    }
}

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
public partial struct TestValueServerSys : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (RefRW<MyValue> myValue
            in SystemAPI.Query<RefRW<MyValue>>())
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                myValue.ValueRW.value = UnityEngine.Random.Range(1, 100);
                Debug.Log("Changed value: " + myValue.ValueRW.value);
            }
        }
    }
}