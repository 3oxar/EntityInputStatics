using Unity.Burst;
using Unity.Entities;

partial struct AIBotEvaluateSystem : ISystem
{
    private Entity _playerEntity;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        foreach(var (playerTag, entity) in SystemAPI.Query<RefRO<PlayerTag>>().WithEntityAccess())
        {
            _playerEntity = entity;
        }

        foreach (var (bot, enemyTag, entity) in SystemAPI.Query<AIBotComponent, RefRO<EnemyTag>>().WithEntityAccess())
        {
            float highScore = float.MinValue;
            bot.ActivBehave = null;

            foreach (var behavior in bot.ListBehave)
            {
                if (behavior is IAIBot aiBot)
                {
                    aiBot.BotEntity = entity;
                    aiBot.PlayerEntity = _playerEntity;
                    aiBot.SettingsBot = bot.SettingsBot;

                    var currentScore = aiBot.Evaluate(ref state);

                    if (currentScore > highScore)
                    {
                        highScore = currentScore;
                        bot.ActivBehave = aiBot;
                    }
                }
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
