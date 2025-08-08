using Unity.Entities;
using UnityEngine;

public class UseItemPlayer : MonoBehaviour
{
    private void OnEnable()
    {
        var useItemPlayerSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<UseItemPlayerSystem>();
        useItemPlayerSystem.OnUseItemHealth += UseItem;
    }

    private void OnDisable()
    {
        var useItemPlayerSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<UseItemPlayerSystem>();
        useItemPlayerSystem.OnUseItemHealth -= UseItem;

    }

    public void UseItem()
    {
       
    }
}

