using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MFarm.Inventory
{
    public class Box : MonoBehaviour
    {
        public InventoryBag_SO boxBagTemplate;
        public InventoryBag_SO boxBagData;
        public GameObject mouseIcon;
        private bool canOpen = false;
        private bool isOpen;
        public int index;//������ÿ����һ���µ�box��index���ۼ�
        private void OnEnable()
        {
            if (boxBagData == null)
            {
                boxBagData = Instantiate(boxBagTemplate);
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                canOpen = true;
                mouseIcon.SetActive(true);
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                canOpen = false;
                mouseIcon.SetActive(false);
            }
        }
        private void Update()
        {
            if (!isOpen && canOpen && Input.GetMouseButtonDown(1))
            {
                //������
                EventHandler.CallBaseBagOpenEvent(SlotType.Box, boxBagData);
                isOpen = true;
            }
            if (!canOpen && isOpen)
            {
                //�ر�����
                EventHandler.CallBaseBagCloseEvent(SlotType.Box, boxBagData);
                isOpen = false;
            }
            if (isOpen && Input.GetKeyDown(KeyCode.Escape))
            {
                //�ر�����
                EventHandler.CallBaseBagCloseEvent(SlotType.Box, boxBagData);
                isOpen = false;
            }
        }
        public void InitBox(int boxIndex)
        {
            index = boxIndex;
            var key = this.name + index;
            if (InventoryManager.Instance.GetBoxDataList(key) != null)//ˢ�µ�ͼ��ȡ����
            {
                boxBagData.itemList = InventoryManager.Instance.GetBoxDataList(key);
            }
            else//�½�����
            {
                InventoryManager.Instance.AddBoxDataDict(this);
            }
        }
    }
}

