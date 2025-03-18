using Unity.Entities;
using UnityEngine;

class BulletAuthoring : MonoBehaviour
{
    public float TimdeDestroy;
}

class BulletAuthoringBaker : Baker<BulletAuthoring>
{
    public override void Bake(BulletAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

        AddComponent<BulletTag>(entity);
        AddComponent(entity, new BulletDestroyComponent
        {
            TimeDestroy = authoring.TimdeDestroy
        });
    }
}

struct BulletTag : IComponentData
{

}

struct BulletDestroyComponent : IComponentData
{
    public float TimeDestroy;//время через которое будет пуля уничтожена
}
