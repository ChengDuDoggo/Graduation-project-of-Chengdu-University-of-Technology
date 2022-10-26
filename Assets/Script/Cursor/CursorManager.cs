using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MFarm.Map;
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
    private bool cursorPositionValid;//�жϵ�ǰ���λ���Ƿ���Ե㰴
    private ItemDetails currentItem;
    private Transform PlayerTransform => FindObjectOfType<Player>().transform;//�õ�Player��Transform��������Ʒ��ʹ�÷�Χ
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
            CheckPlayerInput();
        }
        else
        {
            SetCursorImage(normal);
        }

    }
    private void CheckPlayerInput()
    {
        if (Input.GetMouseButtonDown(0) && cursorPositionValid)//�����������������굱ǰ�ǿ���״̬
        {
            //ִ�з���
            EventHandler.CallMouseClickedEvent(mouseWorldPos, currentItem);
        }
    }
    #region ���������ʽ
    /// <summary>
    /// ������ƷͼƬ
    /// </summary>
    /// <param name="sprite"></param>
    private void SetCursorImage(Sprite sprite)
    {
        cursorImage.sprite = sprite;
        cursorImage.color = new Color(1, 1, 1, 1);
    }
    private void SetCursorValid()//��⵱ǰ�������ڵ�ͼ�Ͽ��ã�����ͼƬ��ȫ��ʾ
    {
        cursorPositionValid = true;//�����Ե㰴
        cursorImage.color = new Color(1, 1, 1, 1);
    }
    private void SetCursorInvalid()//��⵱ǰ�������ڵ�ͼ�ϲ����ã�����ͼƬ��Ϊ��ɫ��͸��
    {
        cursorPositionValid = false;//��겻�ܵ㰴
        cursorImage.color = new Color(1, 0, 0, 0.5f);
    }
    #endregion
    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        if (!isSelected)
        {
            currentItem = null;
            cursorEnable = false;
            currentSprite = normal;
        }
        else
        {
            currentItem = itemDetails;
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
            cursorEnable = true;
        }
    }
    /// <summary>
    /// ʵʱ������ָ���Ƿ����
    /// </summary>
    private void CheckCursorValid()
    {
        mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,-mainCamera.transform.position.z));//��Ļ����ת��Ϊ��������
        mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);//��������ת��Ϊ��������
        var playerGridPos = currentGrid.WorldToCell(PlayerTransform.position);//�õ��������ڵ���������
        //�ж���ʹ�÷�Χ֮��
        if (Mathf.Abs(mouseGridPos.x - playerGridPos.x) > currentItem.itemUseRadius || Mathf.Abs(mouseGridPos.y - playerGridPos.y) > currentItem.itemUseRadius)
        {
            SetCursorInvalid();
            return;
        }
        TileDetails currentTile = GridMapManager.Instance.GetTileDetailsOnMousePosition(mouseGridPos);
        if (currentTile != null)
        {
            switch (currentItem.itemType)
            {
                case ItemType.Commodity:
                    if (currentTile.canDropItm&&currentItem.canDropped) SetCursorValid(); else SetCursorInvalid();
                    break;
                case ItemType.HolTool:
                    if (currentTile.canDig) SetCursorValid(); else SetCursorInvalid();
                    break;
                case ItemType.WaterTool:
                    if (currentTile.daysSinceDug > -1 && currentTile.daysSinceWatered == -1) SetCursorValid(); else SetCursorInvalid();
                    break;
            }
        }
        else
        {
            SetCursorInvalid();
        }
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
