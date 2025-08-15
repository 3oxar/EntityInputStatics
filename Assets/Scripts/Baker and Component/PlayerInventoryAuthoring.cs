using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

class PlayerInventoryAuthoring : MonoBehaviour
{
    public Dictionary<PickUpList, int> itemCountPlayerInventory;//���-�� ��������� ������ ���� � ������
    public List<GameObject> AllItemPlayer;//��� ��������� �������� � ������ 
    public List<GameObject> ItemInventoryPlayer;//�������� � ��������� � ������
    public GridLayoutGroup InventoryPlayerGridLayout;//������ ����������� ��������� �� ������ 
    
}

class PlayerInventoryAuthoringBaker : Baker<PlayerInventoryAuthoring>
{
    public override void Bake(PlayerInventoryAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);

        AddComponentObject(entity, new PlayerInventoryComponent
        {
            AllItemPlayer = authoring.AllItemPlayer,
            InventoryPlayerGridLayout = authoring.InventoryPlayerGridLayout,
            itemCountPlayerInventory = authoring.itemCountPlayerInventory,
            ItemInventoryPlayer = authoring.ItemInventoryPlayer
            
        });

        AddComponent<PlayerInventoryItem>(entity);
    }
}
class PlayerInventoryComponent : IComponentData
{
    public Dictionary<PickUpList, int> itemCountPlayerInventory;
    public List<GameObject> AllItemPlayer;
    public GridLayoutGroup InventoryPlayerGridLayout;
    public List<GameObject> ItemInventoryPlayer;
}

struct PlayerInventoryItem : IComponentData
{
    public PickUpList IndexItem;
}


