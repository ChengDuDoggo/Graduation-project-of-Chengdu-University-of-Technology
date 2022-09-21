using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System;


public class ItemEditor : EditorWindow
{
    private ItemDataList_SO dataBase; //创建的数据库中的数据
    private List<ItemDetails> itemList = new List<ItemDetails>();//new一下ItemDtails,相当于数据库中的详细数据列表
    private VisualTreeAsset itemRowTemplate; //获取自己在UIToolkit中定义的样式
    private ListView itemListView;//获得VisualElement中的滚动列表

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

        //变量赋值
        itemListView = root.Q<VisualElement>("ItemList").Q<ListView>("ListView");

        //加载数据
        LoadDataBase();

        //生成ListView
        GenerateListView();
    }
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
        itemListView.makeItem = makeItem;
        itemListView.bindItem = bindItem;
    }
}