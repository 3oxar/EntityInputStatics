using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;

class CreatePrefabAuthoring : MonoBehaviour
{
    public GameObject Prefab;
    public Transform Transform { get => gameObject.transform; }
    public Quaternion Rotation { get => gameObject.transform.rotation; }
}

class CreatePrefabAuthoringBaker : Baker<CreatePrefabAuthoring>
{
    public override void Bake(CreatePrefabAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
        var entityPrefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic);

        AddComponentObject(entity, new CreatePrefabComponet
        {
            Entity = entityPrefab,
            Transform = authoring.Transform,
            Rotation = authoring.Rotation
        });
    }
}


class CreatePrefabComponet : IComponentData
{
    public Entity Entity;
    public Transform Transform;
    public Quaternion Rotation;
}