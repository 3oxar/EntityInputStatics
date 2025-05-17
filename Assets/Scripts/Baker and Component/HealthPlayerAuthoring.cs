using Unity.Entities;
using UnityEngine;
using Zenject;

class HealthPlayerAuthoring : MonoBehaviour
{
    public CurrentConfigPlayer SettingsPlayer;

    [Inject]
    public void Init(IConfigurationPlayer configurationPlayer)
    {
        SettingsPlayer.CurretnSettingsPlayer = configurationPlayer.SettingsPlayer;
    }
}

class HealthPlayerAuthoringBaker : Baker<HealthPlayerAuthoring>
{
    public override void Bake(HealthPlayerAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);
        AddComponent(entity, new HealthPlayerComponent
        {
            Health = authoring.SettingsPlayer.CurretnSettingsPlayer.Health
        });
    }
}

public struct HealthPlayerComponent : IComponentData
{
    public int Health;
}