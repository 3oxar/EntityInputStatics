using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct MovePlayerSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, transformLocal, input) in SystemAPI.Query<MovePlayerComponent, RefRW<LocalTransform>, RefRO<InputComponent>>())
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            transformLocal.ValueRW.Position +=new float3(input.ValueRO.Move.x * transform.Speed * deltaTime, 0, input.ValueRO.Move.y * transform.Speed * deltaTime);
            transform.TransformPlayer.position = transformLocal.ValueRW.Position;

            var dir = new Vector3(input.ValueRO.Move.x, 0, input.ValueRO.Move.y);

            if (dir == Vector3.zero) return;

            var rot = transform.TransformPlayer.rotation;
            var newRot = Quaternion.LookRotation(Vector3.Normalize(dir));
            if (newRot == rot) return;

            transformLocal.ValueRW.Rotation = Quaternion.Lerp(rot, newRot, deltaTime * 10);
            transform.TransformPlayer.rotation = transformLocal.ValueRO.Rotation;

        }
    }
}
