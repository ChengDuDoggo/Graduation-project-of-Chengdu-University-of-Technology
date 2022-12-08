using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeUI : MonoBehaviour
{
    public Image itemIcon;
    public Text itemName;
    public InputField tradeAmount;
    public Button submitButton;
    public Button cancelButton;
    private ItemDetails item;
    private bool isSellTrade;
    private void Awake()
    {
        cancelButton.onClick.AddListener(CancelTrade);
    }
    /// <summary>
    /// …Ë÷√TradeUIœ‘ æœÍ«È
    /// </summary>
    /// <param name="item"></param>
    /// <param name="isSell"></param>
    public void SetupTradeUI(ItemDetails item,bool isSell)
    {
        this.item = item;
        itemIcon.sprite = item.itemIcon;
        itemName.text = item.itemName;
        isSellTrade = isSell;
        tradeAmount.text = string.Empty;
    }
    private void CancelTrade()
    {
        this.gameObject.SetActive(false);
    }
}
