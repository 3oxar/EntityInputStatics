using NUnit.Framework;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

class CraftItemPlayerAuthoring : MonoBehaviour
{
   
}

class CraftItemPlayerBaker : Baker<CraftItemPlayerAuthoring>
{
    public override void Bake(CraftItemPlayerAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);

        AddComponentObject(entity, new CraftItemPlayerComponent
        {

        });
    }
}

class CraftItemPlayerComponent : IComponentData
{

    public ItemPlayerAllList IndexCraftItem;
    public GameObject ResultItemCraft;

    public bool isAddCraftItem;
}


