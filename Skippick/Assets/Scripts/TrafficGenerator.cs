using System.Collections;
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
    
    private List<Vector3> lanePositions;
    private List<List<GameObject>> generatedBounceableObjects;
    private GameObject player;
    private float lastBounceableZPos;
    private bool removeTrafficWhenReady = false;

    ///////////////////////////////////////////////
    /// MONOBEHAVIOR METHODS
    ///////////////////////////////////////////////
    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        lastBounceableZPos = player.transform.position.z + playerStats.bounceDistance;
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
        if (generatedBounceableObjects == null)
        {
            return null;
        }

        return generatedBounceableObjects[index];
    }
    public int GetListCount()
    {
        if (generatedBounceableObjects == null)
        {
            return 0;
        }

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
        if (generatedBounceableObjects == null)
        {
            generatedBounceableObjects = new List<List<GameObject>>();
        }

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
