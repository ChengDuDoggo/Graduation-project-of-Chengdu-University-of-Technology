using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//物品摇晃效果脚本
public class ItemInterActive : MonoBehaviour
{
    private bool isAnimating;//判断是否正在播放动画
    private WaitForSeconds pause = new WaitForSeconds(0.04f);//动画间隔时间
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAnimating)
        {
            if (other.transform.position.x < transform.position.x)
            {
                //对方在左侧 向右摇晃
                StartCoroutine(RotateRight());
            }
            else
            {
                //对方在右侧 向左摇晃
                StartCoroutine(RotateLeft());
            }
            EventHandler.CallPlaySoundEvent(SoundName.Rustle);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!isAnimating)
        {
            if (other.transform.position.x > transform.position.x)
            {
                //对方在左侧 向右摇晃
                StartCoroutine(RotateRight());
            }
            else
            {
                //对方在右侧 向左摇晃
                StartCoroutine(RotateLeft());
            }
            EventHandler.CallPlaySoundEvent(SoundName.Rustle);
        }
    }
    private IEnumerator RotateLeft()//使用携程来完成动画效果,避免主程冲突
    {
        isAnimating = true;
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(0).Rotate(0, 0, 2);
            yield return pause;
        }
        for (int i = 0; i < 5; i++)
        {
            transform.GetChild(0).Rotate(0, 0, -2);
            yield return pause;
        }
        transform.GetChild(0).Rotate(0, 0, 2);
        yield return pause;
        isAnimating = false;
    }
    private IEnumerator RotateRight()//使用携程来完成动画效果,避免主程冲突
    {
        isAnimating = true;
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(0).Rotate(0, 0, -2);
            yield return pause;
        }
        for (int i = 0; i < 5; i++)
        {
            transform.GetChild(0).Rotate(0, 0, 2);
            yield return pause;
        }
        transform.GetChild(0).Rotate(0, 0, -2);
        yield return pause;
        isAnimating = false;
    }
}
