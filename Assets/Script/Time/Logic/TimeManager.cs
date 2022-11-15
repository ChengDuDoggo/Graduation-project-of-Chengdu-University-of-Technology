using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManager : Singleton<TimeManager>
{
    private int gameSecond, gameMinute, gameHour, gameDay, gameMonth, gameYear;
    private Season gameSeason = Season.春天;//默认游戏进入为春天
    private int monthInSeason = 3;//三个月为一个季节
    private bool gameClockPause;//定义一个布尔值来控制游戏的暂停
    private float tikTime;//计时器
    public TimeSpan GameTime => new TimeSpan(gameHour, gameMinute, gameSecond);
    protected override void Awake()
    {
        base.Awake();
        NewGameTime();
    }
    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
    }
    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
    }

    private void OnAfterSceneLoadedEvent()
    {
        gameClockPause = false;
    }

    private void OnBeforeSceneUnloadEvent()
    {
        gameClockPause = true;
    }

    private void Start()
    {
        EventHandler.CallGameDateSeason(gameHour, gameDay, gameMonth, gameYear, gameSeason);
        EventHandler.CallGameMinuteEvent(gameMinute, gameHour,gameSeason,gameDay);
    }
    private void Update()
    {
        if (!gameClockPause)
        {
            //编写一个秒针计时器
            tikTime += Time.deltaTime;
            if (tikTime >= Settings.secondThreshold)
            {
                tikTime -= Settings.secondThreshold;
                UpdateGameTime();
            }
        }
        if (Input.GetKey(KeyCode.T))//作弊按钮(时间加快)
        {
            for (int i = 0; i < 60; i++)
            {
                UpdateGameTime();
            }
        }
        if (Input.GetKeyDown(KeyCode.G))//作弊按钮(天数增加)
        {
            gameDay++;
            EventHandler.CallGameDayEvent(gameDay, gameSeason);
            EventHandler.CallGameDateSeason(gameHour, gameDay, gameMonth, gameYear, gameSeason);
        }
    }
    private void NewGameTime()//新开一局游戏时给游戏初始化赋值
    {
        gameSecond = 0;
        gameMinute = 0;
        gameHour = 7;//从早上七点开始游戏
        gameDay = 1;
        gameMonth = 1;
        gameYear = 2022;
        gameSeason = Season.春天;
    }
    private void UpdateGameTime()//更新游戏时间,秒分年月日依次递进
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
                        //用来刷新地图和农作物生长
                        EventHandler.CallGameDayEvent(gameDay, gameSeason);
                    }
                }
                //每时间执行到此位置，调用一下委托时间
                EventHandler.CallGameDateSeason(gameHour, gameDay, gameMonth, gameYear, gameSeason); //这里需要调用一下委托事件
            }
            EventHandler.CallGameMinuteEvent(gameMinute, gameHour,gameSeason,gameDay);//这里需要调用一下委托事件
        }
    }
}
