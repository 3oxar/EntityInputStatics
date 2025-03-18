using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

partial struct FlyBulletSystem : ISystem
{
    internal ComponentDataHandles m_ComponentDataHandles;
    internal struct ComponentDataHandles
    {
        public ComponentLookup<FlyBulletComponent> FlyBulletComponent;

        public ComponentDataHandles(ref SystemState systemState)
        {
            FlyBulletComponent = systemState.GetComponentLookup<FlyBulletComponent>(false);
        }

        public void Update(ref SystemState state)
        {
            FlyBulletComponent.Update(ref state);
        }
    }
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        m_ComponentDataHandles = new ComponentDataHandles(ref state);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (flyBullet, bulletTag, velocity, entity) in SystemAPI.Query<RefRW<FlyBulletComponent>, RefRO<BulletTag>,
             RefRW<PhysicsVelocity>>().WithEntityAccess())
        {
            foreach(var bulletBounce in SystemAPI.Query<RefRO<BulletBounceComponent>>())
            {
                if(bulletBounce.ValueRO.IsBulletBounce == true)//если пуля должна отскакивать
                {
                    flyBullet.ValueRW.IsDestroy = false;

                    m_ComponentDataHandles.Update(ref state);
                    state.Dependency = new CollisionEventBulletBounceEnterJob
                    {
                        FlyBulletComponent = m_ComponentDataHandles.FlyBulletComponent,

                    }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
                    state.Dependency.Complete();
                    velocity.ValueRW.Linear = 0;

                    velocity.ValueRW.Linear.z = flyBullet.ValueRO.BulletVelosity;
                }
                else
                {
                    velocity.ValueRW.Linear.z = flyBullet.ValueRO.BulletVelosity;//полет пули по оси Z
                }
            }
           
        }
    }

    struct CollisionEventBulletBounceEnterJob : ICollisionEventsJob
    {
        public ComponentLookup<FlyBulletComponent> FlyBulletComponent;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            if (FlyBulletComponent.HasComponent(entityA))
            {
                var velocity = FlyBulletComponent[entityA];
                velocity.BulletVelosity *= (-1);//меняем направление полета пули
                FlyBulletComponent[entityA] = velocity;
            }
        }
    }
}
