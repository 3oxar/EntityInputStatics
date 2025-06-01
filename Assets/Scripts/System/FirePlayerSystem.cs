using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct FirePlayerSystem : ISystem
{
    private float _fireReload;


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ECB = new EntityCommandBuffer(Allocator.Temp);
        
        foreach(var (fire, input, playerTag, entity) in SystemAPI.Query<FirePlayerComponent, RefRW<InputComponent>, RefRO<PlayerTag>>().WithEntityAccess())
        {
            if(input.ValueRO.Fire > 0 && _fireReload == 0)
            {
                _fireReload = 0.5f;
                var instance = ECB.Instantiate(fire.Bullet);
                var transform = LocalTransform.FromPositionRotationScale(new float3(fire.TransformCreateBullet.position.x, fire.TransformCreateBullet.position.y + 0.7f, 
                    fire.TransformCreateBullet.position.z - 1f), Quaternion.Euler(90,180,0), 0.1f);
                
                ECB.SetComponent(instance, transform);
            }
        }

        if (_fireReload > 0)//перезарядка стрельбы 
        {
            _fireReload -= SystemAPI.Time.DeltaTime;
        }
        else if (_fireReload < 0)
            _fireReload = 0;

        ECB.Playback(state.EntityManager);
        ECB.Dispose();
    }
}
