using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFarm.Save;
namespace MFarm.Inventory //手动添加一个命名空间，别的类不使用该命名空间就不可以调用到该命名空间中的变量或者函数，达到保护作用
{
    public class InventoryManager : Singleton<InventoryManager>,ISaveable//数据管理类
    {
        [Header("物品数据")]
        public ItemDataList_SO itemDataList_SO;//拿到数据库
        [Header("建造蓝图")]
        public BluePrintDataList_SO bulePrintData;
        [Header("背包数据")]
        public InventoryBag_SO playerBagTemp;
        public InventoryBag_SO PlayerBag;
        private InventoryBag_SO currentBoxBag;//当前打开的箱子的数据库
        [Header("交易")]
        public int playerMoney;
        private Dictionary<string, List<InventoryItem>> boxDataDict = new Dictionary<string, List<InventoryItem>>();
        public int BoxDataAmount => boxDataDict.Count;

        public string GUID => GetComponent<DataGUID>().guid;
        private void OnEnable()
        {
            EventHandler.DropItemEvent += OnDropItemEvent;
            EventHandler.HarvestAtPlayerPostion += OnHarvestAtPlayerPostion;
            EventHandler.BuildFurnitureEvent += OnBuildFurnitureEvent;
            EventHandler.BaseBagOpenEvent += OnBaseBagOpenEvent;
            EventHandler.StartNewGameEvent += OnStartNewGameEvent;
        }
        private void OnDisable()
        {
            EventHandler.DropItemEvent -= OnDropItemEvent;
            EventHandler.HarvestAtPlayerPostion -= OnHarvestAtPlayerPostion;
            EventHandler.BuildFurnitureEvent -= OnBuildFurnitureEvent;
            EventHandler.BaseBagOpenEvent -= OnBaseBagOpenEvent;
            EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
        }

        private void OnStartNewGameEvent(int index)
        {
            PlayerBag = Instantiate(playerBagTemp);
            playerMoney = Settings.playerStartMoney;
            boxDataDict.Clear();
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, PlayerBag.itemList);
        }

        private void OnBaseBagOpenEvent(SlotType slotType, InventoryBag_SO bag_SO)
        {
            currentBoxBag = bag_SO;
        }

        private void OnBuildFurnitureEvent(int ID, Vector3 mousePos)
        {
            RemoveItem(ID, 1);
            BulePrintDetailes bulePrint = bulePrintData.GetBulePrintDetailes(ID);
            foreach (var item in bulePrint.resourceItem)
            {
                RemoveItem(item.itemID, item.itemAmount);
            }
        }

        private void OnHarvestAtPlayerPostion(int ID)
        {
            var index = GetItemIndexInBag(ID);
            AddItemAtIndex(ID, index, 1);
            //更新UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, PlayerBag.itemList);
        }

        private void OnDropItemEvent(int ID, Vector3 pos,ItemType itemType)
        {
            RemoveItem(ID, 1);
        }

        private void Start()
        {
/*            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, PlayerBag.itemList);//游戏一开始就调用一下更新UI的委托事件*/
            ISaveable saveable = this;
            saveable.RegisterSaveable();
        }
        /// <summary>
        /// 通过ID返回物品信息
        /// </summary>
        /// <param name="ID">物品ID</param>
        /// <returns></returns>
        public ItemDetails GetItemDetails(int ID)//通过物品的ID来找到具体的ItemDetails数据
        {
            return itemDataList_SO.itemDetailsList.Find(i => i.itemID == ID);//找到ID与itemID相匹配的itemDetails返回,使用拉姆达表达式可简写
        }
        /// <summary>
        /// 添加物品至Player背包
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toDestroy">拾取后是否销毁物品</param>
        public void AddItem(Item item,bool toDestroy)
        {
            //背包是否已经有这个物体
            var index = GetItemIndexInBag(item.itemID);
            AddItemAtIndex(item.itemID, index, 1);
            if (toDestroy)
            {
                Destroy(item.gameObject);
            }
            //添加物品数据之后需要更新一下UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, PlayerBag.itemList);//直接调用其他脚本注册好的委托事件
        }
        /// <summary>
        /// 检查背包是否有空位
        /// </summary>
        /// <returns></returns>
        private bool CheckBagCapacity()
        {
            for (int i = 0; i < PlayerBag.itemList.Count; i++)
            {
                if (PlayerBag.itemList[i].itemID == 0)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 通过物品ID找到背包中物品所在的位置(索引)
        /// </summary>
        /// <param name="ID">传入的物品ID</param>
        /// <returns>没有找到就返回-1</returns>
        private int GetItemIndexInBag(int ID)
        {
            for (int i = 0; i < PlayerBag.itemList.Count; i++)
            {
                if(PlayerBag.itemList[i].itemID == ID)
                {
                    return i;
                }
            }
            return -1;
        }
        private void AddItemAtIndex(int ID,int index,int amount)
        {
            if(index == -1 && CheckBagCapacity())//背包中没有这个物体且背包中有空位
            {
                var item = new InventoryItem { itemID = ID, itemAmount = amount };//new一个在背包中的Item
                for (int i = 0; i < PlayerBag.itemList.Count; i++)
                {
                    if(PlayerBag.itemList[i].itemID == 0)
                    {
                        PlayerBag.itemList[i] = item;//将空位的这个地方将捡到的新物品赋值给List
                        break;
                    }
                }
            }
            else//背包有这个物体
            {
                int currentAmount = PlayerBag.itemList[index].itemAmount+amount;//获取背包中这个物体的当前数量
                var item = new InventoryItem { itemID = ID, itemAmount = currentAmount };//获得新物体的ID和Amount
                PlayerBag.itemList[index] = item;
            }
        }
        /// <summary>
        /// Player背包范围内交换物品
        /// </summary>
        /// <param name="fromIndex">起始序号</param>
        /// <param name="targetIndex">目标数据序号</param>
        public void SwapItem(int fromIndex,int targetIndex)
        {
            InventoryItem currentItem = PlayerBag.itemList[fromIndex];//获得当前选择的物体数据
            InventoryItem targetItem = PlayerBag.itemList[targetIndex];//获得要去的格子上的物体数据
            if (targetItem.itemID != 0)//目标格子上有数据,则交换ID
            {
                PlayerBag.itemList[fromIndex] = targetItem;
                PlayerBag.itemList[targetIndex] = currentItem;
            }
            else
            {
                PlayerBag.itemList[targetIndex] = currentItem;
                PlayerBag.itemList[fromIndex] = new InventoryItem();//new 一个空的InventoryItem
            }
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, PlayerBag.itemList);//刷新一下背包UI
        }
        /// <summary>
        /// 跨背包交换数据
        /// </summary>
        /// <param name="locationFrom"></param>
        /// <param name="fromIndex"></param>
        /// <param name="locationTarget"></param>
        /// <param name="targetIndex"></param>
        public void SwapItem(InventoryLocation locationFrom,int fromIndex,InventoryLocation locationTarget,int targetIndex)
        {
            var currentList = GetItemList(locationFrom);
            var targetList = GetItemList(locationTarget);
            InventoryItem currentItem = currentList[fromIndex];
            if (targetIndex < targetList.Count)
            {
                InventoryItem targetItem = targetList[targetIndex];
                if (targetItem.itemID != 0 && currentItem.itemID != targetItem.itemID)//有不相同的两个物品
                {
                    currentList[fromIndex] = targetItem;
                    targetList[targetIndex] = currentItem;
                }
                else if(currentItem.itemID == targetItem.itemID)//相同的两个物品
                {
                    targetItem.itemAmount += currentItem.itemAmount;
                    targetList[targetIndex] = targetItem;
                    currentList[fromIndex] = new InventoryItem();
                }
                else//目标空格子
                {
                    targetList[targetIndex] = currentItem;
                    currentList[fromIndex] = new InventoryItem();
                }
                EventHandler.CallUpdateInventoryUI(locationFrom, currentList);
                EventHandler.CallUpdateInventoryUI(locationTarget, targetList);
            }
        }
        /// <summary>
        /// 根据位置返回背包的数据列表
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private List<InventoryItem> GetItemList(InventoryLocation location)
        {
            return location switch
            {
                InventoryLocation.Player => PlayerBag.itemList,
                InventoryLocation.Box => currentBoxBag.itemList,
                _ => null,
            };
        }
        /// <summary>
        /// 移除指定数量的背包物品
        /// </summary>
        /// <param name="ID">物品ID</param>
        /// <param name="removeAmount">数量</param>
        private void RemoveItem(int ID,int removeAmount)
        {
            var index = GetItemIndexInBag(ID);
            if (PlayerBag.itemList[index].itemAmount > removeAmount)
            {
                var amount = PlayerBag.itemList[index].itemAmount - removeAmount;
                var item = new InventoryItem { itemID = ID, itemAmount = amount };
                PlayerBag.itemList[index] = item;
            }
            else if (PlayerBag.itemList[index].itemAmount == removeAmount)
            {
                var item = new InventoryItem();
                PlayerBag.itemList[index] = item;
            }
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, PlayerBag.itemList);//更新一下UI
        }
        /// <summary>
        /// 检查建造资源物品库存
        /// </summary>
        /// <param name="ID">图纸ID</param>
        /// <returns></returns>
        public bool CheckStock(int ID)
        {
            var bulePrintDetails = bulePrintData.GetBulePrintDetailes(ID);
            foreach (var resourceItem in bulePrintDetails.resourceItem)
            {
                var itemStock = PlayerBag.GetInventoryItem(resourceItem.itemID);
                if (itemStock.itemAmount >= resourceItem.itemAmount)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 交易物品
        /// </summary>
        /// <param name="itemDetails">物品信息</param>
        /// <param name="amount">交易数量</param>
        /// <param name="isSellTrade">是否卖东西</param>
        public void TradeItem(ItemDetails itemDetails,int amount,bool isSellTrade)
        {
            int cost = itemDetails.itemPrice * amount;//总共花费多少金币
            //获得物品的背包位置
            int index = GetItemIndexInBag(itemDetails.itemID);
            if (isSellTrade)//卖
            {
                if (PlayerBag.itemList[index].itemAmount >= amount)
                {
                    RemoveItem(itemDetails.itemID, amount);
                    //卖出总价
                    cost = (int)(cost * itemDetails.sellPercentage);
                    playerMoney += cost;
                }
            }
            else if (playerMoney - cost >= 0)
            {
                if (CheckBagCapacity())
                {
                    AddItemAtIndex(itemDetails.itemID, index, amount);
                }
                playerMoney -= cost;
            }
            //刷新UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, PlayerBag.itemList);
        }
        /// <summary>
        /// 查找箱子数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<InventoryItem> GetBoxDataList(string key)
        {
            if (boxDataDict.ContainsKey(key))
            {
                return boxDataDict[key];
            }
            return null;
        }
        /// <summary>
        /// 加入箱子数据字典
        /// </summary>
        /// <param name="box"></param>
        public void AddBoxDataDict(Box box)
        {
            var key = box.name + box.index;
            if (!boxDataDict.ContainsKey(key))
            {
                boxDataDict.Add(key, box.boxBagData.itemList);
            }
        }

        public GameSaveData GenerateSaveData()
        {
            GameSaveData saveData = new GameSaveData();
            saveData.playerMoney = this.playerMoney;
            saveData.inventoryDict = new Dictionary<string, List<InventoryItem>>();
            saveData.inventoryDict.Add(PlayerBag.name, PlayerBag.itemList);
            foreach (var item in boxDataDict)
            {
                saveData.inventoryDict.Add(item.Key, item.Value);
            }
            return saveData;
        }

        public void RestoreData(GameSaveData saveDate)
        {
            this.playerMoney = saveDate.playerMoney;
            PlayerBag = Instantiate(playerBagTemp);
            PlayerBag.itemList = saveDate.inventoryDict[PlayerBag.name];
            foreach (var item in saveDate.inventoryDict)
            {
                if (boxDataDict.ContainsKey(item.Key))
                {
                    boxDataDict[item.Key] = item.Value;
                }
            }
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, PlayerBag.itemList);//刷新一下UI
        }
    }
}

