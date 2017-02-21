using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour {

    public Transform player;
    public GameObject roadSection;

    private List<GameObject> roads;
    private GameObject currentRoad;
    private string originalName;
    private int roadNumber = 0;

    void Start ()
    {
        originalName = roadSection.name;

        roads = new List<GameObject>();
        currentRoad = roadSection;
        roads.Add(currentRoad);
    }
	void Update ()
    {
        if (player.position.z >= currentRoad.transform.position.z)
        {
            float currX = currentRoad.transform.position.x;
            float currY = currentRoad.transform.position.y;
            float currZ = currentRoad.transform.position.z;

            roadNumber++;

            GameObject newRoad = GameObject.Instantiate(currentRoad);
            newRoad.transform.position = new Vector3(currX, currY, currZ + 350);
            newRoad.name = originalName + "(" + roadNumber + ")";

            currentRoad = newRoad;
            roads.Add(currentRoad);
        }

        GameObject previousRoad = roads[0];
        if (player.position.z >= previousRoad.transform.position.z + 350)
        {
            roads.Remove(previousRoad);
            Destroy(previousRoad);
        }
    }
}
