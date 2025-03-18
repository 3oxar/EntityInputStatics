using Unity.Entities;
using UnityEngine;

class DamageAuthoting : MonoBehaviour
{
    public float TimeReloadDamage;
}

class DamageAuthotingBaker : Baker<DamageAuthoting>
{
    public override void Bake(DamageAuthoting authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);

        AddComponent(entity, new DamageTag());
        AddComponent(entity, new DamageComponent
        {
            TimeReloadDamage = authoring.TimeReloadDamage
        });
    }
}


struct DamageComponent : IComponentData
{
    public float TimeReloadDamage;//промежуток нанесения урона
}