using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory //�ֶ����һ�������ռ䣬����಻ʹ�ø������ռ�Ͳ����Ե��õ��������ռ��еı������ߺ������ﵽ��������
{
    public class InventoryManager : Singleton<InventoryManager>//���ݹ�����
    {
        [Header("��Ʒ����")]
        public ItemDataList_SO itemDataList_SO;//�õ����ݿ�
        [Header("��������")]
        public InventoryBag_SO PlayerBag;
        /// <summary>
        /// ͨ��ID������Ʒ��Ϣ
        /// </summary>
        /// <param name="ID">��ƷID</param>
        /// <returns></returns>
        public ItemDetails GetItemDetails(int ID)//ͨ����Ʒ��ID���ҵ������ItemDetails����
        {
            return itemDataList_SO.itemDetailsList.Find(i => i.itemID == ID);//�ҵ�ID��itemID��ƥ���itemDetails����,ʹ����ķ����ʽ�ɼ�д
        }
        /// <summary>
        /// �����Ʒ��Player����
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toDestroy">ʰȡ���Ƿ�������Ʒ</param>
        public void AddItem(Item item,bool toDestroy)
        {
            //�����Ƿ��п�λ

            //�����Ƿ��Ѿ����������

            InventoryItem newItem = new InventoryItem();
            newItem.itemID = item.itemID;
            newItem.itemAmount = 1;
            PlayerBag.itemList[0] = newItem;

            if (toDestroy)
            {
                Destroy(item.gameObject);
            }
        }
    }
}

