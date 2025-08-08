using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

class PlayerInventoryAuthoring : MonoBehaviour
{
    //public List<PickUpList> ItemPlayerInventory;
    public Dictionary<PickUpList, int> itemCountPlayerInventory;
    public List<GameObject> AllItemPlayer;
    public List<GameObject> ItemInventoryPlayer;
    public GridLayoutGroup InventoryPlayer;
    
}

class PlayerInventoryAuthoringBaker : Baker<PlayerInventoryAuthoring>
{
    public override void Bake(PlayerInventoryAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);

        AddComponentObject(entity, new PlayerInventoryComponent
        {
            //ItemPlayerInventory = authoring.ItemPlayerInventory,
            AllItemPlayer = authoring.AllItemPlayer,
            InventoryPlayer = authoring.InventoryPlayer,
            itemCountPlayerInventory = authoring.itemCountPlayerInventory,
            ItemInventoryPlayer = authoring.ItemInventoryPlayer
            
        });

        AddComponent<PlayerInventoryItem>(entity);
    }
}
class PlayerInventoryComponent : IComponentData
{
    //public List<PickUpList> ItemPlayerInventory;
    public Dictionary<PickUpList, int> itemCountPlayerInventory;
    public List<GameObject> AllItemPlayer;
    public GridLayoutGroup InventoryPlayer;
    public List<GameObject> ItemInventoryPlayer;
}

struct PlayerInventoryItem : IComponentData
{
    public PickUpList IndexItem;
}


