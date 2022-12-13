using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MFarm.Inventory
{
    public class Box : MonoBehaviour
    {
        public InventoryBag_SO boxBagTemplate;
        public InventoryBag_SO boxBagData;
        public GameObject mouseIcon;
        private bool canOpen = false;
        private bool isOpen;
        private void OnEnable()
        {
            if (boxBagData == null)
            {
                boxBagData = Instantiate(boxBagTemplate);
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                canOpen = true;
                mouseIcon.SetActive(true);
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                canOpen = false;
                mouseIcon.SetActive(false);
            }
        }
        private void Update()
        {
            if (!isOpen && canOpen && Input.GetMouseButtonDown(1))
            {
                //打开箱子
                EventHandler.CallBaseBagOpenEvent(SlotType.Box, boxBagData);
                isOpen = true;
            }
            if (!canOpen && isOpen)
            {
                //关闭箱子
                EventHandler.CallBaseBagCloseEvent(SlotType.Box, boxBagData);
                isOpen = false;
            }
            if (isOpen && Input.GetKeyDown(KeyCode.Escape))
            {
                //关闭箱子
                EventHandler.CallBaseBagCloseEvent(SlotType.Box, boxBagData);
                isOpen = false;
            }
        }
    }
}

