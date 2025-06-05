using Unity.Entities;
using UnityEngine;

class ShaderPlayerAuthoring : MonoBehaviour
{
    public Shader StandartPlayerShared;
    public Shader FrozenPlayerShared;
    public Shader DamagePlayerShared;
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
            StandartPlayerShared = authoring.StandartPlayerShared,
            FrozenPlayerShared = authoring.FrozenPlayerShared,
            DamagePlayerShared = authoring.DamagePlayerShared,
            SkinnedMeshRender = skinndeMesh
        });
    }
}

public class ShaderPlayerComponent : IComponentData
{
    public Shader StandartPlayerShared;
    public Shader FrozenPlayerShared;
    public Shader DamagePlayerShared;
    public SkinnedMeshRenderer SkinnedMeshRender;
}
