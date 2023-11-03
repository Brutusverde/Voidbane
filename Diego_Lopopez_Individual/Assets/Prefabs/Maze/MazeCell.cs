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

    public GameObject leftBlock;
    public GameObject rightBlock;
    public GameObject frontBlock;
    public GameObject backBlock;

    public bool isVisited { get; private set; }

    public void Visit()
    {
        isVisited = true;
        unvisitedBlock.SetActive(false);
    }

    public void ClearLeftWall()
    {
        leftWall.SetActive(false);
    }

    public void ClearRightWall()
    {
        rightWall.SetActive(false);
    }

    public void ClearFrontWall()
    {
        frontWall.SetActive(false);
    }

    public void ClearBackWall()
    {
        backWall.SetActive(false);
    }

    public void FirstCell()
    {
        first.SetActive(true);
    }

    public void LastCell()
    {
        last.SetActive(true);
    }
}
