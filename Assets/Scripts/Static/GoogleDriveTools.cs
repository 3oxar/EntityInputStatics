using System.Text;
using UnityEngine.Networking;
using UnityEngine;
using UnityGoogleDrive;
using UnityGoogleDrive.Data;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public static class GoogleDriveTools
{
    private static UnityWebRequest request;
    private static bool isCoroutineStart = false;

    //public static void Upload(string url, string obj)
    //{
    //    var file = new UnityGoogleDrive.Data.File { Name = nameJson, Content = Encoding.ASCII.GetBytes(obj) };
    //    GoogleDriveFiles.Create(file).Send();
    //}

    /// <summary>
    /// ������� ������ � �����
    /// </summary>
    /// <param name="URL"></param>
    public static void Download(string URL)
    {
        UsersCoroutinec.StartCoroutines(WebGetCoroutine(URL));
    }

    /// <summary>
    /// ��������� ��������� ������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Loading<T>()
    {
        Debug.Log("Loading.");
        if (request.isDone)
        {
            Debug.Log("Set setting");
            T setting = JsonUtility.FromJson<T>(request.downloadHandler.text);
            isCoroutineStart = false;
            return setting;
        }
        else
        {
            Debug.Log("Error");
            isCoroutineStart = false;
            return default(T);
        }
       ;
    }

    /// <summary>
    /// ������ ������ � ����
    /// </summary>
    /// <typeparam name="T">��� ������</typeparam>
    /// <param name="path">���� � �����</param>
    /// <param name="obj">������ ������� ����������</param>
    public static void WriteSettingsFile<T>(string path, T obj)
    {
        Debug.Log("Write new settings");
        var jsonText = JsonUtility.ToJson(obj);
        System.IO.File.WriteAllText(path, jsonText);
    }

    private static IEnumerator WebGetCoroutine(string url)
    {
        request = new UnityWebRequest();
        request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        Debug.Log("Is done - " + request.isDone);
      
    }
}
