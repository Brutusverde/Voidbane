using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MazeGenerator : MonoBehaviour
{

    public MazeCell mazeCellPrefab;
    public int mazeWidth;
    public int mazeDepth;
    public float cellWidth;
    public float cellDepth;

    private MazeCell[,] mazeGrid;



    void Start()
    {
        mazeGrid = new MazeCell[mazeWidth, mazeDepth];

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeWidth; z++)
            {
                mazeGrid[x, z] = Instantiate(mazeCellPrefab, new Vector3(x * cellWidth, -2.3f, z * cellDepth), Quaternion.identity);
            }
        }

        GenerateMaze(null, mazeGrid[0, 0]);
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;
        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
        
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        if(x + 1  < mazeWidth)
        {
            var cellToRight = mazeGrid[x + 1, z];
            if(cellToRight.isVisited == false)
            {
                Debug.Log(1);
                yield return cellToRight;
            }
        }

        if(x - 1  >= 0)
        {
            var cellToLeft = mazeGrid[x - 1, z];
            if (cellToLeft.isVisited == false)
            {
                Debug.Log(2);
                yield return cellToLeft;
            }
        }

        if (z  + 1 < mazeDepth)
        {
            var cellToFront = mazeGrid[x, z + 1];
            if (cellToFront.isVisited == false)
            {
                Debug.Log(3);
                yield return cellToFront;
            }
        }

        if (z - 1  >= 0)
        {
            var cellToBack = mazeGrid[x, z - 1];
            if (cellToBack.isVisited == false)
            {
                Debug.Log(4);
                yield return cellToBack;
            }
        }

        if (z == mazeWidth)
        {
            currentCell.LastCell();
        }
    } 

    void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if(previousCell == null)
        {
            return;
        }

        if(previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }

}
