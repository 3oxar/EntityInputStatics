using Unity.Entities;
using UnityEngine;

class ParallelLoadConfigurationAuthoring : MonoBehaviour
{
    public GameObject PrefabObject;
    public string PathConfiguration;
    public int coutEntity;
}

class ParallelLoadConfigurationAuthoringBaker : Baker<ParallelLoadConfigurationAuthoring>
{
    public override void Bake(ParallelLoadConfigurationAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);
        Entity entityPrefab = GetEntity(authoring.PrefabObject, TransformUsageFlags.None);

        AddComponentObject(entity, new ParallelLoadConfigurationComponent
        {
            PrefabObject = entityPrefab,
            PathConfiguration = authoring.PathConfiguration,
            coutEntity = authoring.coutEntity
        });
    }
}

public class ParallelLoadConfigurationComponent: IComponentData
{
    public Entity PrefabObject;
    public string PathConfiguration;
    public int coutEntity;

}

public struct CharacterStatsComponent: IComponentData
{
    public int Health;
    public float MoveSpeed;
    public float Damage;
    public float Power;
}
