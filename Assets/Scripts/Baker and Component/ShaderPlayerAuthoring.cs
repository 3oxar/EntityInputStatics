using Unity.Entities;
using UnityEngine;

class ShaderPlayerAuthoring : MonoBehaviour
{
    public Material StandartPlayerMaterial;
    public Material FrozenPlayerMaterial;
    public Material DamagePlayerMaterial;
    public GameObject SkinnedMeshRender;
}

class ShaderPlayerAuthoringBaker : Baker<ShaderPlayerAuthoring>
{
    public override void Bake(ShaderPlayerAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);

        var skinndeMesh = authoring.SkinnedMeshRender.GetComponent<SkinnedMeshRenderer>();

        AddComponentObject(entity, new ShaderPlayerComponent
        {
            StandartPlayerMaterial = authoring.StandartPlayerMaterial,
            FrozenPlayerMaterial = authoring.FrozenPlayerMaterial,
            DamagePlayerMaterial = authoring.DamagePlayerMaterial,
            SkinnedMeshRender = skinndeMesh
        });
    }
}

public class ShaderPlayerComponent : IComponentData
{
    public Material StandartPlayerMaterial;
    public Material FrozenPlayerMaterial;
    public Material DamagePlayerMaterial;
    public SkinnedMeshRenderer SkinnedMeshRender;
}
