using Unity.Entities;
using UnityEngine;

class MovePlayerAuthoring : MonoBehaviour
{
    [HideInInspector] public Transform TransformPlayer { get => gameObject.transform;}
    [HideInInspector] public float Speed;
}

class MovePlayerAuthoringBaker : Baker<MovePlayerAuthoring>
{
    public override void Bake(MovePlayerAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);
        authoring.Speed = 1;
        AddComponentObject(entity, new MovePlayerComponent
        {
            TransformPlayer = authoring.TransformPlayer,
            Speed = authoring.Speed
        });
    }
}

class MovePlayerComponent: IComponentData
{
    public Transform TransformPlayer;
    public float Speed;
}
