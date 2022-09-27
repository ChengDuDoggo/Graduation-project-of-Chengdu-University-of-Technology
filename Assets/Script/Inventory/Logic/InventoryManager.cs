using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory //�ֶ����һ�������ռ䣬����಻ʹ�ø������ռ�Ͳ����Ե��õ��������ռ��еı������ߺ������ﵽ��������
{
    public class InventoryManager : Singleton<InventoryManager>//���ݹ�����
    {
        public ItemDataList_SO itemDataList_SO;//�õ����ݿ�
        /// <summary>
        /// ͨ��ID������Ʒ��Ϣ
        /// </summary>
        /// <param name="ID">��ƷID</param>
        /// <returns></returns>
        public ItemDetails GetItemDetails(int ID)//ͨ����Ʒ��ID���ҵ������ItemDetails����
        {
            return itemDataList_SO.itemDetailsList.Find(i => i.itemID == ID);//�ҵ�ID��itemID��ƥ���itemDetails����,ʹ����ķ����ʽ�ɼ�д
        }
    }
}

