using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

partial struct PickUpItemSystem : ISystem
{
    internal ComponentDataHandles _ComponentDataHandles;

    private EntityCommandBuffer _ecb;

    internal struct ComponentDataHandles
    {
        public ComponentLookup<PlayerTag> PlayerTag;
        public ComponentLookup<PickUpTag> PickUpTag;
        public ComponentLookup<PickUpComponent> PickUpComponent;
        public ComponentLookup<PlayerInventoryAddItem> PlayerInventoryAddItem;

        public ComponentDataHandles(ref SystemState systemState)
        {
            PlayerTag = systemState.GetComponentLookup<PlayerTag>(true);
            PickUpTag = systemState.GetComponentLookup<PickUpTag>(true);
            PickUpComponent = systemState.GetComponentLookup<PickUpComponent>(true);
            PlayerInventoryAddItem = systemState.GetComponentLookup<PlayerInventoryAddItem>(false);
            
        }

        public void Update(ref SystemState state)
        {
            PlayerTag.Update(ref state);
            PickUpTag.Update(ref state);
            PickUpComponent.Update(ref state);
            PlayerInventoryAddItem.Update(ref state);
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
        _ecb = new EntityCommandBuffer(Allocator.TempJob);

        foreach (var (PickUp,PickUpTag, entity) in SystemAPI.Query<RefRO<PickUpComponent> ,RefRO<PickUpTag>>().WithEntityAccess())
        {
            _ComponentDataHandles.Update(ref state);
            state.Dependency = new PickUpItemJob
            {
                PickUpTag = _ComponentDataHandles.PickUpTag,
                PlayerTag = _ComponentDataHandles.PlayerTag,
                PickUpComponent = _ComponentDataHandles.PickUpComponent,
                PlayerInventoryAddItem = _ComponentDataHandles.PlayerInventoryAddItem,
                EntityCommandBuffer = _ecb,
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
            state.Dependency.Complete();
        }

        _ecb.Playback(state.EntityManager);
        _ecb.Dispose();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }

     struct PickUpItemJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentLookup<PlayerTag> PlayerTag;
        [ReadOnly] public ComponentLookup<PickUpTag> PickUpTag;
        [ReadOnly] public ComponentLookup<PickUpComponent> PickUpComponent;
        public ComponentLookup<PlayerInventoryAddItem> PlayerInventoryAddItem;
        public EntityCommandBuffer EntityCommandBuffer;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            if (PlayerTag.HasComponent(entityA) && PickUpTag.HasComponent(entityB))
            {
                var indexItem = PickUpComponent.GetRefRO(entityB).ValueRO.PickUpEntity;
                var addItem = PlayerInventoryAddItem.GetRefRW(entityA);
                addItem.ValueRW.IndexItem = indexItem;
                
                EntityCommandBuffer.DestroyEntity(entityB);
            }



        }
    }
}
