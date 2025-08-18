using NUnit.Framework;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

class CraftItemPlayer : MonoBehaviour
{
   
}

class CraftItemPlayerBaker : Baker<CraftItemPlayer>
{
    public override void Bake(CraftItemPlayer authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);

        AddComponentObject(entity, new CraftItemPlayerComponent
        {

        });
    }
}

class CraftItemPlayerComponent : IComponentData
{
}
