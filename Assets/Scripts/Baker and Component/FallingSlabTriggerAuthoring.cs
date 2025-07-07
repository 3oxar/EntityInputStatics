using Unity.Entities;
using UnityEngine;

class FallingSlabTriggerAuthoring : MonoBehaviour
{
   public GameObject FallingEntity;
}

class FallingSlabTriggerAuthoringBaker : Baker<FallingSlabTriggerAuthoring>
{
    public override void Bake(FallingSlabTriggerAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);

        AddComponent(entity, new FallingSlabTriggerTag{
            FallingEntity = GetEntity(authoring.FallingEntity, TransformUsageFlags.Dynamic)
        });
    }
}

public struct FallingSlabTriggerTag : IComponentData
{
    public Entity FallingEntity;
}
