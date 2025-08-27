using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

partial class PlayerItemInventorySystem : SystemBase
{

    private List<ItemPlayerAllList> _indexRemove;

    protected override void OnUpdate()
    {
        foreach (var (playerInventory, playerInventoryItem, PlayerTag, entity) in SystemAPI.Query<PlayerInventoryComponent, RefRW<PlayerInventoryItem>, RefRO<PlayerTag>>().WithEntityAccess())
        {
            //��� �������� �������� �������� ������ � �� ����� ������� ��������� ������� ������
            if (playerInventoryItem.ValueRO.IndexItem != 0)
            {
                var itemIndexAdd = (int)playerInventoryItem.ValueRO.IndexItem;
                Debug.Log("Add Item");
                if (playerInventory.itemCountPlayerInventory.ContainsKey((ItemPlayerAllList)itemIndexAdd) == false)//���� �������� ��� ��� � ������
                {
                    playerInventory.ItemInventoryPlayer.Add(UnityEngine.Object.Instantiate(playerInventory.AllItemPlayer[((int)playerInventoryItem.ValueRO.IndexItem) - 1], playerInventory.InventoryPlayerGridLayout.transform, false));

                    playerInventory.itemCountPlayerInventory.Add((ItemPlayerAllList)itemIndexAdd, 1);
                    TextCoutItem(playerInventory);

                }
                else//���� ����, ������ ����������� ��� ���-��
                {
                    playerInventory.itemCountPlayerInventory[(ItemPlayerAllList)itemIndexAdd] += 1;
                    TextCoutItem(playerInventory);
                }
                playerInventoryItem.ValueRW.IndexItem = 0;
            }
        }

        foreach(var (craftItem, playerInventory, playerInventoryItem, PlayerTag, entity) in
            SystemAPI.Query<CraftItemPlayerComponent, PlayerInventoryComponent, RefRW<PlayerInventoryItem>, RefRO<PlayerTag>>().WithEntityAccess())
        {
            if(craftItem.isAddCraftItem == true)
            {
                craftItem.isAddCraftItem = false;

                if(playerInventory.itemCountPlayerInventory.ContainsKey(craftItem.IndexCraftItem) == false)
                {
                    playerInventory.ItemInventoryPlayer.Add(UnityEngine.Object.Instantiate(playerInventory.AllItemPlayer[(int)craftItem.IndexCraftItem - 1], playerInventory.InventoryPlayerGridLayout.transform, false));
                    playerInventory.itemCountPlayerInventory.Add(craftItem.IndexCraftItem, 1);
                    TextCoutItem(playerInventory);
                }
                else
                {
                    playerInventory.itemCountPlayerInventory[craftItem.IndexCraftItem] += 1;
                    TextCoutItem(playerInventory);
                }

                _indexRemove = new();
                foreach(var item in playerInventory.itemCountPlayerInventory)
                {
                    if (playerInventory.itemCountPlayerInventory[item.Key] <= 0)
                    {
                        _indexRemove.Add(item.Key);//���������� ����� �������� �� 0
                    }
                }

                if (_indexRemove.Count != 0)
                {
                    foreach (var item in _indexRemove)//������� �������� ������� ����������� ����� ������ 
                    {
                        var itemInventoryIndex = playerInventory.ItemInventoryPlayer.FindIndex(x => x.GetComponent<ItemTag>().PickUpItemTag == item);
                        playerInventory.ItemInventoryPlayer.Remove(playerInventory.ItemInventoryPlayer[itemInventoryIndex]);//�������� �� ������ ��������� � ������
                        playerInventory.itemCountPlayerInventory.Remove(item);//�������� �� ������������ ���������� �������� � ���������
                    }
                }
            }
        }
    }

    /// <summary>
    /// ����� � UI ���-�� ���������
    /// </summary>
    /// <param name="playerInventory"></param>
    private void TextCoutItem(PlayerInventoryComponent playerInventory)
    {
        for (int i = 0; i < playerInventory.ItemInventoryPlayer.Count; i++)
        {
            var indexItemTag = playerInventory.ItemInventoryPlayer[i].GetComponent<ItemTag>().PickUpItemTag;
            playerInventory.ItemInventoryPlayer[i].GetComponent<ItemCountInventory>().Cout = playerInventory.itemCountPlayerInventory[indexItemTag].ToString();
        }
    }
}


