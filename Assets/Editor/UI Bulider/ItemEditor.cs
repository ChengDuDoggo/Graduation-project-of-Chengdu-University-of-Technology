using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System;


public class ItemEditor : EditorWindow
{
    private ItemDataList_SO dataBase; //���������ݿ��е�����
    private List<ItemDetails> itemList = new List<ItemDetails>();//newһ��ItemDtails,�൱�����ݿ��е���ϸ�����б�
    private VisualTreeAsset itemRowTemplate; //��ȡ�Լ���UIToolkit�ж������ʽ
    private ListView itemListView;//���VisualElement�еĹ����б�

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

        //ֱ��ͨ������·����ʽ�õ���ʽģ��
        itemRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Bulider/ItemRowTemplate.uxml");

        //������ֵ
        itemListView = root.Q<VisualElement>("ItemList").Q<ListView>("ListView");

        //��������
        LoadDataBase();

        //����ListView
        GenerateListView();
    }
    private void LoadDataBase()//�õ�Assets�����Ǵ��������ݿ��ļ�
    {
        var dataArray = AssetDatabase.FindAssets("ItemDataList_SO");//������Assets���ҵ�����ΪItemDataList_SO����Դ�ļ�����Ϊ����,����ø����ļ���GUID
        if (dataArray.Length > 1)//���Assets�д���ItemDataList_SO����Դ�ļ�
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);//ͨ����õ������еľ���һ���ļ���GUID�������ļ���·��
            //��Ϊ��ֻ��һ��ItemDataList_SO�ļ�,�����ҿ���ֱ��дdataArray[0]
            dataBase = AssetDatabase.LoadAssetAtPath(path, typeof(ItemDataList_SO)) as ItemDataList_SO;//��ͨ���ļ�·���������ݿ���Դ
        }
        itemList = dataBase.itemDetailsList;//Ϊ�б�ֵ
        EditorUtility.SetDirty(dataBase);//��עһ������,�����עһ�����ݷ����޷���������
    }
    private void GenerateListView()//����ListView��������
    {
        Func<VisualElement> makeItem = () => itemRowTemplate.CloneTree(); //ÿ����һ������ListView�����Ϳ�¡һ����ʽģ��
        Action<VisualElement, int> bindItem = (e, i) =>
         {
             if (i < itemList.Count)//����ȷ�����С���б��е�������������ᱨ��
             {
                 if (itemList[i].itemIcon != null)
                 {
                     e.Q<VisualElement>("Icon").style.backgroundImage = itemList[i].itemIcon.texture;//�����ݿ��е����ݸ�ֵ�󶨸�UI Toolkit
                 }
                 e.Q<Label>("Name").text = itemList[i] == null ? "NO ITEM" : itemList[i].itemName;
             }
         };
        //e.Q<VisualElement>:���Ǵ���UI Toolkit�о����Ԫ��,VisualElement��������Ŀհ�����
        //e.Q<Label>��������ı���ǩ���ڴ��ô��뽫����ʵ����

        itemListView.fixedItemHeight = 60;//�̶����ָ߶�
        itemListView.itemsSource = itemList;//���б��е����ݷ��������
        itemListView.makeItem = makeItem;
        itemListView.bindItem = bindItem;
    }
}