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
        private void Start()
        {
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, PlayerBag.itemList);//��Ϸһ��ʼ�͵���һ�¸���UI��ί���¼�
        }
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
            //�����Ƿ��Ѿ����������
            var index = GetItemIndexInBag(item.itemID);
            AddItemAtIndex(item.itemID, index, 1);
            if (toDestroy)
            {
                Destroy(item.gameObject);
            }
            //�����Ʒ����֮����Ҫ����һ��UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, PlayerBag.itemList);//ֱ�ӵ��������ű�ע��õ�ί���¼�
        }
        /// <summary>
        /// ��鱳���Ƿ��п�λ
        /// </summary>
        /// <returns></returns>
        private bool CheckBagCapacity()
        {
            for (int i = 0; i < PlayerBag.itemList.Count; i++)
            {
                if (PlayerBag.itemList[i].itemID == 0)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// ͨ����ƷID�ҵ���������Ʒ���ڵ�λ��(����)
        /// </summary>
        /// <param name="ID">�������ƷID</param>
        /// <returns>û���ҵ��ͷ���-1</returns>
        private int GetItemIndexInBag(int ID)
        {
            for (int i = 0; i < PlayerBag.itemList.Count; i++)
            {
                if(PlayerBag.itemList[i].itemID == ID)
                {
                    return i;
                }
            }
            return -1;
        }
        private void AddItemAtIndex(int ID,int index,int amount)
        {
            if(index == -1 && CheckBagCapacity())//������û����������ұ������п�λ
            {
                var item = new InventoryItem { itemID = ID, itemAmount = amount };//newһ���ڱ����е�Item
                for (int i = 0; i < PlayerBag.itemList.Count; i++)
                {
                    if(PlayerBag.itemList[i].itemID == 0)
                    {
                        PlayerBag.itemList[i] = item;//����λ������ط����񵽵�����Ʒ��ֵ��List
                        break;
                    }
                }
            }
            else//�������������
            {
                int currentAmount = PlayerBag.itemList[index].itemAmount+amount;//��ȡ�������������ĵ�ǰ����
                var item = new InventoryItem { itemID = ID, itemAmount = currentAmount };//����������ID��Amount
                PlayerBag.itemList[index] = item;
            }
        }
    }
}

