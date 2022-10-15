using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory
{
    public class InventoryUI : MonoBehaviour//���Ʊ����򿪣����ߴ�����ʾ��Ϣ��UI�Ŀ���
    {
        [Header("��ұ���UI")]
        [SerializeField] private GameObject bagUI;
        private bool bagOpened;//�жϱ����Ƿ񱻴򿪵�״̬
        [SerializeField] private SlotUI[] playerSlots;
        private void OnEnable()//���ű�ִ��ʱΪί���¼���ӷ���(ע�᷽��)
        {
            EventHandler.UpdateInventoryUI += OnUpdateInventoryUI;
        }
        private void OnDisable()//���ű��ر�ʱȥ��ί���еĺ�������
        {
            EventHandler.UpdateInventoryUI -= OnUpdateInventoryUI;
        }

        private void OnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
        {
            switch (location)
            {
                case InventoryLocation.Player:
                    for (int i = 0; i < playerSlots.Length; i++)
                    {
                        if (list[i].itemAmount > 0)//ֻ�б����е���Ʒ��������0���ܵ��ø��¸��ӵĺ���
                        {
                            var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
                            playerSlots[i].UpdateSlot(item, list[i].itemAmount);
                        }
                        else
                        {
                            playerSlots[i].UpdateEmptySlot();
                        }
                    }
                    break;
            }
        }

        private void Start()
        {
            //��ÿһ���������
            for (int i = 0; i < playerSlots.Length; i++)
            {
                playerSlots[i].slotIndex = i;
            }
            bagOpened = bagUI.activeInHierarchy;//�жϸ�GameObject��Hierarchy�Ƿ��Ǽ���״̬
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                OpenBagUI();
            }
        }
        public void OpenBagUI()
        {
            bagOpened = !bagOpened;
            bagUI.SetActive(bagOpened);
        }
    }
}

