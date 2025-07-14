using Unity.Entities;
using UnityEngine;

class ProceduralAnimationAuthoring : MonoBehaviour
{
    public Transform Transform;
}

class ProceduralAnimationAuthoringBaker : Baker<ProceduralAnimationAuthoring>
{
    public override void Bake(ProceduralAnimationAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

        AddComponentObject(entity, new ProceduralAnimationComponent
        {
            Transform = authoring.Transform
        });
    }
}

public class ProceduralAnimationComponent : IComponentData
{
    public Transform Transform;
}
