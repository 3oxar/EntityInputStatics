using Unity.Entities;

public interface IAIBotsData
{
    public Entity BotEntity { get; set; }
    public Entity PlayerEntity { get; set; }
    public SettingsBot SettingsBot { get; set; }
}
