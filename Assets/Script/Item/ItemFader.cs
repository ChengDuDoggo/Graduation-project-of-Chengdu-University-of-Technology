using DG.Tweening;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class ItemFader : MonoBehaviour
{
    private SpriteRenderer spriteRender;

    private void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }
    /// <summary>
    /// �𽥻ָ���ɫ
    /// </summary>
    public void Fadein()
    {
        Color targetColor = new Color(1, 1, 1, 1);
        spriteRender.DOColor(targetColor, Settings.fadeDuration);
    }
    /// <summary>
    /// �𽥰�͸��
    /// </summary>
    public void FadeOut()
    {
        Color targetColor = new Color(1, 1, 1, Settings.targetAlpha);
        spriteRender.DOColor(targetColor, Settings.fadeDuration);
    }
}
