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
    /// Öð½¥»Ö¸´ÑÕÉ«
    /// </summary>
    public void Fadein()
    {
        Color targetColor = new Color(1, 1, 1, 1);
        spriteRender.DOColor(targetColor, Settings.fadeDuration);
    }
    /// <summary>
    /// Öð½¥°ëÍ¸Ã÷
    /// </summary>
    public void FadeOut()
    {
        Color targetColor = new Color(1, 1, 1, Settings.targetAlpha);
        spriteRender.DOColor(targetColor, Settings.fadeDuration);
    }
}
