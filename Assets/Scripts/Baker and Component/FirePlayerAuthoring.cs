using Unity.Entities;
using UnityEngine;

class FirePlayerAuthoring : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform TransformCreateBullet { get => gameObject.transform; }
}

class FirePlayerAuthoringBaker : Baker<FirePlayerAuthoring>
{
    public override void Bake(FirePlayerAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);

        Entity bullet = GetEntity(authoring.BulletPrefab, TransformUsageFlags.Dynamic);

        AddComponentObject(entity, new FirePlayerComponent
        {
            Bullet = bullet,
            TransformCreateBullet = authoring.TransformCreateBullet
        });
    }
}

class FirePlayerComponent : IComponentData
{
    public Entity Bullet;
    public Transform TransformCreateBullet;
}
