using Unity.Collections;
using Unity.Entities;
using Unity.Burst;
using Zenject;
using UnityEngine;

partial struct HealthPlayerSystem : ISystem
{
    private float _timeDie;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _timeDie = 2;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ECB = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (health, playerPrefab, playerTag, entityQuery) in SystemAPI.Query<RefRO<HealthPlayerComponent>, PlayerGameObjectPrefab, RefRO<PlayerTag>>().WithEntityAccess())
        {
            if(health.ValueRO.Health <= 0)//если 0 жизней то уничтожаем игрока
            {
                if(_timeDie > 0)
                {
                    _timeDie -= SystemAPI.Time.DeltaTime;
                }
                else if(_timeDie <= 0)
                {
                    ECB.DestroyEntity(entityQuery); 
                    _timeDie = 2;
                }
            }
        }

        ECB.Playback(state.EntityManager);
        ECB.Dispose();
    }
}

