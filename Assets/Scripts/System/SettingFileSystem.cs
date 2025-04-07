using System.Collections;
using System.IO;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Networking;

partial struct SettingFileSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var settingFile in SystemAPI.Query<SettingsFileComponent>())
        {
            foreach (var input in SystemAPI.Query<RefRO<InputComponent>>())
            {
                if (input.ValueRO.Upload != 0)
                {
                    //    Debug.Log("Upload.");

                    //    var playerHealthText = JsonUtility.FromJson<HealthPlayerComponent>(File.ReadAllText(
                    //        Application.streamingAssetsPath + $"/{settingFile.SettingsFile.NameFileJson}.json"));
                    //    var json = JsonUtility.ToJson(playerHealthText);
                    //    GoogleDriveTools.Upload(settingFile.SettingsFile.NameFileJson, json);
                }

                if (input.ValueRO.Download != 0f)
                {
                    Debug.Log("Get.");
                    UsersCoroutinec.StartCoroutines(WaitEndJobsCoroutine(settingFile.SettingsFile.URLFile, settingFile.SettingsFile));
                }

                if (input.ValueRO.Write != 0)
                {
                    var newSettingPlayer = new HealthPlayerComponent();
                    newSettingPlayer.Health = settingFile.SettingsFile.SettingsPlayer.Health;
                    GoogleDriveTools.WriteSettingsFile<HealthPlayerComponent>((Application.streamingAssetsPath + $"/{settingFile.SettingsFile.NameFileJson}.json"), newSettingPlayer);
                }
            }
        }
    }

    /// <summary>
    /// Ждем какое-то время для ответа с сайта и устанавливаем полученные настройки
    /// </summary>
    /// <param name="url">ссылка</param>
    /// <param name="settingsFile">куда записываем полученые настройки</param>
    /// <returns></returns>
    private IEnumerator WaitEndJobsCoroutine(string url, SettingsFile settingsFile)
    {
        GoogleDriveTools.Download(url);
        yield return new WaitForSeconds(5);
        settingsFile.SettingsPlayer.Health = GoogleDriveTools.Loading<HealthPlayerComponent>().Health;//устанавлеваем полученые данные
       
    }

}


