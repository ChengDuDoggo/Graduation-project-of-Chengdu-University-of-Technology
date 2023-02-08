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
    private int index => transform.GetSiblingIndex();//得到当前object在同层级Hierarchy中的位置索引
    private void Awake()
    {
        currentButton = GetComponent<Button>();
        currentButton.onClick.AddListener(LoadGameData);//为当前按钮添加监听点击事件
        //如果点击了当前按钮就调用(LoadGameData)函数
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
            dataTime.text = "这个世界还没有开始";
            dataScene.text = "梦还没开始";
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
            Debug.Log("新游戏");
            EventHandler.CallStartNewGameEvent(index);
        }
    }
}
