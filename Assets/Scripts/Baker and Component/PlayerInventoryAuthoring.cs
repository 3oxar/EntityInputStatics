using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

class PlayerInventoryAuthoring : MonoBehaviour
{
    public List<GameObject> ItemPlayerInventory;
    public List<GameObject> AllItemPlayer;
}

class PlayerInventoryAuthoringBaker : Baker<PlayerInventoryAuthoring>
{
    public override void Bake(PlayerInventoryAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);

        AddComponentObject(entity, new PlayerInventoryComponent
        {
            ItemPlayerInventory = authoring.ItemPlayerInventory,
            AllItemPlayer = authoring.AllItemPlayer
        });

        AddComponent<PlayerInventoryAddItem>(entity);
    }
}
class PlayerInventoryComponent : IComponentData
{
    public List<GameObject> ItemPlayerInventory;
    public List<GameObject> AllItemPlayer;


}

struct PlayerInventoryAddItem : IComponentData
{
    public PickUpList IndexItem;
}
