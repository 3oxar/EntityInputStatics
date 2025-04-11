using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

partial struct AIBotSystem : ISystem
{
    private LocalTransform playerTransform;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
       
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        
        foreach (var (playerLocalTransform, tag) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<PlayerTag>>())
        {
            playerTransform = playerLocalTransform.ValueRO;
            
        }

        foreach (var (bot, localTransform, entuty) in SystemAPI.Query<AIBotComponent, RefRO<LocalTransform>>().WithEntityAccess())
        {
            Vector3 distance = (localTransform.ValueRO.Position - playerTransform.Position);

            var evalute = distance.sqrMagnitude;

            if(evalute < 1.6)
            {
                Attack();
            }
            else if(evalute > 1.6)
            {
                Walk();
            }
            else if (evalute == 0)
            {
                Wait();
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void Wait()
    {
        Debug.Log("Wait");
    }

    [BurstCompile]
    public void Walk()
    {
        Debug.Log("Walk");
    }

    [BurstCompile]
    public void Attack()
    {
        Debug.Log("Attack");
    }

}
