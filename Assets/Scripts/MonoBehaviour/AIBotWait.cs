using Unity.Entities;
using UnityEngine;

public class AIBotWait : MonoBehaviour, IAIBot
{
    public Entity BotEntity { get ; set ; }
    public Entity PlayerEntity { get; set; }
    public SettingsBot SettingsBot { get; set; }

    public void Behave(ref SystemState systemState)
    {
    }

    public float Evaluate(ref SystemState systemState)
    {
        if (!systemState.EntityManager.Exists(PlayerEntity))
        {
            return 1f;
        }
        else
        {
            return 0.1f;
        }
    }
}
