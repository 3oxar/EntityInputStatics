using Unity.Burst;
using Unity.Entities;

partial struct PlayerAddItemInventorySystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (playerInventory, playerInventoryAddItem, PlayerTag) in SystemAPI.Query<PlayerInventoryComponent ,RefRW<PlayerInventoryAddItem>, RefRO<PlayerTag>>())
        {
            if(playerInventoryAddItem.ValueRO.IndexItem != 0)
            {
                playerInventory.ItemPlayerInventory.Add(playerInventory.AllItemPlayer[((int)playerInventoryAddItem.ValueRO.IndexItem) - 1]);
                playerInventoryAddItem.ValueRW.IndexItem = 0;
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
