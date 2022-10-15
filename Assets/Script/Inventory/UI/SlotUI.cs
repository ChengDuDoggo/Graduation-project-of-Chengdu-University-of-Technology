using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace MFarm.Inventory
{
    public class SlotUI : MonoBehaviour,IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler//���ü���unity�Դ��ĵ㰴�¼��ӿ�(���,��ʼ��ק,������ק,��ק����)
    {
        [Header("�����ȡ")]
        [SerializeField] private Image slotImage;//[SerializeField]�����ڿ��ӻ������Ϊ˽�б���ֱ����ȡ��ֵ
        [SerializeField] private TextMeshProUGUI amountText;
        public Image slotHightLight;
        [SerializeField] private Button button;
        [Header("��������")]
        public SlotType slotType;
        public bool isSelected;
        public int slotIndex;//ÿһ�������ж�Ӧ�����

        //��Ʒ��Ϣ
        public ItemDetails itemDetails;
        public int itemAmount;

        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();
        private void Start()
        {
            isSelected = false;
            if (itemDetails.itemID == 0)//�����ǰ�����ĸ����������ƷIDΪ0��˵����ǰ��Ʒ����Ϊnull
            {
                UpdateEmptySlot();
            }
        }
        /// <summary>
        /// ���¸��ӵ�UI��Ϣ
        /// </summary>
        /// <param name="item">ItemDetails</param>
        /// <param name="amount">������Ʒ����</param>
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

        public void OnPointerClick(PointerEventData eventData)//�ӿ�����ĺ�������,�����Ǹ�ʲô�Ŀ���ȥUnity�ٷ��ֲ��в鿴
        {
            if (itemAmount == 0)
                return;//���������������û���κ���Ʒ�����޷����
            isSelected = !isSelected;//�л�һ��ѡ�е�״̬
            inventoryUI.UpdateSlotHighlight(slotIndex);
        }

        public void OnBeginDrag(PointerEventData eventData)//����Unity�Դ��Ľӿ��еĺ�������,������;���Բ鿴Unity�ֲ�鿴
        {
            if (itemAmount != 0)
            {
                inventoryUI.dragItem.enabled = true;
                inventoryUI.dragItem.sprite = slotImage.sprite;//�������е�ͼƬ��ֵ��׼����ק��ͼƬ
                inventoryUI.dragItem.SetNativeSize();//����һ��ͼƬ��С
                isSelected = true;//��ק��ͼƬĬ�ϱ�Ϊѡ��״̬
                inventoryUI.UpdateSlotHighlight(slotIndex);//��Ϊ�߹�
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.transform.position = Input.mousePosition;//��ק�����в��Ͻ������ά������ֵ��ͼƬ
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.enabled = false;//��ק����,�ر�ͼƬ
        }
    }
}
