using System.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[UpdateInGroup(typeof(AIBotGroup))]
[UpdateAfter(typeof(AIBotEvaluateSystem))]
partial struct AIBotBehaveSystem : ISystem
{
    private float _deltaTime;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        Debug.Log("behave");
        _deltaTime = SystemAPI.Time.DeltaTime;
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (bot, enemyTag, botAttackTag, entity) in SystemAPI.Query< RefRO<AIBotsComponent>, RefRO<EnemyTag>, RefRO<AIBotAttackTag>>().WithEntityAccess())
        {
            Debug.Log("Attack");
            ecb.RemoveComponent<AIBotAttackTag>(entity);

        }
        foreach (var (botData, bot, botLocalTransform, enemyTag, botAttackTag, entity) in SystemAPI.Query<AIBotsDataComponent ,RefRO<AIBotsComponent>,
            RefRW<LocalTransform>, RefRO<EnemyTag>, RefRO<AIBotMoveTag>>().WithEntityAccess())
        {
            Debug.Log("Move");
            
            botLocalTransform.ValueRW.Position = botLocalTransform.ValueRW.TransformPoint(botData.Distance.normalized * _deltaTime * (-1));
            botLocalTransform.ValueRW.Position.y = 0.5f;

            ecb.RemoveComponent<AIBotMoveTag>(entity);

        }
        foreach (var (bot, enemyTag, botAttackTag, entity) in SystemAPI.Query< RefRO<AIBotsComponent>, RefRO<EnemyTag>, RefRO<AIBotWaitTag>>().WithEntityAccess())
        {
            Debug.Log("Wait");
            ecb.RemoveComponent<AIBotWaitTag>(entity);
        }

        ecb.Playback(state.EntityManager);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }

    
}


//foreach(var (bot, enemyTag) in SystemAPI.Query<AIBotComponent, RefRO<EnemyTag>>())
//{
//    if(bot.ActivBehave != null)
//    {
//        bot.ActivBehave.Behave(ref state);
//    }

//}