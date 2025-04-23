using NUnit.Framework;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

class AIBotAuthoring : MonoBehaviour
{
    public List<MonoBehaviour> ListBehave;
    public IAIBot ActivBehave;
    public SettingsBot SettingsBot;

}

class AIBotAuthoringBaker : Baker<AIBotAuthoring>
{
    public override void Bake(AIBotAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);

        AddComponent<EnemyTag>(entity);
        AddComponentObject(entity, new AIBotComponent
        {
           ListBehave = authoring.ListBehave,
           ActivBehave = authoring.ActivBehave,
           SettingsBot = authoring.SettingsBot
        });
    }
}


class AIBotComponent : IComponentData
{
    public List<MonoBehaviour> ListBehave;
    public IAIBot ActivBehave;
    public SettingsBot SettingsBot;

}