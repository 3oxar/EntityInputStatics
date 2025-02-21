using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class InputAuthoring : MonoBehaviour
{
   [HideInInspector] public float2 Move;
   [HideInInspector] public float Jerk;
}

class InputAuthoringBaker : Baker<InputAuthoring>
{
    public override void Bake(InputAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);

        AddComponent(entity, new InputComponent
        {
            Move = authoring.Move,
            Jerk = authoring.Jerk
        });
    }
}

struct InputComponent: IComponentData
{
    public float2 Move;
    public float Jerk;
}

