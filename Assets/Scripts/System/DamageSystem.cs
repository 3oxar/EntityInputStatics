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
    internal ComponentDataHandles m_ComponentDataHandles;

    private EntityCommandBuffer m_EntityCommandBuffer;
    private EntityManager m_EntityManager;

    internal struct ComponentDataHandles
    {
        public ComponentLookup<PlayerTag> playerTag;
        public ComponentLookup<DamageTag> damageTag;
        public ComponentLookup<HealthPlayerComponent> healthPlayerComponent;

        public ComponentDataHandles(ref SystemState systemState)
        {
            playerTag = systemState.GetComponentLookup<PlayerTag>(true);
            damageTag = systemState.GetComponentLookup<DamageTag>(true);
            healthPlayerComponent = systemState.GetComponentLookup<HealthPlayerComponent>(false);
        }

        public void Update(ref SystemState state)
        {
            playerTag.Update(ref state);
            damageTag.Update(ref state);
            healthPlayerComponent.Update(ref state);
        }
    }

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate(state.GetEntityQuery(ComponentType.ReadOnly<PlayerTag>()));

        m_ComponentDataHandles = new ComponentDataHandles(ref state);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        m_EntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        m_EntityCommandBuffer = SystemAPI.GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>().
            CreateCommandBuffer(m_EntityManager.WorldUnmanaged);

        foreach (var damageComponent in SystemAPI.Query<RefRO<DamageComponent>>())
        {
            m_ComponentDataHandles.Update(ref state);
            state.Dependency = new CollisionEventEnterJob
            {
                PlayerTag = m_ComponentDataHandles.playerTag,
                DamageTag = m_ComponentDataHandles.damageTag,
                EntityManager = m_EntityManager,
                EntityCommandBuffer = m_EntityCommandBuffer,
                healthPlayerComponent = m_ComponentDataHandles.healthPlayerComponent
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
            state.Dependency.Complete();
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }

    [BurstCompile]
    struct CollisionEventEnterJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentLookup<PlayerTag> PlayerTag;
        [ReadOnly] public ComponentLookup<DamageTag> DamageTag;
        public ComponentLookup<HealthPlayerComponent> healthPlayerComponent;
        public EntityCommandBuffer EntityCommandBuffer;
        public EntityManager EntityManager;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;
            //if (entityA != null)
            //{
            //    Debug.Log("EntityA - " + EntityManager.GetName(entityA));
            //}

            //if (entityB != null)
            //{
            //    Debug.Log("EntityB - " + EntityManager.GetName(entityB));
            //}

            if (healthPlayerComponent.HasComponent(entityA))
            {
                var healthPlayer = healthPlayerComponent.GetRefRW(entityA);
                healthPlayer.ValueRW.Health--;
                //Debug.Log("Health player " + healthPlayer.ValueRO.Health);
                
            }

        }
    }
}
