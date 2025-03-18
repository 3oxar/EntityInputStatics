using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;

partial struct BulletDestroySystem : ISystem
{
    internal ComponentDataHandles _ComponentDataHandles;

    private EntityCommandBuffer _EntityCommandBuffer;

    internal struct ComponentDataHandles
    {
        public ComponentLookup<BulletTag> BulletTag;
        public ComponentLookup<PlayerTag> PlayerTag;
        public ComponentLookup<FlyBulletComponent> FlyBulletComponent;

        public ComponentDataHandles(ref SystemState systemState)
        {
            BulletTag = systemState.GetComponentLookup<BulletTag>(true);
            PlayerTag = systemState.GetComponentLookup<PlayerTag>(true);
            FlyBulletComponent = systemState.GetComponentLookup<FlyBulletComponent>(true);
        }

        public void Update(ref SystemState state)
        {
            BulletTag.Update(ref state);
            PlayerTag.Update(ref state);
            FlyBulletComponent.Update(ref state);
        }
    }
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _ComponentDataHandles = new ComponentDataHandles(ref state);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        _EntityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);

        foreach (var (bullet, BulletTag, entity) in SystemAPI.Query<RefRW<BulletDestroyComponent>, RefRO<BulletTag>>().WithEntityAccess())
        {
            bullet.ValueRW.TimeDestroy -= SystemAPI.Time.DeltaTime;

            if(bullet.ValueRO.TimeDestroy < 0)//удаляем пулю если вышло ее время жизни
            {
                _EntityCommandBuffer.DestroyEntity(entity);
            }

            _ComponentDataHandles.Update(ref state);
            state.Dependency = new CollisionEventFlyBulletEnterJob
            {
                BulletTag = _ComponentDataHandles.BulletTag,
                PlayerTag = _ComponentDataHandles.PlayerTag,
                FlyBulletComponent = _ComponentDataHandles.FlyBulletComponent,
                EntityCommandBuffer = _EntityCommandBuffer,
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
            state.Dependency.Complete();
        }

        _EntityCommandBuffer.Playback(state.EntityManager);
        _EntityCommandBuffer.Dispose();
    }

    struct CollisionEventFlyBulletEnterJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentLookup<BulletTag> BulletTag;
        [ReadOnly] public ComponentLookup<PlayerTag> PlayerTag;
        [ReadOnly] public ComponentLookup<FlyBulletComponent> FlyBulletComponent;
        public EntityCommandBuffer EntityCommandBuffer;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            if (FlyBulletComponent.HasComponent(entityA))
            {
                var flyBullet = FlyBulletComponent.GetRefRO(entityA);

                if (flyBullet.ValueRO.IsDestroy == true || PlayerTag.HasComponent(entityB))//если взят бафф отскок пуль при столкновение с игроком пуля всегда уничтожается 
                {
                    EntityCommandBuffer.DestroyEntity(entityA);
                }
            }
        }
    }
}
