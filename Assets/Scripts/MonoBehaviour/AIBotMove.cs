using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

public class AIBotMove : MonoBehaviour, IAIBot
{
    public Entity BotEntity { get; set; }
    public Entity PlayerEntity { get; set; }
    public SettingsBot SettingsBot { get; set; }

    private LocalTransform _localTransformBot;
    private LocalTransform _localTransformPlayer;
    private Vector3 _distance;
    
    private float _deltaTime;

    public void Behave(ref SystemState systemState)
    {
        if (systemState.EntityManager.GetComponentData<DamageComponent>(BotEntity).IsDamage == false)
        {
            MoveBot(-1);
            systemState.EntityManager.SetComponentData(BotEntity, _localTransformBot);
        }
        else
        {
            MoveBot(1 ,0.3f);
            systemState.EntityManager.SetComponentData(BotEntity, _localTransformBot);
        }
    }

    public float Evaluate(ref SystemState systemState)
    {
        if (!systemState.EntityManager.Exists(PlayerEntity)) return 0;

        _deltaTime = Time.deltaTime;

        _localTransformBot = systemState.EntityManager.GetComponentData<LocalTransform>(BotEntity);
        _localTransformPlayer = systemState.EntityManager.GetComponentData<LocalTransform>(PlayerEntity);
        
        _distance = _localTransformBot.Position - _localTransformPlayer.Position;

        if (_distance.sqrMagnitude > SettingsBot.DistanceAttack)
        {
            return 0.6f;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// Движение бота к игроку
    /// </summary>
    /// <param name="move">движение к игроку "1" или от игрока "-1"</param>
    /// <param name="moveBack">как быстро после удара бот будет уходить от игрока</param>
    private void MoveBot(int move ,float moveBack = 1)
    {
        _localTransformBot.Position = _localTransformBot.TransformPoint(_distance.normalized * moveBack * _deltaTime * (move));
        _localTransformBot.Position.y = 0.5f;
    }
}
