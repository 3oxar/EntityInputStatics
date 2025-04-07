using System.Collections;
using UnityEngine;

public sealed class UsersCoroutinec : MonoBehaviour
{
    private static UsersCoroutinec instance
    {
        get
        {
            if(usersCoroutinec == null)
            {
                var go = new GameObject("[COROUTINE MANAGER]");
                usersCoroutinec = go.AddComponent<UsersCoroutinec>();
            }

            return usersCoroutinec;
        }
    }

    private static UsersCoroutinec usersCoroutinec;

    public static Coroutine StartCoroutines(IEnumerator enumerator)
    {
        return instance.StartCoroutine(enumerator);
    }

    public static void StopCoroutines(Coroutine coroutine)
    {
        instance.StopCoroutine(coroutine);
    }
}
