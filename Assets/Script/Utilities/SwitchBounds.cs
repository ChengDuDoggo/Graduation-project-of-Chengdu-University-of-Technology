using Cinemachine;
using UnityEngine;

public class SwitchBounds : MonoBehaviour //�����л��߽�ʱ���࣬ÿ�ŵ�ͼ�߽粻ͬ��ÿ���л���ͼʱ���ô��࣬����һ�±߽���Cinamachine֪��
{
    private void Start()
    {
        SwitchConfinerShape();
    }
    private void SwitchConfinerShape()
    {
        PolygonCollider2D confinerShape = GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();
        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();
        confiner.m_BoundingShape2D = confinerShape;
        //Call this if the bounding shape's points change at runtime
        //ÿ���л�������ʱ����ô˺�������һ��·�����棬���֮ǰ�ı߽���Ϣ
        confiner.InvalidatePathCache();
    }
}
