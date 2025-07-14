using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct JerkPlayerSystem : ISystem
{
    private float _timeJerk;
    private bool _isjerk;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _isjerk = false;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, inputPlayer) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<InputComponent>>())
        {
            if (inputPlayer.ValueRW.Jerk != 0 && _isjerk == false)
            {
                _isjerk = true;
                _timeJerk = 0.2f;
            }
            switch (_isjerk)//движение во время рывка
            {
                case true:
                    //transform.ValueRW.Position = transform.ValueRW.TransformPoint(new float3(inputPlayer.ValueRW.Move.x * 5f * SystemAPI.Time.DeltaTime, 0, inputPlayer.ValueRW.Move.y * 5f * SystemAPI.Time.DeltaTime) * -1);
                    transform.ValueRW.Position += new float3(inputPlayer.ValueRW.Move.x * 5f * SystemAPI.Time.DeltaTime, 0, inputPlayer.ValueRW.Move.y * 5f * SystemAPI.Time.DeltaTime);
                    _timeJerk -= SystemAPI.Time.DeltaTime;
                    if (_timeJerk <= 0)
                    {
                        _isjerk = false;
                    }
                    break;
            }
        }
    }
}
