using DG.Tweening;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class ItemFader : MonoBehaviour //挂载在物体身上，使人物经过道具然后道具透明化的类
{
    private SpriteRenderer spriteRender;

    private void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }
    /// <summary>
    /// 逐渐恢复颜色
    /// </summary>
    public void Fadein()
    {
        Color targetColor = new Color(1, 1, 1, 1);
        spriteRender.DOColor(targetColor, Settings.ItemfadeDuration);
    }
    /// <summary>
    /// 逐渐半透明
    /// </summary>
    public void FadeOut()
    {
        Color targetColor = new Color(1, 1, 1, Settings.targetAlpha);
        spriteRender.DOColor(targetColor, Settings.ItemfadeDuration);
    }
}
