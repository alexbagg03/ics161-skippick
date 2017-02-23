using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour {

    ///////////////////////////////////////////////
    /// MEMBERS
    ///////////////////////////////////////////////
    public Transform player;
    public Transform skipper1;
    public Transform skipper2;
    public PlayerStats playerStats;
    public GameObject roadSection;
    public GameObject finishLineSection;
    public int raceLength = 10;

    public static float ROAD_DISTANCE = 350;
    private List<GameObject> roads;
    private GameObject currentRoad;
    private TrafficGenerator trafficGenerator;
    private string originalName;
    private int roadNumber = 0;

    ///////////////////////////////////////////////
    /// MONOBEHAVIOR METHODS
    ///////////////////////////////////////////////
    void Awake ()
    {
        originalName = roadSection.name;

        roads = new List<GameObject>();
        currentRoad = roadSection;
        roads.Add(currentRoad);

        trafficGenerator = GameObject.Find("TrafficGenerator").GetComponent<TrafficGenerator>();

        for (int i = 0; i < raceLength; i++)
        {
            GenerateNewRoad();
            trafficGenerator.GenerateTraffic();
        }

        GenerateFinishLine();
        trafficGenerator.GenerateTraffic();
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
    private void GenerateFinishLine()
    {
        float currZ = currentRoad.transform.position.z;

        GameObject finishLine = GameObject.Instantiate(finishLineSection);
        Vector3 position = finishLineSection.transform.position;

        position.z = currZ + ROAD_DISTANCE;
        finishLine.transform.position = position;

        // Generate another road section after the finish line (just for aesthetics)
        GameObject nextRoad = GameObject.Instantiate(currentRoad);
        Vector3 nextRoadPos = finishLine.transform.position;
        nextRoadPos.y = currentRoad.transform.position.y;
        nextRoadPos.z += ROAD_DISTANCE;
        nextRoad.transform.position = nextRoadPos;
        roads.Add(nextRoad);
    }

}