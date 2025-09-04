using Unity.Entities;
using UnityEngine;

class PickUpAuthoring : MonoBehaviour
{
    public ItemPlayerAllList PickUpItemList;
}

class PickUpAuthoringBaker : Baker<PickUpAuthoring>
{
    public override void Bake(PickUpAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);
        
        AddComponent<PickUpTag>(entity);
        AddComponent(entity, new PickUpComponent
        {
            PickUpEntity = authoring.PickUpItemList
        });
            
    }
}

struct PickUpComponent : IComponentData
{
     public ItemPlayerAllList PickUpEntity;
}

struct PickUpTag : IComponentData
{

}

public enum ItemPlayerAllList
{
    nullItem = 0,
    Health = 1,
    Speed = 2,
    HealthCraftItem = 3,
    SpeedCraftItem = 4,
    MixedCraftItem = 5,
}