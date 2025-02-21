using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct MovePlayerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, transformLocal, input) in SystemAPI.Query<MovePlayerComponent, RefRW<LocalTransform>, RefRO<InputComponent>>())
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            transformLocal.ValueRW.Position = transformLocal.ValueRW.TransformPoint(new float3(input.ValueRO.Move.x * deltaTime, 0, input.ValueRO.Move.y * deltaTime) * -1);
            transform.TransformPlayer.position = transformLocal.ValueRW.Position;
            
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
