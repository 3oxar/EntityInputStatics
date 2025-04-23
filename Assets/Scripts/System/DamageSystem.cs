using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
partial struct DamageSystem : ISystem
{
    internal ComponentDataHandles _ComponentDataHandles;

    private EntityCommandBuffer _EntityCommandBuffer;
    private EntityManager _EntityManager;

    internal float _timeReloadDamage;

    internal struct ComponentDataHandles
    {
        public ComponentLookup<PlayerTag> playerTag;
        public ComponentLookup<DamageTag> damageTag;
        public ComponentLookup<HealthPlayerComponent> healthPlayerComponent;
        public ComponentLookup<DamageComponent> damageComponent;

        public ComponentDataHandles(ref SystemState systemState)
        {
            playerTag = systemState.GetComponentLookup<PlayerTag>(true);
            damageTag = systemState.GetComponentLookup<DamageTag>(true);
            healthPlayerComponent = systemState.GetComponentLookup<HealthPlayerComponent>(false);
            damageComponent = systemState.GetComponentLookup<DamageComponent>(false);
        }

        public void Update(ref SystemState state)
        {
            playerTag.Update(ref state);
            damageTag.Update(ref state);
            healthPlayerComponent.Update(ref state);
            damageComponent.Update(ref state);
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
        _EntityCommandBuffer = SystemAPI.GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>().
            CreateCommandBuffer(_EntityManager.WorldUnmanaged);
       
        foreach (var (damageComponent, entity) in SystemAPI.Query<RefRW<DamageComponent>>().WithEntityAccess())
        {
            if (damageComponent.ValueRO.TimeReloadDamage == 0)
            {
                _ComponentDataHandles.Update(ref state);
                state.Dependency = new CollisionEventDamageEnterJob
                {
                    PlayerTag = _ComponentDataHandles.playerTag,
                    DamageTag = _ComponentDataHandles.damageTag,
                    healthPlayerComponent = _ComponentDataHandles.healthPlayerComponent,
                    damageComponent = _ComponentDataHandles.damageComponent,
                }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
                state.Dependency.Complete();
            }
            else if(damageComponent.ValueRO.TimeReloadDamage > 0)
            {
                damageComponent.ValueRW.TimeReloadDamage -= SystemAPI.Time.DeltaTime;
              
                if (damageComponent.ValueRO.TimeReloadDamage <= 0)
                {
                    damageComponent.ValueRW.TimeReloadDamage = 0;
                    damageComponent.ValueRW.IsDamage = false;//опять можно наносить урон игроку 
                }
            }
        }
    }

    [BurstCompile]
    struct CollisionEventDamageEnterJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentLookup<PlayerTag> PlayerTag;
        [ReadOnly] public ComponentLookup<DamageTag> DamageTag;
        public ComponentLookup<HealthPlayerComponent> healthPlayerComponent;
        public ComponentLookup<DamageComponent> damageComponent;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            if (healthPlayerComponent.HasComponent(entityA) && damageComponent.HasComponent(entityB))
            {
                var healthPlayer = healthPlayerComponent.GetRefRW(entityA);
                var damage = damageComponent.GetRefRW(entityB);
                if(damage.ValueRO.IsDamage == false)
                {
                    damage.ValueRW.TimeReloadDamage = 2f;
                    damage.ValueRW.IsDamage = true;
                    healthPlayer.ValueRW.Health--;//нанесения урона
                }
            }
        }
    }
}
