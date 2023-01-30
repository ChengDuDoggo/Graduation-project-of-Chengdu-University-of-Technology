using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotUI : MonoBehaviour
{
    public Text dataTime, dataScene;
    private Button currentButton;
    private int index => transform.GetSiblingIndex();//得到当前object在同层级Hierarchy中的位置索引
    private void Awake()
    {
        currentButton = GetComponent<Button>();
        currentButton.onClick.AddListener(LoadGameData);//为当前按钮添加监听点击事件
        //如果点击了当前按钮就调用(LoadGameData)函数
    }
    private void LoadGameData()
    {
        Debug.Log(index);
    }
}
