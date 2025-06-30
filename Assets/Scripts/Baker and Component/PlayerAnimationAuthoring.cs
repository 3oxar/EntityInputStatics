using Unity.Entities;
using UnityEngine;

class PlayerAnimationAuthoring : MonoBehaviour
{
    public GameObject PrefabPlayer;
}

class PlayerAnimationAuthoringBaker : Baker<PlayerAnimationAuthoring>
{
    public override void Bake(PlayerAnimationAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponentObject(entity, new PlayerGameObjectPrefab
        {
            PrefabPlayer = authoring.PrefabPlayer
        });
    }
}

public class PlayerGameObjectPrefab : IComponentData
{
    public GameObject PrefabPlayer;
}

public class PlayerAnimationReference : ICleanupComponentData
{
    public Animator AnimationPlayer;
}