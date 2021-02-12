﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlayer : MonoBehaviour
{
    Terrain terrain;
    TerrainData terrainData;
    public GameObject playerCube;
    // Start is called before the first frame update
    void Start()
    {
        terrainData = Terrain.activeTerrain.terrainData;
        Instantiate(playerCube, new Vector3(Random.Range(0, terrainData.size.x + 1), 1000, Random.Range(0, terrainData.size.z + 1)), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
