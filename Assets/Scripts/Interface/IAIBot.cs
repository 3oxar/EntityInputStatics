using Unity.Entities;
using UnityEngine;

public interface IAIBot 
{
    public Entity BotEntity { get; set; }
    public Entity PlayerEntity { get; set; }
    public SettingsBot SettingsBot { get; set; }

    public float Evaluate(ref SystemState systemState);

    public void Behave(ref SystemState systemState);
}
