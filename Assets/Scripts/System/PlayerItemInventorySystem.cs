using TMPro;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

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
                Debug.Log("Add Item");
                if (playerInventory.itemCountPlayerInventory.ContainsKey((PickUpList)itemIndexAdd) == false)//���� �������� ��� ��� � ������
                {
                    playerInventory.ItemInventoryPlayer.Add(Object.Instantiate(playerInventory.AllItemPlayer[((int)playerInventoryItem.ValueRO.IndexItem) - 1], playerInventory.InventoryPlayerGridLayout.transform, false));

                    playerInventory.itemCountPlayerInventory.Add((PickUpList)itemIndexAdd, 1);
                    playerInventory.ItemInventoryPlayer[playerInventory.ItemInventoryPlayer.Count -1].GetComponent<ItemCountInventory>().Cout = "1";
                }
                else//���� ����, ������ ����������� ��� ���-��
                {
                    playerInventory.itemCountPlayerInventory[(PickUpList)itemIndexAdd] += 1;
                    var itemInventoryIndex = playerInventory.ItemInventoryPlayer.FindIndex(x => x.GetComponent<ItemTag>().PickUpItemTag == (PickUpList)itemIndexAdd);
                    playerInventory.ItemInventoryPlayer[itemInventoryIndex].GetComponent<ItemCountInventory>().Cout = playerInventory.itemCountPlayerInventory[(PickUpList)itemIndexAdd].ToString();
                }
                playerInventoryItem.ValueRW.IndexItem = 0;

            }

            
        }
    }
}
