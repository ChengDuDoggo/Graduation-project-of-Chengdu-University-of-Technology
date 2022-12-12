using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MFarm.Inventory;
//��ItemDetails��һЩ���Դ��뵽��ʾ�����ȥ
public class ItemToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Text valueText;
    [SerializeField] private GameObject buttonPart;
    [Header("����")]
    public GameObject resoursePanel;
    [SerializeField] private Image[] resourseItem;
    public void SetupTooltip(ItemDetails itemDetails,SlotType slotType)
    {
        nameText.text = itemDetails.itemName;
        typeText.text = GetItemType(itemDetails.itemType);
        descriptionText.text = itemDetails.itemDescription;
        //ֻ���������������Ʒ�������۲Ż���ʾ�۸�
        if (itemDetails.itemType == ItemType.Seed || itemDetails.itemType == ItemType.Commodity || itemDetails.itemType == ItemType.Furniture)
        {
            buttonPart.SetActive(true);
            var price = itemDetails.itemPrice;
            if (slotType == SlotType.Bag)//�����Ʒ�ڱ��������У��۸����
            {
                price = (int)(price * itemDetails.sellPercentage);
            }
            valueText.text = price.ToString();
        }
        else
        {
            buttonPart.SetActive(false);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());//����������Ⱦ���Ƶ�ǰ�㼶�������ٵ�����Ϣ�����ӳ�
    }
    private string GetItemType(ItemType itemType)//�ֶ�����Ʒ������Ϣת��Ϊ����
    {
        return itemType switch
        {
            ItemType.Seed => "����",
            ItemType.Commodity => "��Ʒ",
            ItemType.Furniture => "�Ҿ�",
            ItemType.BreakTool => "����",
            ItemType.ChopTool => "����",
            ItemType.CollectTool => "����",
            ItemType.HolTool => "����",
            ItemType.ReapTool => "����",
            ItemType.WaterTool => "����",
            _ => "��"
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
