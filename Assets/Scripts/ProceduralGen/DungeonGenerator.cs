using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DungeonGenerator : MonoBehaviour
{

    public class Cell
    {
        public bool HasBeenVisited = false;
        public bool[] CellStatus = new bool[4];
    }


    public Vector2 Size;
    public int StartPosition = 0;
    public GameObject RoomPrefab;
    public Vector2 RoomOffset;

    List<Cell> board;
    private void Start() 
    {
        GenerateMaze();
    }
    void GenerateDungeon()
    {
        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                if (board[Mathf.FloorToInt(i+j*Size.x)].HasBeenVisited)
                {
                    RoomBehaviour newRoom = Instantiate(RoomPrefab, new Vector3(i*RoomOffset.x, 0, -j*RoomOffset.y), Quaternion.identity).GetComponent<RoomBehaviour>();
                    newRoom.UpdateRoom(board[Mathf.FloorToInt(i+j*Size.x)].CellStatus);

                    newRoom.name = $"Cell[{i},{j}]";
                }
            }
        }
    }

    void GenerateMaze()
    {
        board = new List<Cell>();

        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = StartPosition;

        Stack<int> path = new Stack<int>();

        int k = 0;

        while (k < 100)
        {
            if (currentCell == board.Count)
                break;
                
            k++;
            board[currentCell].HasBeenVisited = true;

            List<int> cellNeighbors = FindNeighbors(currentCell);

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

                if (newCell > currentCell)
                {
                    if (newCell - 1 == currentCell)
                    {
                        // RIGHT
                        board[currentCell].CellStatus[3] = true;
                        currentCell = newCell;
                        board[currentCell].CellStatus[2] =  true;
                    }
                    else
                    {
                        // DOWN
                        board[currentCell].CellStatus[1] = true;
                        currentCell = newCell;
                        board[currentCell].CellStatus[0] =  true;
                    }
                }
                else
                {
                    if (newCell + 1 == currentCell)
                    {
                        // LEFT
                        board[currentCell].CellStatus[2] = true;
                        currentCell = newCell;
                        board[currentCell].CellStatus[3] =  true;
                    }
                    else
                    {
                        // UP
                        board[currentCell].CellStatus[0] = true;
                        currentCell = newCell;
                        board[currentCell].CellStatus[1] =  true;
                    }
                }
            }
        }

        GenerateDungeon();
    }

    List<int> FindNeighbors(int currentCell)
    {
        List<int> neighbors = new List<int>();

        if (currentCell - Size.x >= 0 && !board[Mathf.FloorToInt(currentCell - Size.x)].HasBeenVisited)
            neighbors.Add(Mathf.FloorToInt(currentCell - Size.x));
        if (currentCell + Size.x < board.Count && !board[Mathf.FloorToInt(currentCell + Size.x)].HasBeenVisited)
            neighbors.Add(Mathf.FloorToInt(currentCell + Size.x));

        if ((currentCell+1) % Size.x != 0 && !board[Mathf.FloorToInt(currentCell + 1)].HasBeenVisited)
            neighbors.Add(Mathf.FloorToInt(currentCell + 1));
        if (currentCell % Size.x != 0 && !board[Mathf.FloorToInt(currentCell - 1)].HasBeenVisited)
            neighbors.Add(Mathf.FloorToInt(currentCell - 1));
        
        return neighbors;
    }
}