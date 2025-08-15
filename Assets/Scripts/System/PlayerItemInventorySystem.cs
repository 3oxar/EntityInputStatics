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
            //при поднятие предмета получаем индекс и по этому индексу добовляем предмет игроку
            if (playerInventoryItem.ValueRO.IndexItem != 0)
            {
                var itemIndexAdd = (int)playerInventoryItem.ValueRO.IndexItem;
                Debug.Log("Add Item");
                if (playerInventory.itemCountPlayerInventory.ContainsKey((PickUpList)itemIndexAdd) == false)//если предмета еще нет у игрока
                {
                    playerInventory.ItemInventoryPlayer.Add(Object.Instantiate(playerInventory.AllItemPlayer[((int)playerInventoryItem.ValueRO.IndexItem) - 1], playerInventory.InventoryPlayerGridLayout.transform, false));

                    playerInventory.itemCountPlayerInventory.Add((PickUpList)itemIndexAdd, 1);
                    playerInventory.ItemInventoryPlayer[playerInventory.ItemInventoryPlayer.Count -1].GetComponent<ItemCountInventory>().Cout = "1";
                }
                else//если есть, просто увеличиваем его кол-во
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
