using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MFarm.Map;
//此脚本来控制鼠标在不同功能下切换不同的状态图片
public class CursorManager : MonoBehaviour
{
    public Sprite normal, tool, seed,item;
    private Sprite currentSprite;//存储当前鼠标图片
    private Image cursorImage;//Image是组件,Sprite是图片组件中存放的图片内容
    private RectTransform cursorCanvas;
    //鼠标检测
    private Camera mainCamera;//拿到摄像机用于屏幕坐标转化为世界坐标
    private Grid currentGrid;//拿到当前网格用于世界坐标转化为格子坐标
    private Vector3 mouseWorldPos;//世界坐标
    private Vector3Int mouseGridPos;//网格坐标
    private bool cursorEnable;
    private bool cursorPositionValid;//判断当前鼠标位置是否可以点按
    private ItemDetails currentItem;
    private Transform PlayerTransform => FindObjectOfType<Player>().transform;//拿到Player的Transform来限制物品的使用范围
    private void Start()
    {
        cursorCanvas = GameObject.FindGameObjectWithTag("CursorCanvas").GetComponent<RectTransform>();
        cursorImage = cursorCanvas.GetChild(0).GetComponent<Image>();
        currentSprite = normal;
        SetCursorImage(normal);
        mainCamera = Camera.main;//获取主摄像机
    }
    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;//注册一个当选择物品的时候触发的事件
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;//切换场景之后触发事件
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
        cursorImage.transform.position = Input.mousePosition;//图片始终跟随鼠标移动
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
        if (Input.GetMouseButtonDown(0) && cursorPositionValid)//如果按下鼠标左键切鼠标当前是可用状态
        {
            //执行方法
            EventHandler.CallMouseClickedEvent(mouseWorldPos, currentItem);
        }
    }
    #region 设置鼠标样式
    /// <summary>
    /// 设置物品图片
    /// </summary>
    /// <param name="sprite"></param>
    private void SetCursorImage(Sprite sprite)
    {
        cursorImage.sprite = sprite;
        cursorImage.color = new Color(1, 1, 1, 1);
    }
    private void SetCursorValid()//检测当前鼠标道具在地图上可用，道具图片完全显示
    {
        cursorPositionValid = true;//鼠标可以点按
        cursorImage.color = new Color(1, 1, 1, 1);
    }
    private void SetCursorInvalid()//检测当前鼠标道具在地图上不可用，道具图片变为红色半透明
    {
        cursorPositionValid = false;//鼠标不能点按
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
            currentSprite = itemDetails.itemType switch//根据物品的类型来返回指定的图片
            {
                //WORKFLOW:添加所有类型对应的图片
                ItemType.Seed => seed,
                ItemType.Commodity => item,
                ItemType.ChopTool => tool,
                ItemType.HolTool => tool,
                ItemType.WaterTool => tool,
                ItemType.BreakTool => tool,
                ItemType.ReapTool => tool,
                ItemType.Furniture => tool,
                _ => normal //默认返回的是normal图片
            };
            cursorEnable = true;
        }
    }
    /// <summary>
    /// 实时监测鼠标指针是否可用
    /// </summary>
    private void CheckCursorValid()
    {
        mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,-mainCamera.transform.position.z));//屏幕坐标转化为世界坐标
        mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);//世界坐标转化为网格坐标
        var playerGridPos = currentGrid.WorldToCell(PlayerTransform.position);//拿到人物所在的网格坐标
        //判断在使用范围之内
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
    /// 是否与UI互动
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
