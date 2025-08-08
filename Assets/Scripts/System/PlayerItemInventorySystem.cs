using TMPro;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

partial class PlayerItemInventorySystem : SystemBase
{

    protected override void OnUpdate()
    {
        foreach (var (playerInventory, playerInventoryItem, PlayerTag, entity) in SystemAPI.Query<PlayerInventoryComponent, RefRW<PlayerInventoryItem>, RefRO<PlayerTag>>().WithEntityAccess())
        {
            //��� �������� �������� �������� ������ � �� ����� ������� ��������� ������� ������
            if (playerInventoryItem.ValueRO.IndexItem != 0)
            {
                var itemIndexAdd = (int)playerInventoryItem.ValueRO.IndexItem;
                if (playerInventory.itemCountPlayerInventory.ContainsKey((PickUpList)itemIndexAdd) == false)//���� �������� ��� ��� � ������
                {
                    playerInventory.ItemInventoryPlayer.Add(Object.Instantiate(playerInventory.AllItemPlayer[((int)playerInventoryItem.ValueRO.IndexItem) - 1], playerInventory.InventoryPlayer.transform, false));
                    playerInventory.itemCountPlayerInventory.Add((PickUpList)itemIndexAdd, 1);
                    playerInventory.ItemInventoryPlayer[itemIndexAdd - 1].GetComponentInChildren<TextMeshProUGUI>().text = "1";
                }
                else//���� ����, ������ ����������� ��� ���-��
                {
                    playerInventory.itemCountPlayerInventory[(PickUpList)itemIndexAdd] += 1;
                    playerInventory.ItemInventoryPlayer[itemIndexAdd - 1].GetComponent<ItemCountInventory>().Cout = playerInventory.itemCountPlayerInventory[(PickUpList)itemIndexAdd].ToString();
                }
                playerInventoryItem.ValueRW.IndexItem = 0;

            }

            
        }
    }
}
