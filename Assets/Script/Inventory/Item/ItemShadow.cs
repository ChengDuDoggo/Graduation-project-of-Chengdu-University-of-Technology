using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MFarm.Inventory
{
    [RequireComponent(typeof(SpriteRenderer))]//想要使用此脚本必须包含SpriteRenderer组件
    public class ItemShadow : MonoBehaviour
    {
        public SpriteRenderer itemSprite;//本体
        private SpriteRenderer shadowSprite;//影子
        private void Awake()
        {
            shadowSprite = GetComponent<SpriteRenderer>();
        }
        private void Start()
        {
            shadowSprite.sprite = itemSprite.sprite;
            shadowSprite.color = new Color(0, 0, 0, 0.3f);
        }
    }
}

