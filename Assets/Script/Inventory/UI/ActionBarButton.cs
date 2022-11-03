using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//���ü��̿�ݼ��Ľű�
namespace MFarm.Inventory
{
    [RequireComponent(typeof(SlotUI))]//ȷ������������SloUI���,���ܹ��ر��ű�
    public class ActionBarButton : MonoBehaviour
    {
        public KeyCode key;
        private SlotUI slotUI;
        private void Awake()
        {
            slotUI = GetComponent<SlotUI>();
        }
        private void Update()
        {
            if (Input.GetKeyDown(key))
            {
                if (slotUI.itemDetails != null)//ֻ�и���������Ʒ�����ü��̿�ݼ�ѡ��
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
                    //����ִ�е�����,�������﷢���˵��,��Ϊ������ί���¼�����б�д�˱������ӵ��ί���¼�
                    //��������������﷢���˵����Ӧ��ִ�е��ί���¼�,ʹ�����������
                    EventHandler.CallItemSelectedEvent(slotUI.itemDetails, slotUI.isSelected);
                }
            }
        }
    }
}

