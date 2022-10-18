using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace MFarm.Inventory
{
    [RequireComponent(typeof(SlotUI))]//本脚本挂载的前提是目标物品必须拥有SlotUI脚本
    public class ShowItemToolTip : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler//调用两个接口
    {
        private SlotUI slotUI;
        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (slotUI.itemAmount != 0)
            {
                inventoryUI.itemToolTip.gameObject.SetActive(true);
                inventoryUI.itemToolTip.SetupTooltip(slotUI.itemDetails, slotUI.slotType);
                inventoryUI.itemToolTip.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);
                inventoryUI.itemToolTip.transform.position = transform.position + Vector3.up * 60;
            }
            else
            {
                inventoryUI.itemToolTip.gameObject.SetActive(false);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            inventoryUI.itemToolTip.gameObject.SetActive(false);
        }

        private void Awake()
        {
            slotUI = GetComponent<SlotUI>();
        }
    }
}

