using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ConfigurationInstaller", menuName = "Installers/ConfigurationInstaller")]
public class ConfigurationInstaller : ScriptableObjectInstaller<ConfigurationInstaller>
{
    public SettingsPlayer SettingsPlayerEasy;
    public SettingsPlayer SettingsPlayerMedium;
    public SettingsPlayer SettingsPlayerHard;
    public SettingsPlayer DefaultSettingPlayer;
    public DifficultyLevel DifficultyLevel;
    public override void InstallBindings()
    {
        switch (DifficultyLevel)
        {
            case DifficultyLevel.Easy:
                Container.Bind<ScriptableObject>().FromInstance(SettingsPlayerEasy);
                Container.Bind<IConfigurationPlayer>().To<PlayerConfiguration>().AsSingle().NonLazy();
                Container.BindInstances(SettingsPlayerEasy);
                Container.QueueForInject(SettingsPlayerEasy);
                break;
            case DifficultyLevel.Medium:
                Container.Bind<ScriptableObject>().FromInstance(SettingsPlayerMedium);
                Container.Bind<IConfigurationPlayer>().To<PlayerConfiguration>().AsSingle().NonLazy();
                Container.BindInstances(SettingsPlayerMedium);
                Container.QueueForInject(SettingsPlayerMedium);
                break;
            case DifficultyLevel.Hard:
                Container.Bind<ScriptableObject>().FromInstance(SettingsPlayerHard);
                Container.Bind<IConfigurationPlayer>().To<PlayerConfiguration>().AsSingle().NonLazy();
                Container.BindInstances(SettingsPlayerHard);
                Container.QueueForInject(SettingsPlayerHard);
                break;
            case DifficultyLevel.defaultSetting:
                Container.Bind<ScriptableObject>().FromInstance(DefaultSettingPlayer);
                Container.Bind<IConfigurationPlayer>().To<DummySettingPlayer>().AsSingle().NonLazy();
                Container.BindInstances(DefaultSettingPlayer);
                Container.QueueForInject(DefaultSettingPlayer);
                break;
        }
    }
}

public class PlayerConfiguration: IConfigurationPlayer
{
    public SettingsPlayer SettingsPlayer { get; set; }

    public PlayerConfiguration(SettingsPlayer settingsPlayer)
    {
        SettingsPlayer = settingsPlayer;
    }
}
public interface IConfigurationPlayer
{
    public SettingsPlayer SettingsPlayer { get; set; }
}

/// <summary>
/// Дефолтные настройки
/// </summary>
public class DummySettingPlayer : IConfigurationPlayer
{
    public SettingsPlayer SettingsPlayer { get; set; }

    public DummySettingPlayer(SettingsPlayer settingsPlayer)
    {
        SettingsPlayer = settingsPlayer;
        settingsPlayer.Health = 99;
    }
}

/// <summary>
/// Уровни сложности
/// </summary>
public enum DifficultyLevel
{
    defaultSetting = 0,
    Easy = 1,
    Hard = 2,
    Medium = 3,
}