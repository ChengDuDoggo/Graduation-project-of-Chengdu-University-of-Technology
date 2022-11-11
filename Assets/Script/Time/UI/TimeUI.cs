using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour//将代码逻辑与UI连接到一起的脚本
{
    public RectTransform dayNightImage;
    public RectTransform clockParent;
    public Image seasonImage;
    public TextMeshProUGUI dataText;
    public TextMeshProUGUI timeText;
    public Sprite[] seasonSprites;//存放四张季节图片的数组
    private List<GameObject> clockBlocks = new List<GameObject>();
    private void Awake()
    {
        for (int i = 0; i < clockParent.childCount; i++)
        {
            clockBlocks.Add(clockParent.GetChild(i).gameObject);
            clockParent.GetChild(i).gameObject.SetActive(false);
        }
    }
    private void OnEnable()//注册需要的委托事件
    {
        EventHandler.GameMinuteEvent += OnGameMinuteEvent;
        EventHandler.GameDateSeason += OnGameDateSeason;
    }
    private void OnDisable()//注销需要的委托事件
    {
        EventHandler.GameMinuteEvent -= OnGameMinuteEvent;
        EventHandler.GameDateSeason -= OnGameDateSeason;
    }
    private void OnGameMinuteEvent(int minute, int hour,Season season,int day)
    {
        timeText.text = hour.ToString("00") + ":" + minute.ToString("00");
    }
    private void OnGameDateSeason(int hour, int day, int month, int year, Season season)
    {
        dataText.text = year + "年" + month.ToString("00") + "月" + day.ToString("00") + "日";
        seasonImage.sprite = seasonSprites[(int)season];
        SwitchHourImage(hour);
        DayNightImageRotate(hour);
    }
    /// <summary>
    /// 根据小时切换时间块显示
    /// </summary>
    /// <param name="hour"></param>
    private void SwitchHourImage(int hour)
    {
        int index = hour / 4;

        if (index == 0)
        {
            foreach (var item in clockBlocks)
            {
                item.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < clockBlocks.Count; i++)
            {
                if (i < index + 1)
                    clockBlocks[i].SetActive(true);
                else
                    clockBlocks[i].SetActive(false);
            }
        }
    }
    private void DayNightImageRotate(int hour)//控制日夜图片旋转
    {
        var target = new Vector3(0, 0, hour * 15 - 90);//将要旋转到的目标角度位置
        dayNightImage.DORotate(target, 1f, RotateMode.Fast);//使用DOWTeen动画
    }
}
