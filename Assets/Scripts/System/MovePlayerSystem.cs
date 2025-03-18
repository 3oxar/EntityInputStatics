using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct MovePlayerSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, transformLocal, input) in SystemAPI.Query<MovePlayerComponent, RefRW<LocalTransform>, RefRO<InputComponent>>())
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            transformLocal.ValueRW.Position = transformLocal.ValueRW.TransformPoint(new float3(input.ValueRO.Move.x * deltaTime, 0, input.ValueRO.Move.y * deltaTime) * -1);//умножаем на (-1) потому что персонаж развернут на 180
            transform.TransformPlayer.position = transformLocal.ValueRW.Position;

        }
    }
}
