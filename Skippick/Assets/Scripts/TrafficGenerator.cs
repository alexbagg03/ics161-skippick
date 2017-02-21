using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrafficGenerator : MonoBehaviour {

    ///////////////////////////////////////////////
    /// MEMBERS
    ///////////////////////////////////////////////
    public Transform playerTransform;
    public PlayerStats playerStats;
    public GameObject bounceableObject;
    
    private Vector3 currentPosition;
    private float lastBounceableZPos;
    private List<Vector3> lanePositions;
    private List<List<GameObject>> generatedBounceableObjects;
    private bool removeTrafficWhenReady = false;

    ///////////////////////////////////////////////
    /// MONOBEHAVIOR METHODS
    ///////////////////////////////////////////////
    void Start ()
    {
        lastBounceableZPos = playerTransform.position.z;
        SetLanePositions();
        GenerateBounceableTraffic();
    }
	void Update ()
    {
        if (removeTrafficWhenReady)
        {
            if (playerTransform.position.z > PlayerController.lastPlayerBounceZPos)
            {
                RemoveOldTraffic();
                removeTrafficWhenReady = false;
            }
        }
	}

    ///////////////////////////////////////////////
    /// PUBLIC METHODS
    ///////////////////////////////////////////////
    public void GenerateBounceableTraffic()
    {
        if (generatedBounceableObjects == null)
        {
            generatedBounceableObjects = new List<List<GameObject>>();
        }

        generatedBounceableObjects.Add(new List<GameObject>());

        GenerateBounceableTrafficAtZPos(lastBounceableZPos);
    }
    public void RemoveOldTrafficWhenReady()
    {
        removeTrafficWhenReady = true;
    }
    public void RemoveOldTraffic()
    {
        List<GameObject> oldObjects = generatedBounceableObjects[0];
        List<GameObject> objectsToRemove = new List<GameObject>();

        for (int i = 0; i < lanePositions.Count; i++)
        {
            objectsToRemove.Add(oldObjects[i]);
        }
        foreach (GameObject obj in objectsToRemove)
        {
            oldObjects.Remove(obj);
            Destroy(obj);
        }

        if (oldObjects.Count == 0)
        {
            generatedBounceableObjects.Remove(oldObjects);
        }
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
    private void GenerateBounceableTrafficAtZPos(float zPosition)
    {
        if (zPosition >= lastBounceableZPos + RoadGenerator.ROAD_DISTANCE)
        {
            lastBounceableZPos = zPosition;
            return;
        }

        foreach (Vector3 lanePos in lanePositions)
        {
            Vector3 genPos;
            genPos.x = lanePos.x;
            genPos.y = bounceableObject.transform.position.y;
            genPos.z = zPosition;

            CreateBounceableObjectAtPosition(genPos);
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
