using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


public class DungeonGenerator : MonoBehaviour
{
    [System.Serializable]
    public class Cell
    {
        public bool HasBeenVisited = false;
        public bool[] CellStatus = new bool[4];
        public bool IsNull = true;
        public Rule CellRule;
    }
    [System.Serializable]
    public class Rule
    {
        public Vector2 Size;
        public GameObject RoomPrefab;
    }

    public List<Rule> RoomPrefabs;
    public Vector2 RoomOffset;
    public Vector2 BaseSize = new();
    public Vector2 DungeonSize = new();
    public int StartPosition = 0;

    public List<Cell> board;
    public Rule FinishRule;
    public Rule StartRule;

    bool hasStart = false;
    bool hasFinish = false;
    private void Start() 
    {
        GenerateMaze();
    }
    void GenerateDungeon()
    {
        for (int i = 0; i < DungeonSize.x; i++)
        {
            for (int j = 0; j < DungeonSize.y; j++)
            {
                if (board[Mathf.FloorToInt(i+j* DungeonSize.x)].HasBeenVisited && !board[Mathf.FloorToInt(i + j * DungeonSize.x)].IsNull)
                {
                    if (!hasFinish && Random.Range(0f, 1f) > 0.84f)
                    {
                        RoomBehaviour newFinish = Instantiate(FinishRule.RoomPrefab, new Vector3(i * RoomOffset.x, 0, -j * RoomOffset.y), Quaternion.identity).GetComponent<RoomBehaviour>();
                        newFinish.UpdateRoom(board[Mathf.FloorToInt(i + j * DungeonSize.x)].CellStatus);

                        newFinish.name = $"FINISH";
                        hasFinish = true;
                    }
                    if (!hasStart && Random.Range(0f, 1f) > 0.84f)
                    {
                        RoomBehaviour newStart = Instantiate(StartRule.RoomPrefab, new Vector3(i * RoomOffset.x, 0, -j * RoomOffset.y), Quaternion.identity).GetComponent<RoomBehaviour>();
                        newStart.UpdateRoom(board[Mathf.FloorToInt(i + j * DungeonSize.x)].CellStatus);

                        newStart.name = $"START";
                        hasStart = true;
                    }
                    RoomBehaviour newRoom = Instantiate(board[Mathf.FloorToInt(i + j * DungeonSize.x)].CellRule.RoomPrefab, new Vector3(i*RoomOffset.x, 0, -j*RoomOffset.y), Quaternion.identity).GetComponent<RoomBehaviour>();
                    newRoom.UpdateRoom(board[Mathf.FloorToInt(i+j* DungeonSize.x)].CellStatus);

                    newRoom.name = $"Cell[{i},{j}]";
                }
            }
        }
    }

    void GenerateMaze()
    {
        board = new List<Cell>();

        for (int i = 0; i < DungeonSize.x; i++)
        {
            for (int j = 0; j < DungeonSize.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = StartPosition;

        Stack<int> path = new Stack<int>();

        int k = 0;

        while (k < 2000)
        {

            if (currentCell == board.Count)
                break;
                
            k++;
            board[currentCell].HasBeenVisited = true;

            List<int> cellNeighbors = FindNeighbors(currentCell);
            Rule randomRule = RoomPrefabs[Random.Range(0, RoomPrefabs.Count)];

            if (cellNeighbors.Count == 0)
            {
                if (path.Count == 0)
                    break;
                else
                    currentCell = path.Pop(); 
            }
            else
            {
                path.Push(currentCell);

                int newCell = cellNeighbors[Random.Range(0, cellNeighbors.Count)];

                int CellX = (int)(randomRule.Size.x / BaseSize.x);
                int CellY = (int)(randomRule.Size.y / BaseSize.y);
                if (CellX > 1 || CellY > 1)
                {
                    for (int x = 0; x < (CellX*CellY); x++)
                    {
                        board[currentCell + x].HasBeenVisited = true;
                        board[currentCell + x].IsNull = true;
                        Debug.Log(currentCell+x);
                    }
                    board[currentCell].HasBeenVisited = true;
                    board[currentCell].IsNull = false;
                    board[currentCell].CellRule = randomRule;
                }
                else
                {
                    if (newCell > currentCell)
                    {
                        if (newCell - 1 == currentCell)
                        {
                            // RIGHT
                            board[currentCell].CellStatus[3] = true;
                            board[currentCell].IsNull = false;
                            board[currentCell].CellRule = randomRule;
                            currentCell = newCell;
                            board[currentCell].CellStatus[2] = true;
                            board[currentCell].IsNull = false;
                            board[currentCell].CellRule = RoomPrefabs[Random.Range(0, RoomPrefabs.Count - 1)];
                        }
                        else
                        {
                            // DOWN
                            board[currentCell].CellStatus[1] = true;
                            board[currentCell].IsNull = false;
                            board[currentCell].CellRule = randomRule;
                            currentCell = newCell;
                            board[currentCell].CellStatus[0] = true;
                            board[currentCell].IsNull = false;
                            board[currentCell].CellRule = RoomPrefabs[Random.Range(0, RoomPrefabs.Count - 1)];
                        }
                    }
                    else
                    {
                        if (newCell + 1 == currentCell)
                        {
                            // LEFT
                            board[currentCell].CellStatus[2] = true;
                            board[currentCell].IsNull = false;
                            board[currentCell].CellRule = randomRule;
                            currentCell = newCell;
                            board[currentCell].CellStatus[3] = true;
                            board[currentCell].IsNull = false;
                            board[currentCell].CellRule = RoomPrefabs[Random.Range(0, RoomPrefabs.Count - 1)];
                        }
                        else
                        {
                            // UP
                            board[currentCell].CellStatus[0] = true;
                            board[currentCell].IsNull = false;
                            board[currentCell].CellRule = randomRule;
                            currentCell = newCell;
                            board[currentCell].CellStatus[1] = true;
                            board[currentCell].IsNull = false;
                            board[currentCell].CellRule = RoomPrefabs[Random.Range(0, RoomPrefabs.Count - 1)];
                        }
                    }
                }
            }
        }

        GenerateDungeon();
    }

    List<int> FindNeighbors(int currentCell)
    {
        List<int> neighbors = new List<int>();

        if (currentCell - DungeonSize.x >= 0 && !board[Mathf.FloorToInt(currentCell - DungeonSize.x)].HasBeenVisited)
            neighbors.Add(Mathf.FloorToInt(currentCell - DungeonSize.x));
        if (currentCell + DungeonSize.x < board.Count && !board[Mathf.FloorToInt(currentCell + DungeonSize.x)].HasBeenVisited)
            neighbors.Add(Mathf.FloorToInt(currentCell + DungeonSize.x));

        if ((currentCell+1) % DungeonSize.x != 0 && !board[Mathf.FloorToInt(currentCell + 1)].HasBeenVisited)
            neighbors.Add(Mathf.FloorToInt(currentCell + 1));
        if (currentCell % DungeonSize.x != 0 && !board[Mathf.FloorToInt(currentCell - 1)].HasBeenVisited)
            neighbors.Add(Mathf.FloorToInt(currentCell - 1));
        
        return neighbors;
    }
}