using System;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//该脚本挂载在每一个瓦片地图上面
[ExecuteInEditMode]//在编辑模式下就能运行的脚本,而非必须要点击游戏开始
public class GridMap : MonoBehaviour
{
    public MapData_SO mapData;
    public GridType gridType;
    private Tilemap currentTilemap;
    private void OnEnable()
    {
        if (!Application.IsPlaying(this))//当带有该脚本的物体在编辑器中被激活时
        {
            currentTilemap = GetComponent<Tilemap>();//获得瓦片组件
            if (mapData != null)
            {
                mapData.tileProperties.Clear();//如果map数据库不为null,把该物体列表中的属性清除一下重新绘制
            }
        }
    }
    private void OnDisable()
    {
        if (!Application.IsPlaying(this))//编辑器模式下
        {
            currentTilemap = GetComponent<Tilemap>();
            UpdateTileProperties();
#if UNITY_EDITOR//这里面的代码只会在Unity编辑器中运行,当打包出实际游戏中时,此段代码不会运行
            if (mapData != null)
            {
                EditorUtility.SetDirty(mapData);//在编辑器中刷新一下地图数据库
            }
#endif
        }
    }

    private void UpdateTileProperties()
    {
        currentTilemap.CompressBounds();//压缩一下瓦片地图,方便获得瓦片的大小边界
        if (!Application.IsPlaying(this))
        {
            if (mapData != null)
            {
                //获得已绘制范围的左下角坐标
                Vector3Int startPos = currentTilemap.cellBounds.min;
                //获得已绘制范围的右上角坐标
                Vector3Int endPos = currentTilemap.cellBounds.max;
                //通过双重for循环和获得的矩形两边角点来获取绘制的这个矩阵中的每一块瓦片
                for (int x = startPos.x; x < endPos.x; x++)
                {
                    for (int y = startPos.y; y < endPos.y; y++)
                    {
                        TileBase tile = currentTilemap.GetTile(new Vector3Int(x, y, 0));//根据瓦片坐标获得每一块瓦片,TileBase:每一块瓦片的基本单位
                        if (tile != null)
                        {
                            TileProperty newTile = new TileProperty
                            {
                                tileCoordinate = new Vector2Int(x, y),
                                gridType = this.gridType,
                                boolTypeValue = true
                            };//将具体的属性值赋予new的这个瓦片属性
                            mapData.tileProperties.Add(newTile);//将newTile的瓦片数据存入到列表数据库中
                        }
                    }
                }
            }
        }
    }
}
