using Unity.Entities;
using UnityEngine;

class MovePlayerAuthoring : MonoBehaviour
{
    [HideInInspector] public Transform TransformPlayer { get => gameObject.transform;}
}

class MovePlayerAuthoringBaker : Baker<MovePlayerAuthoring>
{
    public override void Bake(MovePlayerAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);

        AddComponentObject(entity, new MovePlayerComponent
        {
            TransformPlayer = authoring.TransformPlayer,
        });
    }
}

class MovePlayerComponent: IComponentData
{
    public Transform TransformPlayer;

}
