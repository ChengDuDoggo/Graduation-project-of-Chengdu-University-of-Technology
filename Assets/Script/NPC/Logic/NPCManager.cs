using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : Singleton<NPCManager>
{
    public SceneRouteDataList_SO sceneRouteData;
    public List<NPCPosition> npcPositionList;
    private Dictionary<string, SceneRoute> sceneRouteDict = new Dictionary<string, SceneRoute>();
    protected override void Awake()
    {
        base.Awake();
        InitSceneRouteDict();
    }
    private void OnEnable()
    {
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
    }
    private void OnDisable()
    {
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
    }

    private void OnStartNewGameEvent(int index)
    {
        foreach (var character in npcPositionList)
        {
            character.npc.position = character.position;
            character.npc.GetComponent<NPCMovement>().StartScene = character.startScene;
        }
    }

    /// <summary>
    /// 初始化路劲字典
    /// </summary>
    private void InitSceneRouteDict()
    {
        if (sceneRouteData.sceneRouteList.Count > 0)
        {
            foreach (SceneRoute route in sceneRouteData.sceneRouteList)
            {
                var key = route.fromSceneName + route.gotoSceneName;
                if (sceneRouteDict.ContainsKey(key))
                    continue;
                else
                    sceneRouteDict.Add(key, route);
            }
        }
    }
    /// <summary>
    /// 得到两个场景间的路径
    /// </summary>
    /// <param name="fromSceneName">起始场景</param>
    /// <param name="gotoSceneName">目标场景</param>
    /// <returns></returns>
    public SceneRoute GetSceneRoute(string fromSceneName,string gotoSceneName)
    {
        return sceneRouteDict[fromSceneName + gotoSceneName];
    }
}
