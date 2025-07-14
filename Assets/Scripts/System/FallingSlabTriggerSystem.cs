using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

partial struct FallingSlabTriggerSystem : ISystem
{
    internal ComponentDataHandles _ComponentDataHandles;
    internal struct ComponentDataHandles
    {
        public ComponentLookup<PlayerTag> playerTag;
        public ComponentLookup<FallingSlabTriggerTag> fallingSlabTriggerTag;
        public ComponentLookup<FallingSlabComponent> fallingSlabComponent;
       

        public ComponentDataHandles(ref SystemState systemState)
        {
            playerTag = systemState.GetComponentLookup<PlayerTag>(true);
            fallingSlabTriggerTag = systemState.GetComponentLookup<FallingSlabTriggerTag>(true);
            fallingSlabComponent = systemState.GetComponentLookup<FallingSlabComponent>(false);
        }

        public void Update(ref SystemState state)
        {
            playerTag.Update(ref state);
            fallingSlabTriggerTag.Update(ref state);
            fallingSlabComponent.Update(ref state);
        }
    }

    [BurstCompile]
    struct CollisionEventDamageEnterJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<PlayerTag> PlayerTag;
        [ReadOnly] public ComponentLookup<FallingSlabTriggerTag> FallingSlabTriggerTag;
        public ComponentLookup<FallingSlabComponent> FallingSlabComponent;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;
            if (PlayerTag.HasComponent(entityA) && FallingSlabTriggerTag.HasComponent(entityB))
            {
                var entityFalling = FallingSlabComponent.GetRefRW(FallingSlabTriggerTag.GetRefRO(entityB).ValueRO.FallingEntity);
                entityFalling.ValueRW.IsStartAnim = true;
            }
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
        foreach (var (falling, fallingTag) in SystemAPI.Query<FallingSlabComponent, RefRO<FallingSlabTag>>())
        {
            _ComponentDataHandles.Update(ref state);
            state.Dependency = new CollisionEventDamageEnterJob
            {
                PlayerTag = _ComponentDataHandles.playerTag,
                FallingSlabTriggerTag = _ComponentDataHandles.fallingSlabTriggerTag,
                FallingSlabComponent = _ComponentDataHandles.fallingSlabComponent
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
            state.Dependency.Complete();
        }
    }
   
}
