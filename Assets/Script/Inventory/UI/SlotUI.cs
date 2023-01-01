using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace MFarm.Inventory
{
    public class SlotUI : MonoBehaviour,IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler//调用几个unity自带的点按事件接口(点击,开始拖拽,正在拖拽,拖拽结束)
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
        public InventoryLocation Location//属性
        {
            get//可读
            {
                return slotType switch
                {
                    SlotType.Bag => InventoryLocation.Player,
                    SlotType.Box => InventoryLocation.Box,
                    _ => InventoryLocation.Player
                };
            }
        }

        public InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();
        private void Start()
        {
            isSelected = false;
            if (itemDetails == null)//如果当前单独的格子里面的物品ID为0就说明当前物品格子为null
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
                inventoryUI.UpdateSlotHighlight(-1);
                EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
            }
            itemDetails = null;
            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
        }

        public void OnPointerClick(PointerEventData eventData)//接口里面的函数方法,具体是干什么的可以去Unity官方手册中查看
        {
            if (itemDetails == null)
                return;//如过点击的这个格子没有任何物品，则无法点击
            isSelected = !isSelected;//切换一下选中的状态
            inventoryUI.UpdateSlotHighlight(slotIndex);
            if (slotType == SlotType.Bag)//如果格子类型是背包格子，才能触发切换动画效果
            {
                EventHandler.CallItemSelectedEvent(itemDetails, isSelected);//触发点击委托事件
            }
        }

        public void OnBeginDrag(PointerEventData eventData)//都是Unity自带的接口中的函数方法,具体用途可以查看Unity手册查看
        {
            if (itemAmount != 0)
            {
                inventoryUI.dragItem.enabled = true;
                inventoryUI.dragItem.sprite = slotImage.sprite;//将格子中的图片赋值给准备拖拽的图片
                inventoryUI.dragItem.SetNativeSize();//适配一下图片大小
                isSelected = true;//拖拽的图片默认变为选择状态
                inventoryUI.UpdateSlotHighlight(slotIndex);//变为高光
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.transform.position = Input.mousePosition;//拖拽过程中不断将鼠标三维向量赋值给图片
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.enabled = false;//拖拽结束,关闭图片
            if (eventData.pointerCurrentRaycast.gameObject != null)//拖拽物体停止的当前射线之下不为null
            {
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null)//如果图片触碰到的物品没有SlotUI脚本即无法互动，直接返回
                {
                    return;
                }
                var targatSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();//将目标格子获取
                int targetIndex = targatSlot.slotIndex;//获得目标格子的序号
                //在Player自身背包中交换
                if(slotType == SlotType.Bag&&targatSlot.slotType == SlotType.Bag)
                {
                    InventoryManager.Instance.SwapItem(slotIndex, targetIndex);
                }
                else if (slotType == SlotType.Shop && targatSlot.slotType == SlotType.Bag)//买
                {
                    EventHandler.CallShowTradeUI(itemDetails, false);
                }
                else if (slotType == SlotType.Bag && targatSlot.slotType == SlotType.Shop)//卖
                {
                    EventHandler.CallShowTradeUI(itemDetails, true);
                }
                else if(slotType != SlotType.Shop && targatSlot.slotType != SlotType.Shop&&slotType != targatSlot.slotType)
                {
                    //跨背包数据交换物品
                    InventoryManager.Instance.SwapItem(Location, slotIndex, targatSlot.Location, targatSlot.slotIndex);
                }
                //拖拽完成后关闭所有的高亮显示
                inventoryUI.UpdateSlotHighlight(-1);
            }
            /*else//测试扔在地上
            {
                if (itemDetails.canCarried)
                {
                    //鼠标对应的世界地图上的坐标
                    var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));//将鼠标的屏幕坐标转化为世界坐标
                    EventHandler.CallInstantiateItemInScene(itemDetails.itemID, pos);
                }

            }*/
        }
    }
}
