using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MFarm.Inventory;
//将ItemDetails的一些属性传入到提示面板中去
public class ItemToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Text valueText;
    [SerializeField] private GameObject buttonPart;
    [Header("建造")]
    public GameObject resoursePanel;
    [SerializeField] private Image[] resourseItem;
    public void SetupTooltip(ItemDetails itemDetails,SlotType slotType)
    {
        nameText.text = itemDetails.itemName;
        typeText.text = GetItemType(itemDetails.itemType);
        descriptionText.text = itemDetails.itemDescription;
        //只有这三个种类的物品可以销售才会显示价格
        if (itemDetails.itemType == ItemType.Seed || itemDetails.itemType == ItemType.Commodity || itemDetails.itemType == ItemType.Furniture)
        {
            buttonPart.SetActive(true);
            var price = itemDetails.itemPrice;
            if (slotType == SlotType.Bag)//如果物品在背包格子中，价格减半
            {
                price = (int)(price * itemDetails.sellPercentage);
            }
            valueText.text = price.ToString();
        }
        else
        {
            buttonPart.SetActive(false);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());//立即重新渲染绘制当前层级，即减少弹出信息面板的延迟
    }
    private string GetItemType(ItemType itemType)//手动将物品类型信息转化为中文
    {
        return itemType switch
        {
            ItemType.Seed => "种子",
            ItemType.Commodity => "商品",
            ItemType.Furniture => "家具",
            ItemType.BreakTool => "工具",
            ItemType.ChopTool => "工具",
            ItemType.CollectTool => "工具",
            ItemType.HolTool => "工具",
            ItemType.ReapTool => "工具",
            ItemType.WaterTool => "工具",
            _ => "无"
        };
    }
    public void SetupResouresPanel(int ID)
    {
        var bulePrintDetails = InventoryManager.Instance.bulePrintData.GetBulePrintDetailes(ID);
        for (int i = 0; i < resourseItem.Length; i++)
        {
            if (i < bulePrintDetails.resourceItem.Length)
            {
                var item = bulePrintDetails.resourceItem[i];
                resourseItem[i].gameObject.SetActive(true);
                resourseItem[i].sprite = InventoryManager.Instance.GetItemDetails(item.itemID).itemIcon;
                resourseItem[i].transform.GetChild(0).GetComponent<Text>().text = item.itemAmount.ToString();
            }
            else
            {
                resourseItem[i].gameObject.SetActive(false);
            }
        }
    }
}
