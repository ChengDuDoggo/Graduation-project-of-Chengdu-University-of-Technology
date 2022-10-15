using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace MFarm.Inventory
{
    public class SlotUI : MonoBehaviour,IPointerClickHandler//调用一个unity自带的点按事件接口
    {
        [Header("组件获取")]
        [SerializeField] private Image slotImage;//[SerializeField]可以在可视化面板中为私有变量直接托取赋值
        [SerializeField] private TextMeshProUGUI amountText;
        public Image slotHightLight;
        [SerializeField] private Button button;
        [Header("格子类型")]
        public SlotType slotType;
        public bool isSelected;
        public int slotIndex;//每一个格子有对应的序号

        //物品信息
        public ItemDetails itemDetails;
        public int itemAmount;

        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();
        private void Start()
        {
            isSelected = false;
            if (itemDetails.itemID == 0)//如果当前单独的格子里面的物品ID为0就说明当前物品格子为null
            {
                UpdateEmptySlot();
            }
        }
        /// <summary>
        /// 更新格子的UI信息
        /// </summary>
        /// <param name="item">ItemDetails</param>
        /// <param name="amount">持有物品数量</param>
        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            slotImage.sprite = item.itemIcon;
            itemAmount = amount;
            amountText.text = amount.ToString();
            button.interactable = true;
            slotImage.enabled = true;
        }

        /// <summary>
        /// 更新格子为null
        /// </summary>
        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;
            }
            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
        }

        public void OnPointerClick(PointerEventData eventData)//接口里面的函数方法,具体是干什么的可以去Unity官方手册中查看
        {
            if (itemAmount == 0)
                return;//如过点击的这个格子没有任何物品，则无法点击
            isSelected = !isSelected;//切换一下选中的状态
            inventoryUI.UpdateSlotHighlight(slotIndex);
        }
    }
}
