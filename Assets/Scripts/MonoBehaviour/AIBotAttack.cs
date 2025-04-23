using System.Collections;
using Unity.Core;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

public class AIBotAttack : MonoBehaviour, IAIBot
{
    public Entity BotEntity { get; set; }
    public Entity PlayerEntity { get; set; }
    public SettingsBot SettingsBot { get; set; }

    private LocalTransform _localTransformBot;
    private LocalTransform _localTransformPlayer;
    private Vector3 _distance;

    public void Behave(ref SystemState systemState)
    {
        var bot = systemState.EntityManager.GetComponentData<AIBotComponent>(BotEntity);
        systemState.EntityManager.SetComponentData(BotEntity, bot);
    }

    public float Evaluate(ref SystemState systemState)
    {
        if (!systemState.EntityManager.Exists(PlayerEntity)) return 0;

        _localTransformBot = systemState.EntityManager.GetComponentData<LocalTransform>(BotEntity);
        _localTransformPlayer = systemState.EntityManager.GetComponentData<LocalTransform>(PlayerEntity);
        _distance = _localTransformBot.Position - _localTransformPlayer.Position;

        if (_distance.sqrMagnitude < SettingsBot.DistanceAttack)
        {
            return 0.8f;
        }
        else
        {
            return 0;
        }
    }
}        //_localTransformBot = systemState.EntityManager.GetComponentData<LocalTransform>(BotEntity);
         //_localTransformPlayer = systemState.EntityManager.GetComponentData<LocalTransform>(PlayerEntity);

//_distance = (_localTransformBot.Position - _localTransformPlayer.Position);