using System;
using UnityEngine;

[CreateAssetMenu]
public class SettingsFile : ScriptableObject
{
    public string NameFileJson;//имя файля при создании
    public string URLFile;//Url файла
    public SettingsPlayer SettingsPlayer;//настройки игрока
}
