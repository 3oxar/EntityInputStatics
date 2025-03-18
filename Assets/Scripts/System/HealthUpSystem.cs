using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;

partial struct HealthUpSystem : ISystem
{
    internal ComponentDataHandles _ComponentDataHandles;

    private EntityCommandBuffer _EntityCommandBuffer;
    private EntityManager _EntityManager;

    internal struct ComponentDataHandles
    {
        public ComponentLookup<PlayerTag> playerTag;
        public ComponentLookup<HealthPlayerComponent> healthPlayerComponent;
        public ComponentLookup<HealthUpComponent> healthUpComponent;

        public ComponentDataHandles(ref SystemState systemState)
        {
            playerTag = systemState.GetComponentLookup<PlayerTag>(true);
            healthPlayerComponent = systemState.GetComponentLookup<HealthPlayerComponent>(false);
            healthUpComponent = systemState.GetComponentLookup<HealthUpComponent>(true);
        }

        public void Update(ref SystemState state)
        {
            playerTag.Update(ref state);
            healthPlayerComponent.Update(ref state);
            healthUpComponent.Update(ref state);
        }
    }

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate(state.GetEntityQuery(ComponentType.ReadOnly<PlayerTag>()));

        _ComponentDataHandles = new ComponentDataHandles(ref state);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        _EntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _EntityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);

        foreach (var (heathUpComponent, entity) in SystemAPI.Query<RefRO<HealthUpComponent>>().WithEntityAccess())
        {
            _ComponentDataHandles.Update(ref state);
            state.Dependency = new CollisionEventHealtUpEnterJob
            {
                PlayerTag = _ComponentDataHandles.playerTag,
                EntityCommandBuffer = _EntityCommandBuffer,
                healthPlayerComponent = _ComponentDataHandles.healthPlayerComponent,
                healthUpComponent = _ComponentDataHandles.healthUpComponent,
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
            state.Dependency.Complete();
        }

        _EntityCommandBuffer.Playback(state.EntityManager);
        _EntityCommandBuffer.Dispose();
    }

    struct CollisionEventHealtUpEnterJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentLookup<PlayerTag> PlayerTag;
        [ReadOnly] public ComponentLookup<HealthUpComponent> healthUpComponent;
        public ComponentLookup<HealthPlayerComponent> healthPlayerComponent;
        public EntityCommandBuffer EntityCommandBuffer;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            if (healthPlayerComponent.HasComponent(entityA) && healthUpComponent.HasComponent(entityB))
            {
                var healthPlayer = healthPlayerComponent.GetRefRW(entityA);
                var healthUp = healthUpComponent.GetRefRO(entityB);
                healthPlayer.ValueRW.Health += healthUp.ValueRO.HealthUp;//лечение персонажа

                EntityCommandBuffer.DestroyEntity(entityB);//удаление объекта с компонентом лечения
            }

        }
    }
}
