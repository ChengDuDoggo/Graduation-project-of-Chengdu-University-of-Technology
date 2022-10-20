using Cinemachine;
using UnityEngine;

public class SwitchBounds : MonoBehaviour //控制切换边界时的类，每张地图边界不同，每次切换地图时调用此类，更新一下边界让Cinamachine知道
{
    private void OnEnable()
    {
        EventHandler.AfterSceneLoadedEvent += SwitchConfinerShape;//加载注册事件
    }
    private void OnDisable()
    {
        EventHandler.AfterSceneLoadedEvent -= SwitchConfinerShape;
    }
    private void SwitchConfinerShape()
    {
        PolygonCollider2D confinerShape = GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();
        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();
        confiner.m_BoundingShape2D = confinerShape;
        //Call this if the bounding shape's points change at runtime
        //每当切换场景的时候调用此函数清理一下路劲缓存，清除之前的边界信息
        confiner.InvalidatePathCache();
    }
}
