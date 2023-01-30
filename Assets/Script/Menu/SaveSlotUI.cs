using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotUI : MonoBehaviour
{
    public Text dataTime, dataScene;
    private Button currentButton;
    private int index => transform.GetSiblingIndex();//�õ���ǰobject��ͬ�㼶Hierarchy�е�λ������
    private void Awake()
    {
        currentButton = GetComponent<Button>();
        currentButton.onClick.AddListener(LoadGameData);//Ϊ��ǰ��ť��Ӽ�������¼�
        //�������˵�ǰ��ť�͵���(LoadGameData)����
    }
    private void LoadGameData()
    {
        Debug.Log(index);
    }
}
