using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

partial class NetworkManagerSystem : SystemBase
{
    public GameObject ObjectPlayerScene;

    private Entity _hybridAnimationPlayer;
    private EntityManager _entityManager;
    private bool _isCreate;
    //[BurstCompile]
    //public void OnCreate(ref SystemState state)
    //{

    //}

    //[BurstCompile]
    //public void OnUpdate(ref SystemState state)
    //{

    //}

    //[BurstCompile]
    //public void OnDestroy(ref SystemState state)
    //{

    //}

    protected override void OnStartRunning()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        foreach (var playerPrefab in SystemAPI.Query<NetworkManagerComponent>())
        {
            Object.Instantiate(playerPrefab.NetworkManager);
        }
    }
   

    protected override void OnUpdate()
    {
        if (ObjectPlayerScene != null && _isCreate == false)
        {
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);
            foreach (var (playerPrefab, entity) in SystemAPI.Query<NetworkManagerComponent>().WithEntityAccess())
            {
                _hybridAnimationPlayer = _entityManager.Instantiate(playerPrefab.PrefabPlayer);
               
                //ecb.SetComponent(newEntity, component);
            }

            var component = _entityManager.GetComponentObject<PlayerGameObjectPrefab>(_hybridAnimationPlayer);
            component.PrefabPlayer = ObjectPlayerScene;
            component.IsCreate = true;
            _entityManager.SetComponentData(_hybridAnimationPlayer, component);

            _isCreate = true;
        }
    }
}
