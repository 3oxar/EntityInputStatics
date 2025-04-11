using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

class AIBotAuthoring : MonoBehaviour
{
}

class AIBotAuthoringBaker : Baker<AIBotAuthoring>
{
    public override void Bake(AIBotAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);

        AddComponent<EnemyTag>(entity);
        AddComponent(entity, new AIBotComponent
        {
           
        });
    }
}


struct AIBotComponent : IComponentData
{
  
}