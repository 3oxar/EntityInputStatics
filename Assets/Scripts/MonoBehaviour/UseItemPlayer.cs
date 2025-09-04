using System;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class UseItemPlayer : MonoBehaviour
{
    public ItemPlayerAllList ItemIndex;
    public TextMeshProUGUI CoutItemText;

    /// <summary>
    /// ��������� ������� ����� ������� ��� ����������� 
    /// </summary>
    public void UseItem()
    {
        var useItemPlayerSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<UseItemPlayerSystem>();
        useItemPlayerSystem.ItemIndex = ItemIndex;
    }

    private void OnEnable()
    {
        var useItemPlayerSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<UseItemPlayerSystem>();
        useItemPlayerSystem.DestoyObject += DestroyObjectButton;
    }

    private void OnDisable()
    {
        var useItemPlayerSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<UseItemPlayerSystem>();
        useItemPlayerSystem.DestoyObject -= DestroyObjectButton;

    }

    /// <summary>
    /// ������� ������� �� UI ���� �� ����������
    /// </summary>
    private void DestroyObjectButton()
    {
        var coutItem = Convert.ToInt32(CoutItemText.text);
        if(coutItem < 1)
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        var coutItem = Convert.ToInt32(CoutItemText.text);
        if (coutItem < 1)
        {
            Destroy(this.gameObject);
        }
    }
}

