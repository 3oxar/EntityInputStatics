using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

partial class UseItemPlayerSystem : SystemBase
{
    public Action DestoyObject;

    private float _reloadUseItem;
    private EntityManager _entityManager;
    private EntityCommandBuffer _ecb;
    private PickUpList _itemIndex;

    public PickUpList ItemIndex { set => _itemIndex = value; }

    protected override void OnCreate()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override void OnUpdate()
    {
        _ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var (inventoryPlayer, input, playerTag, entity) in SystemAPI.Query<PlayerInventoryComponent, RefRW<InputComponent>, RefRO<PlayerTag>>().WithEntityAccess())
        {
            if (_reloadUseItem == 0 && _itemIndex != 0)
            {
                input.ValueRW.UseItem = 0;
                _reloadUseItem = 1f;

                switch (_itemIndex)
                {
                    case PickUpList.nullItem:
                        break;
                    case PickUpList.Health:
                        HealthItemPlayer(entity);
                        break;
                    case PickUpList.Speed:
                        SpeedItemPlayer(entity);
                        break;
                    default:
                        break;
                }

                inventoryPlayer.itemCountPlayerInventory[_itemIndex] -= 1;
                var itemInventoryIndex = inventoryPlayer.ItemInventoryPlayer.FindIndex(x => x.GetComponent<ItemTag>().PickUpItemTag == _itemIndex);//индекс предмета в инвенторе игрока
                inventoryPlayer.ItemInventoryPlayer[itemInventoryIndex].GetComponent<ItemCountInventory>().Cout = inventoryPlayer.itemCountPlayerInventory[_itemIndex].ToString();

                if (inventoryPlayer.itemCountPlayerInventory[_itemIndex] <= 0)
                {
                    inventoryPlayer.ItemInventoryPlayer.Remove(inventoryPlayer.ItemInventoryPlayer[itemInventoryIndex]);//удаление из списка предметов у игрока
                    inventoryPlayer.itemCountPlayerInventory.Remove(_itemIndex);//удаление из перечесления количетсва предмета в инвентаре
                    DestoyObject?.Invoke();
                }

                _itemIndex = 0;
            }

           
            if (_reloadUseItem != 0 && input.ValueRO.UseItem == 0)
            {
                _reloadUseItem -= SystemAPI.Time.DeltaTime;

                if (_reloadUseItem < 0)
                    _reloadUseItem = 0;
            }

           
        }

        _ecb.Playback(_entityManager);
        _ecb.Dispose();
    }

    private void HealthItemPlayer(Entity entity)
    {
        var healthPlayer = _entityManager.GetComponentData<HealthPlayerComponent>(entity);
        healthPlayer.Health += 5;
        _entityManager.SetComponentData<HealthPlayerComponent>(entity, healthPlayer);
    }

    private void SpeedItemPlayer(Entity entity)
    {
        var movePlayer = _entityManager.GetComponentData<MovePlayerComponent>(entity);
        movePlayer.Speed += 0.2f;
    }
}
