using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    [Header("组件获取")]
    [SerializeField] private Image slotImage;//[SerializeField]可以在可视化面板中为私有变量直接托取赋值
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Image slotHightLight;
    [SerializeField] private Button button;
    [Header("格子类型")]
    public SlotType slotType;
    public bool isSelected;

    //物品信息
    public ItemDetails itemDetails;
    public int itemAmount;
    private void Start()
    {
        isSelected = false;
        if(itemDetails.itemID == 0)//如果当前单独的格子里面的物品ID为0就说明当前物品格子为null
        {
            UpdateEmptySlot();
        }
    }
    /// <summary>
    /// 更新格子的UI信息
    /// </summary>
    /// <param name="item">ItemDetails</param>
    /// <param name="amount">持有物品数量</param>
    public void UpdateSlot(ItemDetails item,int amount)
    {
        itemDetails = item;
        slotImage.sprite = item.itemIcon;
        itemAmount = amount;
        amountText.text = amount.ToString();
        button.interactable = true;
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
}
