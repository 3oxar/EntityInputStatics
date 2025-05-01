using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

class AIBotsAuthoring : MonoBehaviour
{
    public float Priority;
    public bool State;
    public Entity BotEntity;
    public Entity PlayerEntity; 
    public Vector3 Distance;
    public SettingsBot SettingsBot;
}

class AIBotsAuthoringBaker : Baker<AIBotsAuthoring>
{
    public override void Bake(AIBotsAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);

        AddComponent<EnemyTag>(entity);
        AddComponent(entity, new AIBotsComponent
        {
            Priority = authoring.Priority,
            State = authoring.State,
        });
        AddComponentObject(entity, new AIBotsDataComponent
        {
            BotEntity = authoring.BotEntity,
            PlayerEntity = authoring.PlayerEntity,
            SettingsBot = authoring.SettingsBot,
            Distance = authoring.Distance
        });
    }
}


public struct AIBotsComponent : IComponentData
{
    public float Priority;
    public bool State;
}

public class AIBotsDataComponent : IComponentData
{
    public Entity BotEntity;
    public Entity PlayerEntity;
    public Vector3 Distance;
    public SettingsBot SettingsBot;

}
