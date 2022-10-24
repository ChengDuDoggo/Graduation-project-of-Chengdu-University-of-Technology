using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//�˽ű�����������ڲ�ͬ�������л���ͬ��״̬ͼƬ
public class CursorManager : MonoBehaviour
{
    public Sprite normal, tool, seed,item;
    private Sprite currentSprite;//�洢��ǰ���ͼƬ
    private Image cursorImage;//Image�����,Sprite��ͼƬ����д�ŵ�ͼƬ����
    private RectTransform cursorCanvas;
    //�����
    private Camera mainCamera;//�õ������������Ļ����ת��Ϊ��������
    private Grid currentGrid;//�õ���ǰ����������������ת��Ϊ��������
    private Vector3 mouseWorldPos;//��������
    private Vector3Int mouseGridPos;//��������
    private bool cursorEnable;
    private void Start()
    {
        cursorCanvas = GameObject.FindGameObjectWithTag("CursorCanvas").GetComponent<RectTransform>();
        cursorImage = cursorCanvas.GetChild(0).GetComponent<Image>();
        currentSprite = normal;
        SetCursorImage(normal);
        mainCamera = Camera.main;//��ȡ�������
    }
    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;//ע��һ����ѡ����Ʒ��ʱ�򴥷����¼�
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;//�л�����֮�󴥷��¼�
    }
    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
    }

    private void OnBeforeSceneUnloadEvent()
    {
        cursorEnable = false;
    }

    private void OnAfterSceneLoadedEvent()
    {
        currentGrid = FindObjectOfType<Grid>();
        cursorEnable = true;
    }

    private void Update()
    {
        if (cursorCanvas == null)
            return;
        cursorImage.transform.position = Input.mousePosition;//ͼƬʼ�ո�������ƶ�
        if (!InteractWithUI()&&cursorEnable)
        {
            SetCursorImage(currentSprite);
            CheckCursorValid();
        }
        else
        {
            SetCursorImage(normal);
        }

    }
    /// <summary>
    /// ������ƷͼƬ
    /// </summary>
    /// <param name="sprite"></param>
    private void SetCursorImage(Sprite sprite)
    {
        cursorImage.sprite = sprite;
        cursorImage.color = new Color(1, 1, 1, 1);
    }
    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        if (!isSelected)
        {
            currentSprite = normal;
        }
        else
        {
            currentSprite = itemDetails.itemType switch//������Ʒ������������ָ����ͼƬ
            {
                //WORKFLOW:����������Ͷ�Ӧ��ͼƬ
                ItemType.Seed => seed,
                ItemType.Commodity => item,
                ItemType.ChopTool => tool,
                ItemType.HolTool => tool,
                ItemType.WaterTool => tool,
                ItemType.BreakTool => tool,
                ItemType.ReapTool => tool,
                ItemType.Furniture => tool,
                _ => normal //Ĭ�Ϸ��ص���normalͼƬ
            };
        }
    }
    /// <summary>
    /// ʵʱ������ָ���Ƿ����
    /// </summary>
    private void CheckCursorValid()
    {
        mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,-mainCamera.transform.position.z));//��Ļ����ת��Ϊ��������
        mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);//��������ת��Ϊ��������
    }
    /// <summary>
    /// �Ƿ���UI����
    /// </summary>
    /// <returns></returns>
    private bool InteractWithUI()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        return false;
    }
}
