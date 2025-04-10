using Unity.Entities;
using UnityEngine;

class HealthPlayerAuthoring : MonoBehaviour
{
    public SettingsPlayer SettingsPlayer;
    [HideInInspector] public int Health;
}

class HealthPlayerAuthoringBaker : Baker<HealthPlayerAuthoring>
{
    public override void Bake(HealthPlayerAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);

        AddComponent(entity, new HealthPlayerComponent
        {
            Health = authoring.SettingsPlayer.Health
        });
    }
}

struct HealthPlayerComponent : IComponentData
{

    public int Health;
}