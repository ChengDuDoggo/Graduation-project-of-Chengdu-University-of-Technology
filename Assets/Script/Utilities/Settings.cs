using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings //�Լ���д��һ����̬�����࣬�൱��д��һ�����ð�ť�����ƹ���������Ҫ����ʹ�õĳ������ߺ���
                      //ֻ������ͨ���������ʽ���ֳ������ð�ť������Ϸ�е�һ��Ч����Ҫ�ı�ʱ��ֱ�ӵ���������������һ��
{
    public const float ItemfadeDuration = 0.35f;
    public const float targetAlpha = 0.45f;
    //ʱ�����
    public const float secondThreshold = 0.1f;//��ֵԽСʱ��Խ��
    //ʱ�����,ʱ��ﵽ���ٹ���ͬʱ��ǰ��һ
    public const int secondHold = 59;
    public const int minuteHold = 59;
    public const int hourHold = 23;
    public const int dayHold = 30;
    public const int seasonHold = 3;
    //Transition
    public const float fadeDuration = 1.5f;//�����л����������¼�
    //�����������
    public const int reapAmount = 2;
    //NPC�����ƶ�
    public const float gridCellSize = 1;
    public const float gridCellDiagonalSize = 1.41f;//б�����ƶ�1.14�պõ���������
    public const float pixelSize = 0.05f;//����Ϸһ�����ص�Ĵ�С
}
