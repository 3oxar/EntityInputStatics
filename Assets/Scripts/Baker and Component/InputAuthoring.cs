using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class InputAuthoring : MonoBehaviour
{
    [HideInInspector] public float2 Move;
    [HideInInspector] public float Jerk;
    [HideInInspector] public float CreatePrefab;
    [HideInInspector] public float Fire;
    [HideInInspector] public float Download;
    [HideInInspector] public float Upload;
    [HideInInspector] public float Write;
}

class InputAuthoringBaker : Baker<InputAuthoring>
{
    public override void Bake(InputAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);

        AddComponent(entity, new InputComponent
        {
            Move = authoring.Move,
            Jerk = authoring.Jerk,
            CreatePrefab = authoring.CreatePrefab, 
            Fire = authoring.Fire,
            Download = authoring.Download,
            Upload = authoring.Upload,
            Write = authoring.Write
        });
    }
}

struct InputComponent: IComponentData
{
    public float2 Move;
    public float Jerk;
    public float CreatePrefab;
    public float Fire;
    public float Download;
    public float Upload;
    public float Write;
}

