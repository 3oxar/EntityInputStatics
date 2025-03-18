using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;

partial struct BulletBounceSystem : ISystem
{
    internal ComponentDataHandles _ComponentDataHandles;
    internal struct ComponentDataHandles
    {
        public ComponentLookup<PlayerTag> PlayerTag;
        public ComponentLookup<BulletBounceComponent> BulletBounceComponent;

        public ComponentDataHandles(ref SystemState systemState)
        {
            PlayerTag = systemState.GetComponentLookup<PlayerTag>(true);
            BulletBounceComponent = systemState.GetComponentLookup<BulletBounceComponent>(false);
        }

        public void Update(ref SystemState state)
        {
            PlayerTag.Update(ref state);
            BulletBounceComponent.Update(ref state);
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

        foreach (var (BulletBounceComponent, entity) in SystemAPI.Query<RefRW<BulletBounceComponent>>().WithEntityAccess())
        {
            _ComponentDataHandles.Update(ref state);

            if (BulletBounceComponent.ValueRO.TimeBulletBounce == 0)
            {

                state.Dependency = new CollisionEventHealtUpEnterJob
                {
                    PlayerTag = _ComponentDataHandles.PlayerTag,
                    BulletBounceComponent = _ComponentDataHandles.BulletBounceComponent,

                }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
                state.Dependency.Complete();
            }
            else if(BulletBounceComponent.ValueRO.TimeBulletBounce > 0)//время действия отскока пуль 
            {
                BulletBounceComponent.ValueRW.TimeBulletBounce -= SystemAPI.Time.DeltaTime;

                if (BulletBounceComponent.ValueRO.TimeBulletBounce <= 0)
                {
                    BulletBounceComponent.ValueRW.TimeBulletBounce = 0;
                    BulletBounceComponent.ValueRW.IsBulletBounce = false;
                }
            }
          
        }
    }

    struct CollisionEventHealtUpEnterJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentLookup<PlayerTag> PlayerTag;
        public ComponentLookup<BulletBounceComponent> BulletBounceComponent;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            if (BulletBounceComponent.HasComponent(entityB) && PlayerTag.HasComponent(entityA))
            {
                var bulletBounce = BulletBounceComponent.GetRefRW(entityB);
                bulletBounce.ValueRW.IsBulletBounce = true;//включаем отскок пуль
                bulletBounce.ValueRW.TimeBulletBounce = 4f;//устанавливаем время
            }
        }
    }
}
