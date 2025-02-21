using Unity.Entities;
using UnityEngine;

class PlayerAuthoring : MonoBehaviour
{
    
}

class PlayerAuthoringBaker : Baker<PlayerAuthoring>
{
    public override void Bake(PlayerAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);

        AddComponent(entity, new PlayerTag());
    }
}
