using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using UnityEditor.Search;


partial struct HealthPlayerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ECB = new EntityCommandBuffer(Allocator.Temp);
        

        foreach (var (health, player, entity) in SystemAPI.Query<RefRO<HealthPlayerComponent>, RefRO<PlayerTag>>().WithEntityAccess())
        {
            if (health.ValueRO.Health <= 0)
            {
                Debug.Log("destroy");
                var buffer = state.EntityManager.GetBuffer<Child>(entity).AsNativeArray().Reinterpret<Entity>();

                ECB.DestroyEntity(buffer);
                ECB.DestroyEntity(entity);
            }
        }
        ECB.Playback(state.EntityManager);
        ECB.Dispose();



    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}

