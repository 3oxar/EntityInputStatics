using Unity.Entities;
using UnityEngine;

class FallingSlabAuthoring : MonoBehaviour
{
    public bool IsStartAnim;
    public Vector3 FallDistance;
    public Vector3 StartFall;
}

class FallingSlabAuthoringBaker : Baker<FallingSlabAuthoring>
{
    public override void Bake(FallingSlabAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
        AddComponent<FallingSlabTag>(entity);
        AddComponent(entity, new FallingSlabComponent
        {
            IsStartAnim = authoring.IsStartAnim,
            FallDistance = authoring.FallDistance,
            StartFall = authoring.StartFall
        });
    }
}

public struct FallingSlabComponent : IComponentData 
{
    public bool IsStartAnim;
    public Vector3 FallDistance;
    public Vector3 StartFall;

}

public struct FallingSlabTag : IComponentData
{

}