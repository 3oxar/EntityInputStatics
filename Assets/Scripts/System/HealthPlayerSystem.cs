using Unity.Collections;
using Unity.Entities;
using Unity.Burst;
using Zenject;

partial struct HealthPlayerSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ECB = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (health, player, entityQuery) in SystemAPI.Query<RefRO<HealthPlayerComponent>, RefRO<PlayerTag>>().WithEntityAccess())
        {
            if(health.ValueRO.Health <= 0)//если 0 жизней то уничтожаем игрока
            {
                ECB.DestroyEntity(entityQuery);
            }
        }

        ECB.Playback(state.EntityManager);
        ECB.Dispose();
    }
}

