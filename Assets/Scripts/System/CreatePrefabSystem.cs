using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;

partial struct CreatePrefabSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ECB = new EntityCommandBuffer(Allocator.Temp);
         
        foreach (var (item, input) in SystemAPI.Query<CreatePrefabComponet, RefRO<InputComponent>>())
        {
            if (input.ValueRO.CreatePrefab > 0)//создание игрового персонажа 
            {
                var instance = ECB.Instantiate(item.Entity);
                var transform = LocalTransform.FromPositionRotation(item.Transform.position, item.Rotation);
                ECB.SetComponent(instance, transform);
            }
        }

        ECB.Playback(state.EntityManager);
        ECB.Dispose();
    }
}
