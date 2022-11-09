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
        [Header("”√”⁄≤‚ ‘")]
        public Vector2Int startPos;
        public Vector2Int finishPos;
        public Tilemap displayMap;
        public TileBase displayTile;
        public bool displayStartAndFinish;
        public bool displayPath;
        private Stack<MovementStep> npcMovementStepStack;
        private void Awake()
        {
            aStar = GetComponent<AStar>();
            npcMovementStepStack = new Stack<MovementStep>();
        }
        private void Update()
        {
            ShowPathOnGridMap();
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

