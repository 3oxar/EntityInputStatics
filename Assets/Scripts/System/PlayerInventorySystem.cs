using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

partial struct PlayerInventorySystem : ISystem, ISystemStartStop
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
       
    }

    public void OnStartRunning(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (playerInventory, playerTag) in SystemAPI.Query<PlayerInventoryComponent, RefRO<PlayerTag>>())
        {
            var inventoryPanelFind = GameObject.FindWithTag("PlayerInventoryTag");
            playerInventory.InventoryPlayer = inventoryPanelFind.GetComponentInChildren<GridLayoutGroup>();
            playerInventory.InventoryPlayer.gameObject.SetActive(false);

            playerInventory.itemCountPlayerInventory = new System.Collections.Generic.Dictionary<PickUpList, int>();
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (playerInventory, playerTag) in SystemAPI.Query<PlayerInventoryComponent, RefRO<PlayerTag>>())
        {
            
        }
       
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }

  

    public void OnStopRunning(ref SystemState state)
    {
    }
}
