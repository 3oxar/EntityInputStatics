using UnityEngine;
using UnityEditor;
using System;


[CustomEditor(typeof(ConfigurationInstaller))]
public class SettingsConfugirationEditor : EditorWindow
{

    private string[] _settingPlayer;
    private string[] _curentSettingsPlayer;
    private string[] _configurationInstaller;
    private DifficultyLevel[] _difficultyLevel;

    private int _index;

    [MenuItem("Configuration/Player")]
    public static void Test()
    {
        EditorWindow.GetWindow(typeof(SettingsConfugirationEditor));
    }

    private void OnGUI()
    {
        _settingPlayer = AssetDatabase.FindAssets("Player t:SettingsPlayer");
        _curentSettingsPlayer = AssetDatabase.FindAssets("Player t:CurrentConfigPlayer");
        _configurationInstaller = AssetDatabase.FindAssets("Installer t:ConfigurationInstaller");
        _difficultyLevel = (DifficultyLevel[])Enum.GetValues(typeof(DifficultyLevel));
        _index = 0;

        GUILayout.Label("Game setting player", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        foreach(var file in _settingPlayer)
        {
            //��������� �� ������ ��� �� �������� ������ �������� �����
            var splitPathFirst = AssetDatabase.GUIDToAssetPath(file).Split("/");
            var splitPathSecond = splitPathFirst[splitPathFirst.Length - 1].Split(".");
            
            if (GUILayout.Button(splitPathSecond[0]))//������� ������ � ��������� ������ ����� 
            {
                //var settingPlayer = AssetDatabase.LoadAssetAtPath<SettingsPlayer>(AssetDatabase.GUIDToAssetPath(file));
                //var configurationCurrent = AssetDatabase.LoadAssetAtPath<CurrentConfigPlayer>(AssetDatabase.GUIDToAssetPath(_curentSettingsPlayer[0]));
                //configurationCurrent.CurretnSettingsPlayer = settingPlayer;//������������� ������� ��������� ������
              
                //�.� ��� �������� ���������� ��������� ��������� ���� ��� ���� ����� �� ��������� � ������ � ����� ������������ ��������� ������ ���������
                //��� ������� � ������� ������������ (CurrentConfugiration) ����� ����������� ������ ��������� 

                var configurationInstaller = AssetDatabase.LoadAssetAtPath<ConfigurationInstaller>(AssetDatabase.GUIDToAssetPath(_configurationInstaller[0]));
                configurationInstaller.DifficultyLevel = (DifficultyLevel)_difficultyLevel.GetValue(_index);//������������� ������� ��������� ��� ������� 

                AssetDatabase.SaveAssets();
            }
            _index++;
        }

    }
}
