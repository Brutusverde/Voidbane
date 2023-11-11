using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject frontWall;
    public GameObject backWall;

    public GameObject unvisitedBlock;

    public GameObject first;

    public GameObject last;

    public Transform locationPoint;

    public GameObject leftFloor;
    public GameObject rightFloor;
    public GameObject frontFloor;
    public GameObject backFloor;


    //public GameObject leftFloor;

    public bool isVisited { get; private set; }

    public void Visit()
    {
        isVisited = true;
        unvisitedBlock.SetActive(false);
    }


    #region Clear walls and floors
    public void ClearLeftWall()
    {
        leftWall.SetActive(false);
        leftFloor.SetActive(false);
    }

    public void ClearRightWall()
    {
        rightWall.SetActive(false);
        rightFloor.SetActive(false);
    }

    public void ClearFrontWall()
    {
        frontWall.SetActive(false);
        frontFloor.SetActive(false);
    }

    public void ClearBackWall()
    {
        backWall.SetActive(false);
        backFloor.SetActive(false);
    }
    #endregion


    public void FirstCell()
    {
        first.SetActive(true);
    }

    public void LastCell()
    {
        last.SetActive(true);
    }
}
