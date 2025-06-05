using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;


partial struct ShaderPlayerSystem : ISystem
{
    private bool _isChangedFrozenStandartShader;
    private float _changedShaderReload;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _isChangedFrozenStandartShader = false;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (material, input, playerTag, entity) in SystemAPI.Query<ShaderPlayerComponent,RefRW<InputComponent>, RefRO<PlayerTag>>().WithEntityAccess())
        {
            if (input.ValueRO.ChangedShader != 0 && _isChangedFrozenStandartShader == false && _changedShaderReload == 0)
            {
                material.SkinnedMeshRender.sharedMaterial.shader = material.FrozenPlayerShared;
                _isChangedFrozenStandartShader = true;
                _changedShaderReload = 1f;
            }
            else if (input.ValueRO.ChangedShader != 0 && _isChangedFrozenStandartShader == true && _changedShaderReload == 0)
            {

                material.SkinnedMeshRender.sharedMaterial.shader = material.StandartPlayerShared;
                _isChangedFrozenStandartShader = false;
                _changedShaderReload = 1f;
            }

            if (_changedShaderReload > 0)
            {
                _changedShaderReload -= SystemAPI.Time.DeltaTime;
            }
            else if (_changedShaderReload < 0)
                _changedShaderReload = 0;
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

