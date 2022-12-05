using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MFarm.Inventory
{
    public class InventoryUI : MonoBehaviour//控制背包打开，道具触碰显示信息等UI的控制
    {
        public ItemToolTip itemToolTip;
        [Header("拖拽的图片")]
        public Image dragItem;
        [Header("玩家背包UI")]
        [SerializeField] private GameObject bagUI;
        private bool bagOpened;//判断背包是否被打开的状态
        [Header("通用背包")]
        [SerializeField] private GameObject baseBag;
        public GameObject shopSlotPrefab;//商店格子预制体
        [SerializeField] private SlotUI[] playerSlots;//玩家的每一个背包格子
        [SerializeField] private List<SlotUI> baseBagSlots;
        private void OnEnable()//当脚本执行时为委托事件添加方法(注册方法)
        {
            EventHandler.UpdateInventoryUI += OnUpdateInventoryUI;
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
            EventHandler.BaseBagOpenEvent += OnBaseBagOpenEvent;
        }
        private void OnDisable()//当脚本关闭时去除委托中的函数方法
        {
            EventHandler.UpdateInventoryUI -= OnUpdateInventoryUI;
            EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
            EventHandler.BaseBagOpenEvent -= OnBaseBagOpenEvent;
        }

        private void OnBaseBagOpenEvent(SlotType slotType, InventoryBag_SO bagData)
        {
            //TODO:通用箱子Prefab
            GameObject prefab = slotType switch//语法糖,因为这里是通用事件,不只是打开商店,也可以用来打开箱子,所以Switch语法糖来判断传入不同的预制体
            {
                SlotType.Shop => shopSlotPrefab,
                _ => null,
            };
            //生成背包UI
            baseBag.SetActive(true);//激活背包UI面板
            baseBagSlots = new List<SlotUI>();
            for (int i = 0; i < bagData.itemList.Count; i++)
            {
                var slot = Instantiate(prefab, baseBag.transform.GetChild(1)).GetComponent<SlotUI>();
                slot.slotIndex = i;
                baseBagSlots.Add(slot);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(baseBag.GetComponent<RectTransform>());//强制刷新,否则UI显示不正确
            //更新UI显示
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
                        if (list[i].itemAmount > 0)//只有背包中的物品数量大于0才能调用更新格子的函数
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
                        if (list[i].itemAmount > 0)//只有背包中的物品数量大于0才能调用更新格子的函数
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
            //给每一个格子序号
            for (int i = 0; i < playerSlots.Length; i++)
            {
                playerSlots[i].slotIndex = i;
            }
            bagOpened = bagUI.activeInHierarchy;//判断该GameObject在Hierarchy是否是激活状态
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                OpenBagUI();
            }
        }
        /// <summary>
        /// 打开关闭背包UI，按钮点击事件
        /// </summary>
        public void OpenBagUI()
        {
            bagOpened = !bagOpened;
            bagUI.SetActive(bagOpened);
        }
        /// <summary>
        /// 更新Slots高亮显示
        /// </summary>
        /// <param name="index">每一个格子的序号</param>
        public void UpdateSlotHighlight(int index)//更新格子选中时的高光表现，每次只有一个格子能表现高光
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

