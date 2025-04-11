using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using Unity.Core;

partial struct AIBotSystem : ISystem
{
    private LocalTransform _playerTransform;
    private Vector3 _distance;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
       
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        
        foreach (var (playerLocalTransform, tag) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<PlayerTag>>())
        {
            _playerTransform = playerLocalTransform.ValueRO;
            
        }

        foreach (var (bot, localTransform, entuty) in SystemAPI.Query<AIBotComponent, RefRO<LocalTransform>>().WithEntityAccess())
        {
            _distance = (localTransform.ValueRO.Position - _playerTransform.Position);

            var evalute = _distance.sqrMagnitude;

            if(evalute < 2)
            {
                Attack();
            }
            else if(evalute > 2)
            {
                Walk(ref state);
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
    public void Walk(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        
        foreach(var (bot, localTransform) in SystemAPI.Query<AIBotComponent, RefRW<LocalTransform>>())
        {
            Debug.Log("Walk");
            localTransform.ValueRW.Position = localTransform.ValueRO.TransformPoint(_distance.normalized * deltaTime * (-1));
        }

    }

    [BurstCompile]
    public void Attack()
    {
        Debug.Log("Attack");
    }

}
