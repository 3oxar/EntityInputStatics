using System;
using Unity.Entities;
using UnityEngine;

class SettingFileAuthoring : MonoBehaviour
{
    public SettingsFile SettingsFile;
}

class SettingFileAuthoringBaker : Baker<SettingFileAuthoring>
{
    public override void Bake(SettingFileAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.None);

        AddComponentObject(entity, new SettingsFileComponent
        {
            SettingsFile = authoring.SettingsFile
        });
    }
}


class SettingsFileComponent : IComponentData
{
    public SettingsFile SettingsFile;
}
