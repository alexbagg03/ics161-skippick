using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour {

    ///////////////////////////////////////////////
    /// MEMBERS
    ///////////////////////////////////////////////
    public float driveSpeed = 0.5f;
    public float laneChangeSpeed = 0.25f;
    public float jumpForce = 100f;

    private int FAR_RIGHT_LANE = 3;
    private int FAR_LEFT_LANE = 0;
    private GameObject[] lanes;
    private GameObject currentLane;
    private Vector3 newPosition;
    private Vector3 lanePosition;
    private Vector3 jumpPoint;
    private int currentLaneIndex = -1;
    private float startHeight;
    private bool leftLaneChange = false;
    private bool rightLaneChange = false;
    private bool crashed = false;

    ///////////////////////////////////////////////
    /// MONOBEHAVIOR METHODS
    ///////////////////////////////////////////////
    void Start ()
    {
        // Find lanes and sort them (Lane1, Lane2, ...)
        lanes = GameObject.FindGameObjectsWithTag("Lane");
        Array.Sort(lanes, CompareObjectNames);

        startHeight = transform.position.y;

        //StartBounce();
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
        BounceTesting();
    }

    ///////////////////////////////////////////////
    /// PUBLIC METHODS
    ///////////////////////////////////////////////
    public void Crash()
    {
        crashed = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
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
    private void StartBounce()
    {
        Vector3 vel = GetComponent<Rigidbody>().velocity;
        vel.y = 0;
        GetComponent<Rigidbody>().velocity = vel;
        GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpForce, 0));
    }
    private void BounceTesting()
    {
        if (transform.position.y <= startHeight)
        {
            Vector3 vel = GetComponent<Rigidbody>().velocity;
            vel.y = 0;
            GetComponent<Rigidbody>().velocity = vel;
            GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpForce, 0));
        }
    }
    private void LeftLaneChange()
    {
        if (currentLaneIndex == -1)
        {
            currentLaneIndex = 1;
        }
        else if (currentLaneIndex != FAR_LEFT_LANE)
        {
            currentLaneIndex--;
        }

        leftLaneChange = true;
        currentLane = lanes[currentLaneIndex];
    }
    private void RightLaneChange()
    {
        if (currentLaneIndex == -1)
        {
            currentLaneIndex = 2;
        }
        else if (currentLaneIndex != FAR_RIGHT_LANE)
        {
            currentLaneIndex++;
        }

        rightLaneChange = true;
        currentLane = lanes[currentLaneIndex];
    }
    private int CompareObjectNames(GameObject x, GameObject y)
    {
        return x.name.CompareTo(y.name);
    }


}
