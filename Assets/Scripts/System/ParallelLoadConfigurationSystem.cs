using System.IO;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

partial struct ParallelLoadConfigurationSystem : ISystem, ISystemStartStop
{
    private CreateEntityJobs _creatyEntityJobs;
    private LoadConfigurationJobs _loadConfigurationJobs;

    private CharacterStatsComponent _characterStatsComponentJSON;

    private EntityCommandBuffer _ecb;
    private EntityCommandBuffer.ParallelWriter _ecbParallel;

    private NativeArray<Entity> _entities;
    private EntityArchetype _entityArchetype;
    private Entity _entityPrefab;

    private JobHandle _jobHandle;

    private int _coutPrefab;//кол-во создаваемых объектов

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate(state.GetEntityQuery(ComponentType.ReadOnly<ParallelLoadConfigurationComponent>()));
    }

    [BurstCompile]
    public void OnStartRunning(ref SystemState state)
    {
        _ecb = new EntityCommandBuffer(Allocator.Persistent);
        _ecbParallel = _ecb.AsParallelWriter();

        foreach(var parallelLoadConfiguration in SystemAPI.Query<ParallelLoadConfigurationComponent>())
        {
            _characterStatsComponentJSON = JsonUtility.FromJson<CharacterStatsComponent>(File.ReadAllText(parallelLoadConfiguration.PathConfiguration));//получаем значение компонента из JSON
            _entityArchetype = state.EntityManager.GetChunk(parallelLoadConfiguration.PrefabObject).Archetype;
            _coutPrefab = parallelLoadConfiguration.coutEntity;
            _entityPrefab = parallelLoadConfiguration.PrefabObject;
        }

        _entities = new NativeArray<Entity>(_coutPrefab, Allocator.Persistent);

        _creatyEntityJobs = new CreateEntityJobs()
        {
            ECBParallel = _ecbParallel,
            Entities = _entities,
            EntityArchetype = _entityArchetype
        };

        _jobHandle = _creatyEntityJobs.Schedule(_coutPrefab, 5);
        _jobHandle.Complete();

        state.EntityManager.CreateEntity(_entityArchetype, _entities);
        state.EntityManager.Instantiate(_entityPrefab, _entities);//создаем префабы на сцене

        _loadConfigurationJobs = new LoadConfigurationJobs()
        {
            ECBParallel = _ecbParallel,
            Entities = _entities,
            CharacterStatsComponent = _characterStatsComponentJSON
        };

        _jobHandle = _loadConfigurationJobs.Schedule(_coutPrefab, 5);
        _jobHandle.Complete();

        _ecb.Playback(state.EntityManager);

        _ecb.Dispose();
        _entities.Dispose();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }

    public void OnStopRunning(ref SystemState state)
    {
       
    }
}

public struct CreateEntityJobs : IJobParallelFor
{
    public EntityCommandBuffer.ParallelWriter ECBParallel;
    public EntityArchetype EntityArchetype;
    public NativeArray<Entity> Entities;

    public void Execute(int index)
    {
        Entities[index] = ECBParallel.CreateEntity(index, EntityArchetype);
    }
}

public struct LoadConfigurationJobs : IJobParallelFor
{
    public EntityCommandBuffer.ParallelWriter ECBParallel;
    public NativeArray<Entity> Entities;
    public CharacterStatsComponent CharacterStatsComponent;

    public void Execute(int index)
    {
        var newComponentCharacterStats = new CharacterStatsComponent()
        {
            Health = CharacterStatsComponent.Health,
            MoveSpeed = CharacterStatsComponent.MoveSpeed + index * 0.2f,
            Damage = CharacterStatsComponent.Damage * 0.5f + index,
            Power = CharacterStatsComponent.Power * 0.33f + index
        };

        ECBParallel.AddComponent(index, Entities[index], newComponentCharacterStats);
    }
}

