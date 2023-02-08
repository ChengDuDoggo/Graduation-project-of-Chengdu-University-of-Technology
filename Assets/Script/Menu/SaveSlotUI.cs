using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MFarm.Save;

public class SaveSlotUI : MonoBehaviour
{
    public Text dataTime, dataScene;
    private Button currentButton;
    private DataSlot currentData;
    private int index => transform.GetSiblingIndex();//�õ���ǰobject��ͬ�㼶Hierarchy�е�λ������
    private void Awake()
    {
        currentButton = GetComponent<Button>();
        currentButton.onClick.AddListener(LoadGameData);//Ϊ��ǰ��ť��Ӽ�������¼�
        //�������˵�ǰ��ť�͵���(LoadGameData)����
    }
    private void OnEnable()
    {
        SetupSlotUI();
    }
    private void SetupSlotUI()
    {
        currentData = SaveLoadManager.Instance.dataSlots[index];
        if (currentData != null)
        {
            dataTime.text = currentData.DataTime;
            dataScene.text = currentData.DataScene;
        }
        else
        {
            dataTime.text = "������绹û�п�ʼ";
            dataScene.text = "�λ�û��ʼ";
        }
    }
    private void LoadGameData()
    {
        if (currentData != null)
        {
            SaveLoadManager.Instance.Load(index);
        }
        else
        {
            Debug.Log("����Ϸ");
            EventHandler.CallStartNewGameEvent(index);
        }
    }
}
