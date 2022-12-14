using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory //�ֶ����һ�������ռ䣬����಻ʹ�ø������ռ�Ͳ����Ե��õ��������ռ��еı������ߺ������ﵽ��������
{
    public class InventoryManager : Singleton<InventoryManager>//���ݹ�����
    {
        [Header("��Ʒ����")]
        public ItemDataList_SO itemDataList_SO;//�õ����ݿ�
        [Header("������ͼ")]
        public BluePrintDataList_SO bulePrintData;
        [Header("��������")]
        public InventoryBag_SO PlayerBag;
        [Header("����")]
        public int playerMoney;
        private void OnEnable()
        {
            EventHandler.DropItemEvent += OnDropItemEvent;
            EventHandler.HarvestAtPlayerPostion += OnHarvestAtPlayerPostion;
            EventHandler.BuildFurnitureEvent += OnBuildFurnitureEvent;
        }
        private void OnDisable()
        {
            EventHandler.DropItemEvent -= OnDropItemEvent;
            EventHandler.HarvestAtPlayerPostion -= OnHarvestAtPlayerPostion;
            EventHandler.BuildFurnitureEvent -= OnBuildFurnitureEvent;
        }

        private void OnBuildFurnitureEvent(int ID, Vector3 mousePos)
        {
            RemoveItem(ID, 1);
            BulePrintDetailes bulePrint = bulePrintData.GetBulePrintDetailes(ID);
            foreach (var item in bulePrint.resourceItem)
            {
                RemoveItem(item.itemID, item.itemAmount);
            }
        }

        private void OnHarvestAtPlayerPostion(int ID)
        {
            var index = GetItemIndexInBag(ID);
            AddItemAtIndex(ID, index, 1);
            //����UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, PlayerBag.itemList);
        }

        private void OnDropItemEvent(int ID, Vector3 pos,ItemType itemType)
        {
            RemoveItem(ID, 1);
        }

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
        /// <summary>
        /// Player������Χ�ڽ�����Ʒ
        /// </summary>
        /// <param name="fromIndex">��ʼ���</param>
        /// <param name="targetIndex">Ŀ���������</param>
        public void SwapItem(int fromIndex,int targetIndex)
        {
            InventoryItem currentItem = PlayerBag.itemList[fromIndex];//��õ�ǰѡ�����������
            InventoryItem targetItem = PlayerBag.itemList[targetIndex];//���Ҫȥ�ĸ����ϵ���������
            if (targetItem.itemID != 0)//Ŀ�������������,�򽻻�ID
            {
                PlayerBag.itemList[fromIndex] = targetItem;
                PlayerBag.itemList[targetIndex] = currentItem;
            }
            else
            {
                PlayerBag.itemList[targetIndex] = currentItem;
                PlayerBag.itemList[fromIndex] = new InventoryItem();//new һ���յ�InventoryItem
            }
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, PlayerBag.itemList);//ˢ��һ�±���UI
        }
        /// <summary>
        /// �Ƴ�ָ�������ı�����Ʒ
        /// </summary>
        /// <param name="ID">��ƷID</param>
        /// <param name="removeAmount">����</param>
        private void RemoveItem(int ID,int removeAmount)
        {
            var index = GetItemIndexInBag(ID);
            if (PlayerBag.itemList[index].itemAmount > removeAmount)
            {
                var amount = PlayerBag.itemList[index].itemAmount - removeAmount;
                var item = new InventoryItem { itemID = ID, itemAmount = amount };
                PlayerBag.itemList[index] = item;
            }
            else if (PlayerBag.itemList[index].itemAmount == removeAmount)
            {
                var item = new InventoryItem();
                PlayerBag.itemList[index] = item;
            }
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, PlayerBag.itemList);//����һ��UI
        }
        /// <summary>
        /// ��齨����Դ��Ʒ���
        /// </summary>
        /// <param name="ID">ͼֽID</param>
        /// <returns></returns>
        public bool CheckStock(int ID)
        {
            var bulePrintDetails = bulePrintData.GetBulePrintDetailes(ID);
            foreach (var resourceItem in bulePrintDetails.resourceItem)
            {
                var itemStock = PlayerBag.GetInventoryItem(resourceItem.itemID);
                if (itemStock.itemAmount >= resourceItem.itemAmount)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// ������Ʒ
        /// </summary>
        /// <param name="itemDetails">��Ʒ��Ϣ</param>
        /// <param name="amount">��������</param>
        /// <param name="isSellTrade">�Ƿ�������</param>
        public void TradeItem(ItemDetails itemDetails,int amount,bool isSellTrade)
        {
            int cost = itemDetails.itemPrice * amount;//�ܹ����Ѷ��ٽ��
            //�����Ʒ�ı���λ��
            int index = GetItemIndexInBag(itemDetails.itemID);
            if (isSellTrade)//��
            {
                if (PlayerBag.itemList[index].itemAmount >= amount)
                {
                    RemoveItem(itemDetails.itemID, amount);
                    //�����ܼ�
                    cost = (int)(cost * itemDetails.sellPercentage);
                    playerMoney += cost;
                }
            }
            else if (playerMoney - cost >= 0)
            {
                if (CheckBagCapacity())
                {
                    AddItemAtIndex(itemDetails.itemID, index, amount);
                }
                playerMoney -= cost;
            }
            //ˢ��UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, PlayerBag.itemList);
        }
    }
}

