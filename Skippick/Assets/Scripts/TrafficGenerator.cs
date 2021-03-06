﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrafficGenerator : MonoBehaviour {

    ///////////////////////////////////////////////
    /// MEMBERS
    ///////////////////////////////////////////////
    public PlayerStats playerStats;
    public GameObject bounceableObject;
    public float bounceablePercentage = 70;
    [HideInInspector]
    public float lastBounceableZPos;

    private List<Vector3> lanePositions;
    private List<List<GameObject>> generatedBounceableObjects = new List<List<GameObject>>();
    private GameObject player;

    ///////////////////////////////////////////////
    /// MONOBEHAVIOR METHODS
    ///////////////////////////////////////////////
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
	void Update ()
    {
        // Nothing yet
	}

    ///////////////////////////////////////////////
    /// PUBLIC METHODS
    ///////////////////////////////////////////////
    public void GenerateTraffic()
    {
        SetLanePositions();
        GenerateBounceableTraffic();
    }
    public List<GameObject> GetBounceableObjectsAtIndex(int index)
    {
        if (generatedBounceableObjects.Count == 0)
        {
            return null;
        }

        return generatedBounceableObjects[index];
    }
    public int GetListCount()
    {

        return generatedBounceableObjects.Count; 
    }

    ///////////////////////////////////////////////
    /// PRIVATE METHODS
    ///////////////////////////////////////////////
    private void SetLanePositions()
    {
        // Find lanes and sort them (Lane1, Lane2, ...)
        GameObject[] lanes = GameObject.FindGameObjectsWithTag("Lane");
        Array.Sort(lanes, CompareObjectNames);

        lanePositions = new List<Vector3>();
        foreach (GameObject lane in lanes)
        {
            lanePositions.Add(lane.transform.position);
        }
    }
    private void GenerateBounceableTraffic()
    {
        GenerateBounceableTrafficAtZPos(lastBounceableZPos);
    }
    private void GenerateBounceableTrafficAtZPos(float zPosition)
    {
        // Base case: if the zPosition has reached the end of the road, return
        if (zPosition >= lastBounceableZPos + RoadGenerator.ROAD_DISTANCE)
        {
            lastBounceableZPos = zPosition;
            return;
        }

        generatedBounceableObjects.Add(new List<GameObject>());

        while (generatedBounceableObjects[generatedBounceableObjects.Count-1].Count == 0)
        {
            foreach (Vector3 lanePos in lanePositions)
            {
                float percentage = UnityEngine.Random.value * 100;

                // Chance to generate a bounceableobject at the lane postion
                if (percentage <= bounceablePercentage)
                {
                    Vector3 genPos;
                    genPos.x = lanePos.x;
                    genPos.y = bounceableObject.transform.position.y;
                    genPos.z = zPosition;

                    CreateBounceableObjectAtPosition(genPos);
                }
            }
        }

        GenerateBounceableTrafficAtZPos(zPosition + playerStats.bounceDistance);
    }
    private void CreateBounceableObjectAtPosition(Vector3 position)
    {
        GameObject newObject = GameObject.Instantiate(bounceableObject);
        newObject.transform.position = position;
        generatedBounceableObjects[generatedBounceableObjects.Count-1].Add(newObject);
    }
    private int CompareObjectNames(GameObject x, GameObject y)
    {
        return x.name.CompareTo(y.name);
    }

}
