using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour {

    ///////////////////////////////////////////////
    /// MEMBERS
    ///////////////////////////////////////////////
    public static float lastPlayerBounceZPos = 0f;
    public float driveSpeed = 0.5f;
    public float laneChangeSpeed = 0.25f;
    public float bounceForce = 400f;
    public float testingJumpForce = 400f;
    public bool spacebarBouncing;

    private static int FAR_RIGHT_LANE = 3;
    private static int FAR_LEFT_LANE = 0;
    private GameObject[] lanes;
    private GameObject currentLane;
    private TrafficGenerator trafficGenerator;
    private Vector3 newPosition;
    private Vector3 lanePosition;
    private Vector3 jumpPoint;
    private int currentLaneIndex = -1;
    private bool leftLaneChange = false;
    private bool rightLaneChange = false;
    private bool crashed = false;
    private float startHeight;
    private float previousZ = 0f;

    ///////////////////////////////////////////////
    /// MONOBEHAVIOR METHODS
    ///////////////////////////////////////////////
    void Start ()
    {
        // Find lanes and sort them (Lane1, Lane2, ...)
        lanes = GameObject.FindGameObjectsWithTag("Lane");
        Array.Sort(lanes, CompareObjectNames);

        trafficGenerator = GameObject.Find("TrafficGenerator").GetComponent<TrafficGenerator>();

        startHeight = transform.position.y;

        StartPlayerBounce();
    }
	void Update ()
    {
        if (crashed)
        {
            return;
        }

        if (!leftLaneChange && !rightLaneChange)
        {
            // Detect any lane changes
            if (Input.GetAxis("Horizontal") < 0)
            {
                LeftLaneChange();
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                RightLaneChange();
            }
        }
    }
    void FixedUpdate()
    {
        if (crashed)
        {
            return;
        }

        PlayerMovement();
    }

    ///////////////////////////////////////////////
    /// PUBLIC METHODS
    ///////////////////////////////////////////////
    public void Crash()
    {
        crashed = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
    public void BouncePlayer()
    {
        // Cancel y velocity
        Vector3 vel = GetComponent<Rigidbody>().velocity;
        vel.y = 0;
        GetComponent<Rigidbody>().velocity = vel;

        // Set player y position to a fixed position (for consistent bouncing distance)
        Vector3 pos = transform.position;
        pos.y = startHeight;
        transform.position = pos;

        // Save this as the last bounce z position of the player, then use that to remove old traffic
        lastPlayerBounceZPos = transform.position.z;
        trafficGenerator.RemoveOldTraffic();

        // Apply the bounce force
        GetComponent<Rigidbody>().AddForce(new Vector3(0, bounceForce, 0));
    }

    ///////////////////////////////////////////////
    /// PRIVATE METHODS
    ///////////////////////////////////////////////
    private void PlayerMovement()
    {
        if (currentLane == null)
        {
            lanePosition = transform.position;
        }
        else
        {
            lanePosition = currentLane.transform.position;
        }
        
        // If the player is changing lanes, do the lane change movement
        if (leftLaneChange)
        {
            newPosition = new Vector3(transform.position.x - 0.25f, transform.position.y, transform.position.z + driveSpeed);

            // If the player has reached the chosen lane, movement setup is over
            if (newPosition.x <= lanePosition.x)
            {
                leftLaneChange = false;
            }
        }
        else if (rightLaneChange)
        {
            newPosition = new Vector3(transform.position.x + 0.25f, transform.position.y, transform.position.z + driveSpeed);

            // If the player has reached the chosen lane, movement setup is over
            if (newPosition.x >= lanePosition.x)
            {
                rightLaneChange = false;
            }
        }
        // Otherwise, proceed with standard movement
        else
        {
            newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + driveSpeed);
        }

        // Move to new postion
        GetComponent<Rigidbody>().MovePosition(newPosition);
    }
    private void StartPlayerBounce()
    {
        // Cancel y velocity
        Vector3 vel = GetComponent<Rigidbody>().velocity;
        vel.y = 0;
        GetComponent<Rigidbody>().velocity = vel;

        // Set player y position to a fixed position (for consistent bouncing distance)
        Vector3 pos = transform.position;
        pos.y = startHeight;
        transform.position = pos;

        // Save this as the last bounce z position of the player, then use that to remove old traffic
        lastPlayerBounceZPos = transform.position.z;

        // Apply the bounce force
        GetComponent<Rigidbody>().AddForce(new Vector3(0, bounceForce, 0));
    }
    private void LeftLaneChange()
    {
        if (currentLaneIndex == -1)
        {
            currentLaneIndex = 1;
            leftLaneChange = true;
            currentLane = lanes[currentLaneIndex];
        }
        else if (currentLaneIndex != FAR_LEFT_LANE)
        {
            currentLaneIndex--;
            leftLaneChange = true;
            currentLane = lanes[currentLaneIndex];
        }
    }
    private void RightLaneChange()
    {
        if (currentLaneIndex == -1)
        {
            currentLaneIndex = 2;
            rightLaneChange = true;
            currentLane = lanes[currentLaneIndex];
        }
        else if (currentLaneIndex != FAR_RIGHT_LANE)
        {
            currentLaneIndex++;
            rightLaneChange = true;
            currentLane = lanes[currentLaneIndex];
        }
    }
    private int CompareObjectNames(GameObject x, GameObject y)
    {
        return x.name.CompareTo(y.name);
    }


}
