using System.Collections;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

partial struct AIBotBehaveSystem : ISystem
{
   
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
       
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (bot, enemyTag) in SystemAPI.Query<AIBotComponent, RefRO<EnemyTag>>())
        {
            if(bot.ActivBehave != null)
            {
                bot.ActivBehave.Behave(ref state);
            }
           
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }

    
}
