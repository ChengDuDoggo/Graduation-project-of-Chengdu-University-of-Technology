using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    private LightControl[] sceneLights;
    private LightShift currentLightShift;
    private Season currentSeaon;
    private float timeDifference;
    private void OnEnable()
    {
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;//��ʾ:ί�к����¼���һ��װ���������ı���,���ǲ�����ÿ��ί��ֻ��װһ����������,ί�п���װ�ܶ಻ͬ�ĺ���Ȼ��������ͬʱִ��!
        EventHandler.LightShiftChangeEvent += OnLightShiftChangeEvent;
    }
    private void OnDisable()
    {
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.LightShiftChangeEvent -= OnLightShiftChangeEvent;
    }

    private void OnLightShiftChangeEvent(Season season, LightShift lightShift, float timeDifference)
    {
        currentSeaon = season;
        this.timeDifference = timeDifference;
        if (currentLightShift != lightShift)
        {
            currentLightShift = lightShift;
            foreach (LightControl light in sceneLights)
            {
                //lightcontrol �ı�ƹ�ķ���
                light.ChangeLightShift(currentSeaon, currentLightShift, timeDifference);
            }
        }
    }

    private void OnAfterSceneLoadedEvent()
    {
        sceneLights = FindObjectsOfType<LightControl>();
        foreach (LightControl light in sceneLights)
        {
            //lightControl �ı�ƹ�ķ���
            light.ChangeLightShift(currentSeaon, currentLightShift, timeDifference);
        }
    }
}
