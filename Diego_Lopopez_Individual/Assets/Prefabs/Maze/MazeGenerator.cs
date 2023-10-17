using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Netcode;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class MazeGenerator : NetworkBehaviour
{
    //Maze generator
    public MazeCell mazeCellPrefab;
    public int mazeWidth;
    public int mazeDepth;
    public int cellWidth;
    public int cellDepth;

    public bool useSeed;

    public int seedVar;
    public NetworkVariable<int> seed = new NetworkVariable<int>();

    private MazeCell[,] mazeGrid;

    //Maze path visualization
    public Transform startPoint;
    public Transform endPoint;

    public LineRenderer Path;
    public NavMeshTriangulation Triangulation;
    public Coroutine DrawPathCoroutine;

    public float pathHeighOffset;
    public float pathUpdateSpeed;




    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            seedVar = Random.Range(1, 1000000);
            Random.InitState(seedVar);
            seed.Value = seedVar;
            Debug.Log("Server created seed " + seedVar);
        }
        if (!IsServer)
        {
            Random.InitState(seed.Value);
            Debug.Log("Client created seed " + seed.Value);
        }

        
       

        mazeGrid = new MazeCell[mazeWidth, mazeDepth];

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeWidth; z++)
            {
                var cell = Instantiate(mazeCellPrefab, new Vector3(x * cellWidth, -2.3f, z * cellDepth), Quaternion.identity);
                mazeGrid[x, z] = cell;

                if (x == 0 && z == 0)
                {
                    cell.FirstCell();
                    startPoint.position = cell.locationPoint.position;
                }

                if (x == mazeWidth - 1 && z == mazeDepth - 1)
                {
                    cell.LastCell();
                    endPoint.position = cell.locationPoint.position;
                }
            }
        }

        GenerateMaze(null, mazeGrid[0, 0]);
        NavMeshSurface nav = GetComponent<NavMeshSurface>();
        nav.BuildNavMesh();

        if(DrawPathCoroutine != null)
        {
            StopCoroutine(DrawPathCoroutine);
        }

        DrawPathCoroutine = StartCoroutine(DrawPath());
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
        } 
        while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x / (int)cellWidth;
        int z = (int)currentCell.transform.position.z / (int)cellDepth;

        if(x + 1  < mazeWidth)
        {
            var cellToRight = mazeGrid[x + 1, z];
            if(cellToRight.isVisited == false)
            {
                yield return cellToRight;
            }
        }

        if(x - 1  >= 0)
        {
            var cellToLeft = mazeGrid[x - 1, z];
            if (cellToLeft.isVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if (z  + 1 < mazeDepth)
        {
            var cellToFront = mazeGrid[x, z + 1];
            if (cellToFront.isVisited == false)
            {
                yield return cellToFront;
            }
        }

        if (z - 1  >= 0)
        {
            var cellToBack = mazeGrid[x, z - 1];
            if (cellToBack.isVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
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

    public IEnumerator DrawPath()
    {
        //yield return new WaitForSeconds(0);
        
        WaitForSeconds Wait = new WaitForSeconds(pathUpdateSpeed);
        NavMeshPath path = new NavMeshPath();

        while(startPoint && endPoint)
        {
            Triangulation = NavMesh.CalculateTriangulation();
            
            if (NavMesh.CalculatePath(startPoint.position, endPoint.position, NavMesh.AllAreas, path))
            {
                Path.positionCount = path.corners.Length;
                for(int i = 0; i < path.corners.Length; i++)
                {
                    Path.SetPosition(i, path.corners[i] + Vector3.up * pathHeighOffset);
                    Debug.Log("Working");
                }
            }
            else
            {
                Debug.Log("Upsi");
            }
            yield return Wait;
        }

        
    }
}
