using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;


[UpdateInGroup(typeof(AIBotGroup))]
partial struct AIBotEvaluateSystem : ISystem
{
    private Entity _playerEntity;

    private LocalTransform _localTransformBot;
    private LocalTransform _localTransformPlayer;
    private Vector3 _distance;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (playerTag, entity) in SystemAPI.Query<RefRO<PlayerTag>>().WithEntityAccess())
        {
            _playerEntity = entity;
        }
      
        foreach (var (botData, bot, botTag, entity) in SystemAPI.Query<AIBotsDataComponent, RefRW<AIBotsComponent>, RefRO<EnemyTag>>().WithEntityAccess())
        {
            if (state.EntityManager.Exists(_playerEntity) == true)
            {
                _localTransformBot = state.EntityManager.GetComponentData<LocalTransform>(entity);
                _localTransformPlayer = state.EntityManager.GetComponentData<LocalTransform>(_playerEntity);
                _distance = _localTransformBot.Position - _localTransformPlayer.Position;

                botData.BotEntity = entity;
                botData.PlayerEntity = _playerEntity;
                botData.Distance = _distance;

                if (_distance.sqrMagnitude < botData.SettingsBot.DistanceAttack)
                {
                    ecb.AddComponent<AIBotAttackTag>(entity);
                }
                else if (_distance.sqrMagnitude > botData.SettingsBot.DistanceAttack)
                {
                    ecb.AddComponent<AIBotMoveTag>(entity);
                }
            }
            else
            {
                ecb.AddComponent<AIBotWaitTag>(entity);
            }
        }
        ecb.Playback(state.EntityManager);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }
}


//foreach (var (playerTag, entity) in SystemAPI.Query<RefRO<PlayerTag>>().WithEntityAccess())
//{
//    _playerEntity = entity;
//}

//foreach (var (bot, enemyTag, entity) in SystemAPI.Query<AIBotComponent, RefRO<EnemyTag>>().WithEntityAccess())
//{
//    float highScore = float.MinValue;
//    bot.ActivBehave = null;

//    foreach (var behavior in bot.ListBehave)
//    {
//        if (behavior is IAIBot aiBot)
//        {
//            aiBot.BotEntity = entity;
//            aiBot.PlayerEntity = _playerEntity;
//            aiBot.SettingsBot = bot.SettingsBot;

//            var currentScore = aiBot.Evaluate(ref state);

//            if (currentScore > highScore)
//            {
//                highScore = currentScore;
//                bot.ActivBehave = aiBot;
//            }
//        }
//    }
//}