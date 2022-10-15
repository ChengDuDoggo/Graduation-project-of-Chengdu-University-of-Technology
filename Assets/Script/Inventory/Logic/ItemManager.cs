using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ר���������������ͼ�����е���Ʒ�Ĺ�����
namespace MFarm.Inventory
{
    public class ItemManager : Singleton<ItemManager>
    {
        public Item itemPrefab;
        private Transform itemParent;
        private void OnEnable()
        {
            EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;//Ϊί���¼���Ӻ�������
        }
        private void OnDisable()
        {
            EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
        }
        private void Start()
        {
            itemParent = GameObject.FindWithTag("ItemParent").transform;
        }
        private void OnInstantiateItemInScene(int ID, Vector3 pos)
        {
            var item = Instantiate(itemPrefab, pos, Quaternion.identity, itemParent);//��¡����
            item.itemID = ID;
        }
    }
}

