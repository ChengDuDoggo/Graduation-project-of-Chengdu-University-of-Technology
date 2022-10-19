using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour//�������߼���UI���ӵ�һ��Ľű�
{
    public RectTransform dayNightImage;
    public RectTransform clockParent;
    public Image seasonImage;
    public TextMeshProUGUI dataText;
    public TextMeshProUGUI timeText;
    public Sprite[] seasonSprites;//������ż���ͼƬ������
    private List<GameObject> clockBlocks = new List<GameObject>();
    private void Awake()
    {
        for (int i = 0; i < clockParent.childCount; i++)
        {
            clockBlocks.Add(clockParent.GetChild(i).gameObject);
            clockParent.GetChild(i).gameObject.SetActive(false);
        }
    }
    private void OnEnable()//ע����Ҫ��ί���¼�
    {
        EventHandler.GameMinuteEvent += OnGameMinuteEvent;
        EventHandler.GameDateSeason += OnGameDateSeason;
    }
    private void OnDisable()//ע����Ҫ��ί���¼�
    {
        EventHandler.GameMinuteEvent -= OnGameMinuteEvent;
        EventHandler.GameDateSeason -= OnGameDateSeason;
    }
    private void OnGameMinuteEvent(int minute, int hour)
    {
        timeText.text = hour.ToString("00") + ":" + minute.ToString("00");
    }
    private void OnGameDateSeason(int hour, int day, int month, int year, Season season)
    {
        dataText.text = year + "��" + month.ToString("00") + "��" + day.ToString("00") + "��";
        seasonImage.sprite = seasonSprites[(int)season];
        SwitchHourImage(hour);
        DayNightImageRotate(hour);
    }
    /// <summary>
    /// ����Сʱ�л�ʱ�����ʾ
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
    private void DayNightImageRotate(int hour)//������ҹͼƬ��ת
    {
        var target = new Vector3(0, 0, hour * 15 - 90);//��Ҫ��ת����Ŀ��Ƕ�λ��
        dayNightImage.DORotate(target, 1f, RotateMode.Fast);//ʹ��DOWTeen����
    }
}
