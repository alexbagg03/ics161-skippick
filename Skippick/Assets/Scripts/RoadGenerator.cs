using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour {

    ///////////////////////////////////////////////
    /// MEMBERS
    ///////////////////////////////////////////////
    public Transform player;
    public PlayerStats playerStats;
    public GameObject roadSection;

    public static float ROAD_DISTANCE = 350;
    private List<GameObject> roads;
    private GameObject currentRoad;
    private GameObject previousRoad;
    private TrafficGenerator trafficGenerator;
    private string originalName;
    private int roadNumber = 0;

    ///////////////////////////////////////////////
    /// MONOBEHAVIOR METHODS
    ///////////////////////////////////////////////
    void Start ()
    {
        originalName = roadSection.name;

        roads = new List<GameObject>();
        currentRoad = roadSection;
        roads.Add(currentRoad);

        trafficGenerator = GameObject.Find("TrafficGenerator").GetComponent<TrafficGenerator>();
    }
	void Update ()
    {
        if (player.position.z >= currentRoad.transform.position.z)
        {
            GenerateNewRoad();
            trafficGenerator.GenerateTraffic();
        }

        // Remove old road that is too far behind
        previousRoad = roads[0];
        if (player.position.z >= previousRoad.transform.position.z + ROAD_DISTANCE + playerStats.bounceDistance)
        {
            RemoveOldRoad();
        }
    }

    ///////////////////////////////////////////////
    /// PRIVATE METHODS
    ///////////////////////////////////////////////
    private void GenerateNewRoad()
    {
        float currX = currentRoad.transform.position.x;
        float currY = currentRoad.transform.position.y;
        float currZ = currentRoad.transform.position.z;

        roadNumber++;

        GameObject newRoad = GameObject.Instantiate(currentRoad);
        newRoad.transform.position = new Vector3(currX, currY, currZ + ROAD_DISTANCE);
        newRoad.name = originalName + "(" + roadNumber + ")";
        roadSection = newRoad;

        currentRoad = newRoad;
        roads.Add(currentRoad);
    }
    private void RemoveOldRoad()
    {
        roads.Remove(previousRoad);
        Destroy(previousRoad);
    }

}