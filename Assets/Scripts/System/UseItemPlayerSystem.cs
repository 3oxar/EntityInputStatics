using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

partial class UseItemPlayerSystem : SystemBase
{
    public float _reloadUseItem;
    public Action OnUseItemHealth;

    private EntityManager _entityManager;
    private EntityCommandBuffer _ecb;

    protected override void OnCreate()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override void OnStartRunning()
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (playerInventory, playerTag) in SystemAPI.Query<PlayerInventoryComponent, RefRO<PlayerTag>>())
        {

        }

        ecb.Playback(_entityManager);
        ecb.Dispose();
    }

    protected override void OnUpdate()
    {
        _ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var (inventoryPlayer, input, playerTag, entity) in SystemAPI.Query<PlayerInventoryComponent, RefRW<InputComponent>, RefRO<PlayerTag>>().WithEntityAccess())
        {
            if (input.ValueRO.UseItem != 0 && _reloadUseItem == 0)
            {
                Debug.Log("useItem");

                input.ValueRW.UseItem = 0;
                _reloadUseItem = 1f;

                var healthPlayer = _entityManager.GetComponentData<HealthPlayerComponent>(entity);
                Debug.Log(_entityManager.GetName(entity));
                healthPlayer.Health += 5;
                _ecb.SetComponent(entity, healthPlayer);

                inventoryPlayer.itemCountPlayerInventory[(PickUpList)1] -= 1;
                inventoryPlayer.ItemInventoryPlayer[0].GetComponent<ItemCountInventory>().Cout = inventoryPlayer.itemCountPlayerInventory[(PickUpList)1].ToString();

                if(inventoryPlayer.itemCountPlayerInventory[(PickUpList)1] <= 0)
                {
                    Debug.Log("remove");
                    inventoryPlayer.ItemInventoryPlayer.RemoveAt(0);
                }
            }

            if (_reloadUseItem != 0 && input.ValueRO.UseItem == 0)
            {
                _reloadUseItem -= SystemAPI.Time.DeltaTime;

                if (_reloadUseItem < 0)
                    _reloadUseItem = 0;
            }

           
        }
        //OnUseItemHealth?.Invoke();

        _ecb.Playback(_entityManager);
        _ecb.Dispose();



    }
}
