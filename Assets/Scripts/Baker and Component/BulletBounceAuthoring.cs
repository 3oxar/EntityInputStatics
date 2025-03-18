using Unity.Entities;
using UnityEngine;

class BulletBounceAuthoring : MonoBehaviour
{
    public bool IsBulletBounce;
    public float TimeBulletBounce;
}

class BulletBounceAuthoringBaker : Baker<BulletBounceAuthoring>
{
    public override void Bake(BulletBounceAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);

        AddComponent(entity, new BulletBounceComponent
        {
            IsBulletBounce = authoring.IsBulletBounce,
            TimeBulletBounce = authoring.TimeBulletBounce
        });
    }
}

struct BulletBounceComponent : IComponentData
{
    public bool IsBulletBounce;
    public float TimeBulletBounce;
}
