using Unity.Entities;
using UnityEngine;

class NetworkManagerAuthoring : MonoBehaviour
{
    public GameObject PrefabPlayer;
    public NetworkManager NetworkManager;
}

class NetworkManagerAuthoringBaker : Baker<NetworkManagerAuthoring>
{
    public override void Bake(NetworkManagerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        var entityPrefab = GetEntity(authoring.PrefabPlayer, TransformUsageFlags.None);
        AddComponentObject(entity, new NetworkManagerComponent
        {
            PrefabPlayer = entityPrefab,
            NetworkManager = authoring.NetworkManager
        });
    }
}

public class NetworkManagerComponent : IComponentData
{
    public Entity PrefabPlayer;
    public NetworkManager NetworkManager;
}

