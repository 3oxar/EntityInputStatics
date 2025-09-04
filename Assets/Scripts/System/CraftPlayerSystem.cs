using System;
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
    private bool _isStartCraft;
    
    public List<ItemPlayerAllList> FormulaItem { get => _formulaItem; set => _formulaItem = value; }
    public List<int> CoutNeedItem { get => _coutNeedItem; set => _coutNeedItem = value; }
    public GameObject ResultItemCraft { get => _resultItemCraft; set => _resultItemCraft = value; }
    public bool IsCraft { get => _isCraft; set => _isCraft = value; }
    public ItemPlayerAllList IndexCraftItem { get => _indexCraftItem; set => _indexCraftItem = value; }

    protected override void OnUpdate()
    {
        foreach(var (cratfItem, playerInventory, playerTag) in SystemAPI.Query<CraftItemPlayerComponent, PlayerInventoryComponent, RefRO<PlayerTag>>())
        {
            if (_isCraft == true)
            {
                _isCraft = false;
                _isStartCraft = false;
                foreach (var item in _formulaItem)//сначало проверяем все ли предметы есть по кол-во у игрока
                {
                    foreach (var coutItem in _coutNeedItem)
                    {
                        if (playerInventory.itemCountPlayerInventory[item] < coutItem)
                        {
                            _isStartCraft = false;
                            return;
                        }
                        else
                            _isStartCraft = true;
                    }
                }

                if (_isStartCraft == true)
                {
                    foreach (var item in _formulaItem)
                    {
                        foreach (var coutItem in _coutNeedItem)//отнимаем предметы 
                        {
                            playerInventory.itemCountPlayerInventory[item] -= coutItem;
                        }
                        //начинаем крафт
                        cratfItem.IndexCraftItem = _indexCraftItem;
                        cratfItem.ResultItemCraft = _resultItemCraft;
                        cratfItem.isAddCraftItem = true;
                        cratfItem.ItemIndex = item;
                    }
                }



            }
        }
    }
}
