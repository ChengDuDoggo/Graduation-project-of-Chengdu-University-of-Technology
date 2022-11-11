using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings //自己编写的一个静态设置类，相当于写了一个设置按钮来控制工程里面需要经常使用的常量或者函数
                      //只不过是通过代码的形式呈现出的设置按钮，当游戏中的一个效果需要改变时，直接到设置里面来设置一样
{
    public const float ItemfadeDuration = 0.35f;
    public const float targetAlpha = 0.45f;
    //时间相关
    public const float secondThreshold = 0.1f;//数值越小时间越快
    //时间进制,时间达到多少归零同时向前进一
    public const int secondHold = 59;
    public const int minuteHold = 59;
    public const int hourHold = 23;
    public const int dayHold = 30;
    public const int seasonHold = 3;
    //Transition
    public const float fadeDuration = 1.5f;//场景切换动画持续事件
    //割草数量限制
    public const int reapAmount = 2;
    //NPC网格移动
    public const float gridCellSize = 1;
    public const float gridCellDiagonalSize = 1.41f;//斜方向移动1.14刚好到网格中心
    public const float pixelSize = 0.05f;//本游戏一个像素点的大小
}
