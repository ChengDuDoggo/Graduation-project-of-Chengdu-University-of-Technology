using System;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//�ýű�������ÿһ����Ƭ��ͼ����
[ExecuteInEditMode]//�ڱ༭ģʽ�¾������еĽű�,���Ǳ���Ҫ�����Ϸ��ʼ
public class GridMap : MonoBehaviour
{
    public MapData_SO mapData;
    public GridType gridType;
    private Tilemap currentTilemap;
    private void OnEnable()
    {
        if (!Application.IsPlaying(this))//�����иýű��������ڱ༭���б�����ʱ
        {
            currentTilemap = GetComponent<Tilemap>();//�����Ƭ���
            if (mapData != null)
            {
                mapData.tileProperties.Clear();//���map���ݿⲻΪnull,�Ѹ������б��е��������һ�����»���
            }
        }
    }
    private void OnDisable()
    {
        if (!Application.IsPlaying(this))//�༭��ģʽ��
        {
            currentTilemap = GetComponent<Tilemap>();
            UpdateTileProperties();
#if UNITY_EDITOR//������Ĵ���ֻ����Unity�༭��������,�������ʵ����Ϸ��ʱ,�˶δ��벻������
            if (mapData != null)
            {
                EditorUtility.SetDirty(mapData);//�ڱ༭����ˢ��һ�µ�ͼ���ݿ�
            }
#endif
        }
    }

    private void UpdateTileProperties()
    {
        currentTilemap.CompressBounds();//ѹ��һ����Ƭ��ͼ,��������Ƭ�Ĵ�С�߽�
        if (!Application.IsPlaying(this))
        {
            if (mapData != null)
            {
                //����ѻ��Ʒ�Χ�����½�����
                Vector3Int startPos = currentTilemap.cellBounds.min;
                //����ѻ��Ʒ�Χ�����Ͻ�����
                Vector3Int endPos = currentTilemap.cellBounds.max;
                //ͨ��˫��forѭ���ͻ�õľ������߽ǵ�����ȡ���Ƶ���������е�ÿһ����Ƭ
                for (int x = startPos.x; x < endPos.x; x++)
                {
                    for (int y = startPos.y; y < endPos.y; y++)
                    {
                        TileBase tile = currentTilemap.GetTile(new Vector3Int(x, y, 0));//������Ƭ������ÿһ����Ƭ,TileBase:ÿһ����Ƭ�Ļ�����λ
                        if (tile != null)
                        {
                            TileProperty newTile = new TileProperty
                            {
                                tileCoordinate = new Vector2Int(x, y),
                                gridType = this.gridType,
                                boolTypeValue = true
                            };//�����������ֵ����new�������Ƭ����
                            mapData.tileProperties.Add(newTile);//��newTile����Ƭ���ݴ��뵽�б����ݿ���
                        }
                    }
                }
            }
        }
    }
}
