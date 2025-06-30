using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct PlayerAnimateSystem : ISystem
{
  
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

        foreach(var (transform, animatorReference, moveInput) in SystemAPI.Query<LocalTransform, PlayerAnimationReference, InputComponent>())
        {
            animatorReference.AnimationPlayer.SetBool("IsMoving", math.length(moveInput.Move) > 0f);
            animatorReference.AnimationPlayer.transform.position = transform.Position;
            animatorReference.AnimationPlayer.transform.rotation = transform.Rotation;
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
