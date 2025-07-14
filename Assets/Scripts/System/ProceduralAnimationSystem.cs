using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using DG.Tweening;
using Unity.Collections;

partial struct ProceduralAnimationSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //foreach (var (proceduralAnim, localTransform, cubeDamage, entity) in SystemAPI.Query<ProceduralAnimationComponent,RefRW<LocalTransform>, RefRO<DamageTag>>().WithEntityAccess())
        //{
        //proceduralAnim.Transform.DOMove(proceduralAnim.Transform.transform.position + Vector3.forward * SystemAPI.Time.DeltaTime, 0.1f);
        //localTransform.ValueRW.Position = proceduralAnim.Transform.position;
        //}
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
