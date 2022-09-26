using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System;
using System.Linq;

public class ItemEditor : EditorWindow
{
    private ItemDataList_SO dataBase; //创建的数据库中的数据
    private List<ItemDetails> itemList = new List<ItemDetails>();//new一下ItemDtails,相当于数据库中的详细数据列表
    private VisualTreeAsset itemRowTemplate; //获取自己在UIToolkit中定义的样式
    private ListView itemListView;//获得VisualElement中的滚动列表
    private ScrollView itemDetailsSection;//获取UITookit右侧滚轮
    private ItemDetails activeItem;//获取当前选中的道具(类)
    private VisualElement iconPreview;//icon预览
    private Sprite defaultIcon;//默认预览图片

    [MenuItem("Doggo/ItemEditor")]
    public static void ShowExample()
    {
        ItemEditor wnd = GetWindow<ItemEditor>();
        wnd.titleContent = new GUIContent("ItemEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
/*        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);*/

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Bulider/ItemEditor.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        //直接通过绝对路径方式拿到样式模板
        itemRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Bulider/ItemRowTemplate.uxml");

        //拿到默认图片
        defaultIcon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/M Studio/Art/Items/Icons/icon_M.png");

        //变量赋值
        itemListView = root.Q<VisualElement>("ItemList").Q<ListView>("ListView");
        itemDetailsSection = root.Q<ScrollView>("ItemDetails");//滚轮页面赋值
        iconPreview = itemDetailsSection.Q<VisualElement>("Icon");//找到当前滚轮详情页面的Icon
        root.Q<Button>("AddButton").clicked += OnAddItemClicked;
        root.Q<Button>("DeleteButton").clicked += OnDeleteClicked;

        //加载数据
        LoadDataBase();

        //生成ListView
        GenerateListView();
    }

    #region 按键事件
    private void OnDeleteClicked()
    {
        itemList.Remove(activeItem);
        itemListView.Rebuild();//刷新一下滚轮列表，刷新数据
        itemDetailsSection.visible = false;//删除列表后数据丢失，所以关闭详情面板
    }

    private void OnAddItemClicked()
    {
        ItemDetails newItem = new ItemDetails();
        newItem.itemID = 1001 + itemList.Count;
        newItem.itemName = "NEW ITEM";
        itemList.Add(newItem);
        itemListView.Rebuild();//刷新一下数据
    }
    #endregion
    private void LoadDataBase()//拿到Assets中我们创建的数据库文件
    {
        var dataArray = AssetDatabase.FindAssets("ItemDataList_SO");//在整个Assets中找到类型为ItemDataList_SO的资源文件并列为数组,并获得该类文件的GUID
        if (dataArray.Length > 1)//如果Assets中存在ItemDataList_SO类资源文件
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);//通过获得的数组中的具体一个文件的GUID来查找文件的路径
            //因为我只有一个ItemDataList_SO文件,所以我可以直接写dataArray[0]
            dataBase = AssetDatabase.LoadAssetAtPath(path, typeof(ItemDataList_SO)) as ItemDataList_SO;//再通过文件路径加载数据库资源
        }
        itemList = dataBase.itemDetailsList;//为列表赋值
        EditorUtility.SetDirty(dataBase);//标注一下数据,必须标注一下数据否则无法保存数据
    }
    private void GenerateListView()//生成ListView函数方法
    {
        Func<VisualElement> makeItem = () => itemRowTemplate.CloneTree(); //每调用一次生成ListView方法就克隆一份样式模板
        Action<VisualElement, int> bindItem = (e, i) =>
         {
             if (i < itemList.Count)//必须确保序号小于列表中的总体数量否则会报空
             {
                 if (itemList[i].itemIcon != null)
                 {
                     e.Q<VisualElement>("Icon").style.backgroundImage = itemList[i].itemIcon.texture;//将数据库中的数据赋值绑定给UI Toolkit
                 }
                 e.Q<Label>("Name").text = itemList[i] == null ? "NO ITEM" : itemList[i].itemName;
             }
         };
        //e.Q<VisualElement>:就是代表UI Toolkit中具体的元素,VisualElement就是里面的空白容器
        //e.Q<Label>代表的是文本标签，在此用代码将他们实例化

        itemListView.fixedItemHeight = 60;//固定滚轮高度
        itemListView.itemsSource = itemList;//将列表中的数据放入滚轮中
        itemListView.makeItem = makeItem;//先将模板克隆在滚轮中，于是滚轮就拥有了Icon和Name两个Toolkit元素
        itemListView.bindItem = bindItem;//在这里调用Action委托就能实现将数据库指定数据绑定至Toolkit

        itemListView.onSelectionChange += OnListSelectionChange;//事件添加委托
        itemDetailsSection.visible = false;//右侧面板信息不可见(默认点进去的时候不会显示任何物品信息，只有点击一个触发函数后才显示)
    }
    private void OnListSelectionChange(IEnumerable<object> selectedItem)
    {
        activeItem = (ItemDetails)selectedItem.First();
        GetItemDetails();
        itemDetailsSection.visible = true;
    }
    private void GetItemDetails()
    {
        itemDetailsSection.MarkDirtyRepaint();//在下一帧触发中重绘，可以撤销，返回功能
        itemDetailsSection.Q<IntegerField>("ItemID").value = activeItem.itemID;//将当前选中的道具类的ID值传递给滚动详情列表中的ID
        itemDetailsSection.Q<IntegerField>("ItemID").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemID = evt.newValue;
        });//当ID值发生改变执行回调函数

        itemDetailsSection.Q<TextField>("ItemName").value = activeItem.itemName;
        itemDetailsSection.Q<TextField>("ItemName").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemName = evt.newValue;
            itemListView.Rebuild();//重构,刷新一下,当在详情面板中修改了道具的名字,左侧滚动列表中的名字就会刷新重构
        });

        iconPreview.style.backgroundImage = activeItem.itemIcon == null?defaultIcon.texture:activeItem.itemIcon.texture;//获取当前选中的道具的Icon,如果为null就放入默认icon
        itemDetailsSection.Q<ObjectField>("ItemIcon").value = activeItem.itemIcon;
        itemDetailsSection.Q<ObjectField>("ItemIcon").RegisterValueChangedCallback(evt =>
        {
            Sprite newIcon = evt.newValue as Sprite;
            activeItem.itemIcon = newIcon;

            iconPreview.style.backgroundImage = newIcon == null ? defaultIcon.texture : newIcon.texture;
            itemListView.Rebuild();
        });

        //其他所有变量的绑定
        itemDetailsSection.Q<ObjectField>("ItemSprite").value = activeItem.itemOnWorldIcon;
        itemDetailsSection.Q<ObjectField>("ItemSprite").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemOnWorldIcon = (Sprite)evt.newValue;
        });

        itemDetailsSection.Q<EnumField>("ItemType").Init(activeItem.itemType);
        itemDetailsSection.Q<EnumField>("ItemType").value = activeItem.itemType;
        itemDetailsSection.Q<EnumField>("ItemType").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemType = (ItemType)evt.newValue;
        });

        itemDetailsSection.Q<TextField>("Description").value = activeItem.itemDescription;
        itemDetailsSection.Q<TextField>("Description").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemDescription = evt.newValue;
        });

        itemDetailsSection.Q<IntegerField>("ItemUseRadius").value = activeItem.itemUseRadius;
        itemDetailsSection.Q<IntegerField>("ItemUseRadius").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemUseRadius = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("CanPickedup").value = activeItem.canPickedUp;
        itemDetailsSection.Q<Toggle>("CanPickedup").RegisterValueChangedCallback(evt =>
        {
            activeItem.canPickedUp = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("CanDropped").value = activeItem.canDropped;
        itemDetailsSection.Q<Toggle>("CanDropped").RegisterValueChangedCallback(evt =>
        {
            activeItem.canDropped = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("CanCarried").value = activeItem.canCarried;
        itemDetailsSection.Q<Toggle>("CanCarried").RegisterValueChangedCallback(evt =>
        {
            activeItem.canCarried = evt.newValue;
        });

        itemDetailsSection.Q<IntegerField>("Price").value = activeItem.itemPrice;
        itemDetailsSection.Q<IntegerField>("Price").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemPrice = evt.newValue;
        });

        itemDetailsSection.Q<Slider>("SellPercentage").value = activeItem.sellPercentage;
        itemDetailsSection.Q<Slider>("SellPercentage").RegisterValueChangedCallback(evt =>
        {
            activeItem.sellPercentage = evt.newValue;
        });
    }
}