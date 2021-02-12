﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    GameObject Cube, Plane, Pyramid;
    // Start is called before the first frame update
    void Start()
    {
        Cube = Resources.Load<GameObject>("Prefabs/Cube");
        Pyramid = Resources.Load<GameObject>("Prefabs/Pyramid");
        Plane = Resources.Load<GameObject>("Prefabs/Plane");
        StartCoroutine(createMaze());
    }

    IEnumerator createMaze()
    {
        GameObject Maze = new GameObject("Maze");
        Maze.transform.position = new Vector3(0f, 0f, 0f);

        for (int x = 0; x < 10; x += 2)
        {
            GameObject leftWallCube = createCube(x, 0f, 0f);
            leftWallCube.transform.SetParent(Maze.transform);
        }

        for (int x = 2; x < 10; x += 2)
        {
            GameObject bottomWallCube = createCube(0f, 0f, x);
            bottomWallCube.transform.SetParent(Maze.transform);
        }

        for (int x = 2; x < 10; x += 2)
        {
            GameObject topWallCube = createCube(x, 0f, 8f);
            topWallCube.transform.SetParent(Maze.transform);
        }

        for (int x = 2; x < 10; x += 2)
        {
            GameObject bottomWallCube = createCube(8f, 0f, x);
            bottomWallCube.transform.SetParent(Maze.transform);
        }

        //Maze Walls are generated here
        GameObject wallCube = createCube(5f, 0f, 2.5f);
        wallCube.transform.SetParent(Maze.transform);
        wallCube.transform.localScale = new Vector3(2f, 1f, 0.1f);

        wallCube = createCube(3f, 0f, 5f);
        wallCube.transform.SetParent(Maze.transform);
        wallCube.transform.localScale = new Vector3(2f, 1f, 0.1f);

        GameObject startMarker = createPyramid(17f, -0.5f, 25f);
        startMarker.name = "Start";
        GameObject floorPlane = createPlane(7.7f, -0.5f, 15f);
        floorPlane.name = "Plane";
        floorPlane.transform.localScale = (new Vector3(2.5f, 5f, 1f));
        GameObject endMarker = createPyramid(45f, -0.5f, 90f);
        endMarker.name = "End";

        Maze.transform.localScale = new Vector3(7.5f, 1f, 15f);
        yield return null;
    }

    GameObject createCube(float xpos, float ypos, float zpos)
    {
        //Cube Prefab instantiated at specified X,Y,Z co-ordinates
        return Instantiate(Cube, new Vector3(xpos, ypos, zpos), Quaternion.identity);
    }

    GameObject createPyramid(float xpos, float ypos, float zpos)
    {
        //Pyramid Prefab instantiated at specified X,Y,Z co-ordinates
        return Instantiate(Pyramid, new Vector3(xpos, ypos, zpos), Quaternion.identity);
    }

    GameObject createPlane(float xpos, float ypos, float zpos)
    {
        //Plane Prefab instantiated at specified X,Y,Z co-ordinates
        return Instantiate(Plane, new Vector3(xpos, ypos, zpos), Quaternion.Euler(new Vector3(90f, 0f, 0f)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
