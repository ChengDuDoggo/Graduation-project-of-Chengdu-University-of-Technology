using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private int gameSecond, gameMinute, gameHour, gameDay, gameMonth, gameYear;
    private Season gameSeason = Season.����;//Ĭ����Ϸ����Ϊ����
    private int monthInSeason = 3;//������Ϊһ������
    private bool gameClockPause;//����һ������ֵ��������Ϸ����ͣ
    private float tikTime;//��ʱ��
    private void Awake()
    {
        NewGameTime();
    }
    private void Start()
    {
        EventHandler.CallGameDateSeason(gameHour, gameDay, gameMonth, gameYear, gameSeason);
        EventHandler.CallGameMinuteEvent(gameMinute, gameHour);
    }
    private void Update()
    {
        if (!gameClockPause)
        {
            //��дһ�������ʱ��
            tikTime += Time.deltaTime;
            if (tikTime >= Settings.secondThreshold)
            {
                tikTime -= Settings.secondThreshold;
                UpdateGameTime();
            }
        }
        if (Input.GetKey(KeyCode.T))//���װ�ť
        {
            for (int i = 0; i < 60; i++)
            {
                UpdateGameTime();
            }
        }
    }
    private void NewGameTime()//�¿�һ����Ϸʱ����Ϸ��ʼ����ֵ
    {
        gameSecond = 0;
        gameMinute = 0;
        gameHour = 7;//�������ߵ㿪ʼ��Ϸ
        gameDay = 1;
        gameMonth = 1;
        gameYear = 2022;
        gameSeason = Season.����;
    }
    private void UpdateGameTime()//������Ϸʱ��,������������εݽ�
    {
        gameSecond++;
        if (gameSecond > Settings.secondHold)
        {
            gameMinute++;
            gameSecond = 0;

            if (gameMinute > Settings.minuteHold)
            {
                gameHour++;
                gameMinute = 0;

                if (gameHour > Settings.hourHold)
                {
                    gameDay++;
                    gameHour = 0;

                    if (gameDay > Settings.dayHold)
                    {
                        gameDay = 1;
                        gameMonth++;

                        if (gameMonth > 12)
                            gameMonth = 1;

                        monthInSeason--;
                        if (monthInSeason == 0)
                        {
                            monthInSeason = 3;

                            int seasonNumber = (int)gameSeason;
                            seasonNumber++;

                            if (seasonNumber > Settings.seasonHold)
                            {
                                seasonNumber = 0;
                                gameYear++;
                            }

                            gameSeason = (Season)seasonNumber;

                            if (gameYear > 9999)
                            {
                                gameYear = 2022;
                            }
                        }
                    }
                }
                EventHandler.CallGameDateSeason(gameHour, gameDay, gameMonth, gameYear, gameSeason); //������Ҫ����һ��ί���¼�
            }
            EventHandler.CallGameMinuteEvent(gameMinute, gameHour);//������Ҫ����һ��ί���¼�
        }
    }
}
