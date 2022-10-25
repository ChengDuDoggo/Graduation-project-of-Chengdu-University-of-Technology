using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MFarm.Inventory
{
    [RequireComponent(typeof(SpriteRenderer))]//��Ҫʹ�ô˽ű��������SpriteRenderer���
    public class ItemShadow : MonoBehaviour
    {
        public SpriteRenderer itemSprite;//����
        private SpriteRenderer shadowSprite;//Ӱ��
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

