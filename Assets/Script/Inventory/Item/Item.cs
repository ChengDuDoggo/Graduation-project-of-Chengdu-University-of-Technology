using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFarm.CropPlant;
namespace MFarm.Inventory
{
    public class Item : MonoBehaviour //�������������Ʒ��������
    {
        public int itemID;
        private SpriteRenderer spriteRenderer;//ͼƬ���
        public ItemDetails itemDetails;//��Ʒ��Ϣ
        private BoxCollider2D coll;
        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            coll = GetComponent<BoxCollider2D>();
        }
        private void Start()
        {
            Init(itemID);
        }
        public void Init(int ID)
        {
            itemID = ID;
            //Inventory��õ�ǰ����
            itemDetails = InventoryManager.Instance.GetItemDetails(itemID);
            if (itemDetails != null)
            {
                spriteRenderer.sprite = itemDetails.itemOnWorldIcon ? itemDetails.itemOnWorldIcon : itemDetails.itemIcon;
            }
            //�޸���ײ��ߴ�
            Vector2 newSize = new Vector2(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y);
            coll.size = newSize;
            coll.offset = new Vector2(0, spriteRenderer.sprite.bounds.center.y);
            if(itemDetails.itemType == ItemType.ReapableScenery)
            {
                gameObject.AddComponent<ReapItem>();
                gameObject.GetComponent<ReapItem>().InitCropData(itemDetails.itemID);
                gameObject.AddComponent<ItemInterActive>();
            }
        }
    }
}

