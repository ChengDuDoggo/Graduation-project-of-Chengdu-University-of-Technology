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
    /// <summary>
    /// ��ʼ��·���ֵ�
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
    /// �õ������������·��
    /// </summary>
    /// <param name="fromSceneName">��ʼ����</param>
    /// <param name="gotoSceneName">Ŀ�곡��</param>
    /// <returns></returns>
    public SceneRoute GetSceneRoute(string fromSceneName,string gotoSceneName)
    {
        return sceneRouteDict[fromSceneName + gotoSceneName];
    }
}
