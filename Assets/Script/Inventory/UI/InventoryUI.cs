using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MFarm.Inventory
{
    public class InventoryUI : MonoBehaviour//���Ʊ����򿪣����ߴ�����ʾ��Ϣ��UI�Ŀ���
    {
        public ItemToolTip itemToolTip;
        [Header("��ק��ͼƬ")]
        public Image dragItem;
        [Header("��ұ���UI")]
        [SerializeField] private GameObject bagUI;
        private bool bagOpened;//�жϱ����Ƿ񱻴򿪵�״̬
        [Header("ͨ�ñ���")]
        [SerializeField] private GameObject baseBag;
        public GameObject shopSlotPrefab;//�̵����Ԥ����
        [SerializeField] private SlotUI[] playerSlots;//��ҵ�ÿһ����������
        [SerializeField] private List<SlotUI> baseBagSlots;
        private void OnEnable()//���ű�ִ��ʱΪί���¼���ӷ���(ע�᷽��)
        {
            EventHandler.UpdateInventoryUI += OnUpdateInventoryUI;
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
            EventHandler.BaseBagOpenEvent += OnBaseBagOpenEvent;
        }
        private void OnDisable()//���ű��ر�ʱȥ��ί���еĺ�������
        {
            EventHandler.UpdateInventoryUI -= OnUpdateInventoryUI;
            EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
            EventHandler.BaseBagOpenEvent -= OnBaseBagOpenEvent;
        }

        private void OnBaseBagOpenEvent(SlotType slotType, InventoryBag_SO bagData)
        {
            //TODO:ͨ������Prefab
            GameObject prefab = slotType switch//�﷨��,��Ϊ������ͨ���¼�,��ֻ�Ǵ��̵�,Ҳ��������������,����Switch�﷨�����жϴ��벻ͬ��Ԥ����
            {
                SlotType.Shop => shopSlotPrefab,
                _ => null,
            };
            //���ɱ���UI
            baseBag.SetActive(true);//�����UI���
            baseBagSlots = new List<SlotUI>();
            for (int i = 0; i < bagData.itemList.Count; i++)
            {
                var slot = Instantiate(prefab, baseBag.transform.GetChild(1)).GetComponent<SlotUI>();
                slot.slotIndex = i;
                baseBagSlots.Add(slot);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(baseBag.GetComponent<RectTransform>());//ǿ��ˢ��,����UI��ʾ����ȷ
            //����UI��ʾ
            OnUpdateInventoryUI(InventoryLocation.Box, bagData.itemList);
        }

        private void OnBeforeSceneUnloadEvent()
        {
            UpdateSlotHighlight(-1);
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
                case InventoryLocation.Box:
                    for (int i = 0; i < baseBagSlots.Count; i++)
                    {
                        if (list[i].itemAmount > 0)//ֻ�б����е���Ʒ��������0���ܵ��ø��¸��ӵĺ���
                        {
                            var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
                            baseBagSlots[i].UpdateSlot(item, list[i].itemAmount);
                        }
                        else
                        {
                            baseBagSlots[i].UpdateEmptySlot();
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
        /// <summary>
        /// �򿪹رձ���UI����ť����¼�
        /// </summary>
        public void OpenBagUI()
        {
            bagOpened = !bagOpened;
            bagUI.SetActive(bagOpened);
        }
        /// <summary>
        /// ����Slots������ʾ
        /// </summary>
        /// <param name="index">ÿһ�����ӵ����</param>
        public void UpdateSlotHighlight(int index)//���¸���ѡ��ʱ�ĸ߹���֣�ÿ��ֻ��һ�������ܱ��ָ߹�
        {
            foreach (var slot in playerSlots)
            {
                if (slot.isSelected && slot.slotIndex == index)
                {
                    slot.slotHightLight.gameObject.SetActive(true);
                }
                else
                {
                    slot.isSelected = false;
                    slot.slotHightLight.gameObject.SetActive(false);
                }
            }
        }
    }
}

