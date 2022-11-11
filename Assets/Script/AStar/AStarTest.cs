using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace MFarm.AStar
{
    public class AStarTest : MonoBehaviour
    {
        private AStar aStar;
        [Header("���ڲ���")]
        public Vector2Int startPos;
        public Vector2Int finishPos;
        public Tilemap displayMap;
        public TileBase displayTile;
        public bool displayStartAndFinish;
        public bool displayPath;
        private Stack<MovementStep> npcMovementStepStack;
        [Header("�����ƶ�NPC")]
        public NPCMovement npcMovement;
        public bool moveNPC;
        [SceneName] public string targetScene;
        public Vector2Int targetPos;
        public AnimationClip stopClip;
        private void Awake()
        {
            aStar = GetComponent<AStar>();
            npcMovementStepStack = new Stack<MovementStep>();
        }
        private void Update()
        {
            ShowPathOnGridMap();
            if (moveNPC)
            {
                moveNPC = false;
                var schedule = new SchedulDetails(0, 0, 0, 0, Season.����, targetScene, targetPos, stopClip, true);
                npcMovement.BuildPath(schedule);//��NPC�������ʱ����������ƶ�·��
            }
        }
        private void ShowPathOnGridMap()
        {
            if (displayMap != null && displayTile != null)
            {
                if (displayStartAndFinish)
                {
                    displayMap.SetTile((Vector3Int)startPos, displayTile);
                    displayMap.SetTile((Vector3Int)finishPos, displayTile);
                }
                else
                {
                    displayMap.SetTile((Vector3Int)startPos, null);
                    displayMap.SetTile((Vector3Int)finishPos, null);
                }
                if (displayPath)
                {
                    var sceneName = SceneManager.GetActiveScene().name;
                    aStar.BuildPath(sceneName, startPos, finishPos, npcMovementStepStack);
                    foreach (var step in npcMovementStepStack)
                    {
                        displayMap.SetTile((Vector3Int)step.gridCoordinate, displayTile);
                    }
                }
                else
                {
                    if (npcMovementStepStack.Count > 0)
                    {
                        foreach (var step in npcMovementStepStack)
                        {
                            displayMap.SetTile((Vector3Int)step.gridCoordinate, null);
                        }
                        npcMovementStepStack.Clear();
                    }
                }
            }
        }
    }
}

