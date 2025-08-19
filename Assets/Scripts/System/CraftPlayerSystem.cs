using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

partial class CraftPlayerSystem : SystemBase
{
    private List<ItemPlayerAllList> _formulaItem;
    private List<int> _coutNeedItem;

    private ItemPlayerAllList _indexCraftItem;
    private GameObject _resultItemCraft;

    private bool _isCraft;

    public List<ItemPlayerAllList> FormulaItem { get => _formulaItem; set => _formulaItem = value; }
    public List<int> CoutNeedItem { get => _coutNeedItem; set => _coutNeedItem = value; }
    public GameObject ResultItemCraft { get => _resultItemCraft; set => _resultItemCraft = value; }
    public bool IsCraft { get => _isCraft; set => _isCraft = value; }
    public ItemPlayerAllList IndexCraftItem { get => _indexCraftItem; set => _indexCraftItem = value; }

    protected override void OnUpdate()
    {
        foreach(var (cratfItem, playerInventory, playerTag) in SystemAPI.Query<CraftItemPlayerComponent, PlayerInventoryComponent, RefRO<PlayerTag>>())
        {
            if(_isCraft == true)
            {
                _isCraft = false;

                //удалить потом
                foreach (var item in _formulaItem)
                {
                    Debug.Log("нужные предметы - " + item);
                }
                foreach (var item in _coutNeedItem)
                {
                    Debug.Log("кол-во - " + item);
                }

                foreach (var item in _formulaItem)
                {
                    if(playerInventory.itemCountPlayerInventory.ContainsKey(item) == true)
                    {
                        Debug.Log("Предмет есть у игрока");

                    }
                    else
                    {
                        Debug.Log("Предмета нет");
                        return;
                    }

                    foreach(var coutItem in _coutNeedItem)
                    {
                        if (playerInventory.itemCountPlayerInventory[item] >= coutItem)
                        {
                            Debug.Log("Кол-во предметов достаточно у игрока");
                            playerInventory.itemCountPlayerInventory[item] -= coutItem;

                            Debug.Log("Craft");
                            cratfItem.IndexCraftItem = _indexCraftItem;
                            cratfItem.ResultItemCraft = _resultItemCraft;
                            cratfItem.isAddCraftItem = true;
                        }
                        else
                        {
                            Debug.Log("не хватает.");
                            return;
                        }
                    }


                }


                
            }
        }
    }
}
