using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//设置键盘快捷键的脚本
namespace MFarm.Inventory
{
    [RequireComponent(typeof(SlotUI))]//确保对象身上有SloUI组件,才能挂载本脚本
    public class ActionBarButton : MonoBehaviour
    {
        public KeyCode key;
        private SlotUI slotUI;
        private bool canUse;
        private void Awake()
        {
            slotUI = GetComponent<SlotUI>();
        }
        private void OnEnable()
        {
            EventHandler.UpdateGameStateEvent += OnUpdateGameStateEvent;
        }
        private void OnDisable()
        {
            EventHandler.UpdateGameStateEvent -= OnUpdateGameStateEvent;
        }

        private void OnUpdateGameStateEvent(GameState gameState)
        {
            canUse = gameState == GameState.Gameplay;
        }

        private void Update()
        {
            if (Input.GetKeyDown(key) && canUse)
            {
                if (slotUI.itemDetails != null)//只有格子中有物品才能用键盘快捷键选择
                {
                    slotUI.isSelected = !slotUI.isSelected;
                    if (slotUI.isSelected)
                    {
                        slotUI.inventoryUI.UpdateSlotHighlight(slotUI.slotIndex);
                    }
                    else
                    {
                        slotUI.inventoryUI.UpdateSlotHighlight(-1);
                    }
                    //程序执行到这里,代表这里发生了点击,因为我们在委托事件句柄中编写了背包格子点击委托事件
                    //所以无论如何这里发生了点击都应该执行点击委托事件,使程序连贯合理
                    EventHandler.CallItemSelectedEvent(slotUI.itemDetails, slotUI.isSelected);
                }
            }
        }
    }
}

