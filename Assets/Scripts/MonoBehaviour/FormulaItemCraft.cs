using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class FormulaItemCraft : MonoBehaviour
{
    public List<ItemPlayerAllList> FormulaItem;
    public List<int> CoutNeedItem;

    public ItemPlayerAllList IndexCraftItem;
    public GameObject ResultItemCraft;

    private bool _isCraft;

    public void CraftItem()
    {
        var useCraftPlayerSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<CraftPlayerSystem>();

        useCraftPlayerSystem.FormulaItem = FormulaItem;
        useCraftPlayerSystem.CoutNeedItem = CoutNeedItem;
        useCraftPlayerSystem.ResultItemCraft = ResultItemCraft;
        useCraftPlayerSystem.IsCraft = true;
        useCraftPlayerSystem.IndexCraftItem = IndexCraftItem;
        
    }
}
