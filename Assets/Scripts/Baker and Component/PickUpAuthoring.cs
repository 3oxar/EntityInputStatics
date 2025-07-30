using Unity.Entities;
using UnityEngine;

class PickUpAuthoring : MonoBehaviour
{
    public PickUpList PickUpItemList;
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
     public PickUpList PickUpEntity;
}

struct PickUpTag : IComponentData
{

}

enum PickUpList
{
    nullItem = 0,
    Health = 1,
    Speed = 2
}