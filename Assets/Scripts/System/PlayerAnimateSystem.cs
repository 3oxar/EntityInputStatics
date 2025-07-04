using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct PlayerAnimateSystem : ISystem
{
    private int _healthPlayer;
    private float _timeHitAnim;

    public void OnCreate(ref SystemState state)
    {
        _timeHitAnim = 0.4f;
        foreach (var (playerHealth, player) in SystemAPI.Query<RefRO<HealthPlayerComponent>, RefRO<PlayerTag>>())
        {
            _healthPlayer = playerHealth.ValueRO.Health;
        }
    }

    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach(var (playerPrefab, entity) in SystemAPI.Query<PlayerGameObjectPrefab>().WithNone<PlayerAnimationReference>().WithEntityAccess())
        {
            var newCompanionObject = Object.Instantiate(playerPrefab.PrefabPlayer);
            var newAnimatorReference = new PlayerAnimationReference
            {
                AnimationPlayer = newCompanionObject.GetComponent<Animator>()
            };
            ecb.AddComponent(entity, newAnimatorReference);
        }

        foreach(var (transform, animatorReference, input) in SystemAPI.Query<LocalTransform, PlayerAnimationReference, InputComponent>())//анимация бега
        {
            animatorReference.AnimationPlayer.SetBool("IsMoving", math.length(input.Move) > 0f);
            animatorReference.AnimationPlayer.transform.position = transform.Position;
            animatorReference.AnimationPlayer.transform.rotation = transform.Rotation;
        }

        foreach (var (animatorReference, input) in SystemAPI.Query<PlayerAnimationReference, InputComponent>())//анимация атаки 
        {
            animatorReference.AnimationPlayer.SetBool("IsAttack", math.length(input.Fire) > 0f);
        }

        foreach (var (healthPlayer,animatorReference, input) in SystemAPI.Query<RefRO<HealthPlayerComponent>,PlayerAnimationReference, InputComponent>())//анимация получения урона и смерти
        {
            if(healthPlayer.ValueRO.Health < _healthPlayer && _timeHitAnim != 0 && animatorReference.AnimationPlayer.GetBool("IsHit") == false)
            {
                _healthPlayer = healthPlayer.ValueRO.Health;
                animatorReference.AnimationPlayer.SetBool("IsHit", true);
            }
            else if(_timeHitAnim <= 0)
            {
                animatorReference.AnimationPlayer.SetBool("IsHit", false);
                _timeHitAnim = 0.4f;
            }

            if (healthPlayer.ValueRO.Health > _healthPlayer)
                _healthPlayer = healthPlayer.ValueRO.Health;

            if (_timeHitAnim > 0 && animatorReference.AnimationPlayer.GetBool("IsHit") == true)
                _timeHitAnim -= SystemAPI.Time.DeltaTime;

            if(healthPlayer.ValueRO.Health <= 0 && animatorReference.AnimationPlayer.GetBool("IsDie") == false)
            {
                animatorReference.AnimationPlayer.SetBool("IsDie", true);
            }
        }

        foreach (var (animatorReference, entity) in SystemAPI.Query<PlayerAnimationReference>().WithNone<PlayerGameObjectPrefab, LocalTransform>().WithEntityAccess())
        {
            Object.Destroy(animatorReference.AnimationPlayer.gameObject);
            ecb.RemoveComponent<PlayerAnimationReference>(entity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
