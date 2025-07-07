using DG.Tweening;
using UnityEngine;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

partial struct FallingSlabSystem : ISystem
{
    private bool _isRevertFall;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _isRevertFall = false;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (falling, proceduralAnim, localTransform, fallingTag, entity) in 
            SystemAPI.Query<RefRW<FallingSlabComponent>, ProceduralAnimationComponent, RefRW<LocalTransform>, RefRO<FallingSlabTag>>().WithEntityAccess())
        {
            var fall = Vector3.down.y * SystemAPI.Time.DeltaTime;

            if (localTransform.ValueRO.Position.y < falling.ValueRO.FallDistance.y && _isRevertFall == false)
                _isRevertFall = true;
            else if(localTransform.ValueRO.Position.y >= falling.ValueRO.StartFall.y && _isRevertFall == true)
            {
                _isRevertFall = false;
                falling.ValueRW.IsStartAnim = false;
            }

            if (_isRevertFall == true)
                fall *= (-1);

           if(falling.ValueRO.IsStartAnim == true)
            {
                proceduralAnim.Transform.DOMoveY(proceduralAnim.Transform.transform.position.y + fall, 0.03f);
                localTransform.ValueRW.Position = proceduralAnim.Transform.position;
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
