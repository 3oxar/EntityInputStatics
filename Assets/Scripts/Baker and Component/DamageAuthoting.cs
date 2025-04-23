using Unity.Entities;
using UnityEngine;

public class DamageAuthoting : MonoBehaviour
{
    [HideInInspector] public bool IsDamage = false; 
    public float TimeReloadDamage;
}

public class DamageAuthotingBaker : Baker<DamageAuthoting>
{
    public override void Bake(DamageAuthoting authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);

        AddComponent(entity, new DamageTag());
        AddComponent(entity, new DamageComponent
        {
            TimeReloadDamage = authoring.TimeReloadDamage,
            IsDamage = authoring.IsDamage
        });
    }
}


public struct DamageComponent : IComponentData
{
    public float TimeReloadDamage;//промежуток нанесения урона
    public bool IsDamage;
}