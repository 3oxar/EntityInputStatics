using Unity.Entities;
using UnityEngine;

class HealthUpAuthoring : MonoBehaviour
{
    public int HealthUp;
}

class HealthUpAuthoringBaker : Baker<HealthUpAuthoring>
{
    public override void Bake(HealthUpAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);

        AddComponent(entity, new HealthUpComponent
        {
            HealthUp = authoring.HealthUp
        });
    }
}

struct HealthUpComponent : IComponentData
{
    public int HealthUp;
}
