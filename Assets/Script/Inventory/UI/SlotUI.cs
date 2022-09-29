using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    [Header("�����ȡ")]
    [SerializeField] private Image slotImage;//[SerializeField]�����ڿ��ӻ������Ϊ˽�б���ֱ����ȡ��ֵ
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Image slotHightLight;
    [SerializeField] private Button button;
    [Header("��������")]
    public SlotType slotType;
    public bool isSelected;

    //��Ʒ��Ϣ
    public ItemDetails itemDetails;
    public int itemAmount;
    private void Start()
    {
        isSelected = false;
        if(itemDetails.itemID == 0)//�����ǰ�����ĸ����������ƷIDΪ0��˵����ǰ��Ʒ����Ϊnull
        {
            UpdateEmptySlot();
        }
    }
    /// <summary>
    /// ���¸��ӵ�UI��Ϣ
    /// </summary>
    /// <param name="item">ItemDetails</param>
    /// <param name="amount">������Ʒ����</param>
    public void UpdateSlot(ItemDetails item,int amount)
    {
        itemDetails = item;
        slotImage.sprite = item.itemIcon;
        itemAmount = amount;
        amountText.text = amount.ToString();
        button.interactable = true;
    }

    /// <summary>
    /// ���¸���Ϊnull
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
