using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkipperController : MonoBehaviour {

    ///////////////////////////////////////////////
    /// MEMBERS
    ///////////////////////////////////////////////
    public int skipperNumber = 0;
    public float driveSpeed = 0.5f;
    public float laneChangeSpeed = 0.25f;
    public float bounceForce = 400f;

    private int FAR_RIGHT_LANE = 3;
    private int FAR_LEFT_LANE = 0;
    private GameObject[] lanes;
    private GameObject currentLane;
    private Vector3 newPosition;
    private Vector3 lanePosition;
    private int currentLaneIndex = -1;
    private int bestLaneIndex = -2;
    private bool leftLaneChange = false;
    private bool rightLaneChange = false;
    private bool crashed = false;
    private float startHeight;

    ///////////////////////////////////////////////
    /// MONOBEHAVIOR METHODS
    ///////////////////////////////////////////////
    void Start ()
    {
        // Find lanes and sort them (Lane1, Lane2, ...)
        lanes = GameObject.FindGameObjectsWithTag("Lane");
        Array.Sort(lanes, CompareObjectNames);

        startHeight = transform.position.y;

        // If the number wasn't set in the editor, try to grab it from the gameObject name
        if (skipperNumber == 0)
        {
            skipperNumber = int.Parse("" + name[name.Length - 1]);
        }
        
        BounceSkipper();
    }
	void Update ()
    {
        if (GameManager.Instance.gameOver)
        {
            return;
        }
        if (crashed)
        {
            return;
        }

        ChangeToNextBestLane();
    }
    void FixedUpdate()
    {
        if (GameManager.Instance.gameOver)
        {
            return;
        }
        if (crashed)
        {
            return;
        }

        SkipperMovement();
    }

    ///////////////////////////////////////////////
    /// PUBLIC METHODS
    ///////////////////////////////////////////////
    public void Crash()
    {
        crashed = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
    public void BounceSkipper()
    {
        // Cancel y velocity
        Vector3 vel = GetComponent<Rigidbody>().velocity;
        vel.y = 0;
        GetComponent<Rigidbody>().velocity = vel;

        // Set player y position to a fixed position (for consistent bouncing distance)
        Vector3 pos = transform.position;
        pos.y = startHeight;
        transform.position = pos;

        // Apply the bounce force
        GetComponent<Rigidbody>().AddForce(new Vector3(0, bounceForce, 0));
    }

    ///////////////////////////////////////////////
    /// PRIVATE METHODS
    ///////////////////////////////////////////////
    private void ChangeToNextBestLane()
    {
        if (TrafficGenerator.nextBounceableObjectPositions != null)
        {
            float minDistanceAway = 1000; // Set to some large number
            Vector3 bestObjPos = Vector3.zero;

            foreach (Vector3 objPos in TrafficGenerator.nextBounceableObjectPositions)
            {
                float distanceAway = Mathf.Abs(objPos.x) - Mathf.Abs(transform.position.x);

                if (distanceAway < minDistanceAway)
                {
                    minDistanceAway = distanceAway;
                    bestObjPos = objPos;
                }
            }

            for (int i = 0; i < lanes.Length; i++)
            {
                GameObject lane = lanes[i];

                if (lane.transform.position.x == bestObjPos.x)
                {
                    bestLaneIndex = i;
                    break;
                }
            }

            if (bestLaneIndex != -2)
            {
                if (currentLaneIndex < bestLaneIndex)
                {
                    while (currentLaneIndex < bestLaneIndex)
                    {
                        RightLaneChange();
                    }
                }
                else if (currentLaneIndex > bestLaneIndex)
                {
                    while (currentLaneIndex > bestLaneIndex)
                    {
                        LeftLaneChange();
                    }
                }
            }
        }
    }
    private void SkipperMovement()
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
    private void LeftLaneChange()
    {
        if (currentLaneIndex != FAR_LEFT_LANE)
        {
            currentLaneIndex--;
            leftLaneChange = true;
            currentLane = lanes[currentLaneIndex];
        }
    }
    private void RightLaneChange()
    {
        if (currentLaneIndex != FAR_RIGHT_LANE)
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
