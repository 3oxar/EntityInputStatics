using Unity.Entities;
using UnityEngine;

class DamageAuthoting : MonoBehaviour
{
    
}

class DamageAuthotingBaker : Baker<DamageAuthoting>
{
    public override void Bake(DamageAuthoting authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);

        AddComponent(entity, new DamageTag());
        AddComponent(entity, new DamageComponent());
    }
}


struct DamageComponent : IComponentData
{

}