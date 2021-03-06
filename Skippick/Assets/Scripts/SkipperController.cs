﻿using System.Collections;
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
    public float bounceForce = 500;
    public float resetTime = 1f;
    public float variabilityPercentage = 70;
    public float errorPercentage = 40;
    public float boostPercentage = 30;
    public bool finished = false;
    public bool boosted = false;

    private static int FAR_RIGHT_LANE = 3;
    private static int FAR_LEFT_LANE = 0;
    private static int MAX_FAILED_ATEMPTS = 2;
    private GameObject[] lanes;
    private GameObject currentLane;
    private TrafficGenerator trafficGenerator;
    private Vector3 newPosition;
    private Vector3 lanePosition;
    private Vector3 resetPosition;
    private int currentLaneIndex = -1;
    private int previousLaneIndex = -1;
    private int bestLaneIndex = -1;
    private int nextBounceIndex = 0;
    private int previousBounceIndex = 0;
    private int failedAttemps = 0;
    private bool leftLaneChange = false;
    private bool rightLaneChange = false;
    private bool resetting = false;
    private bool difficultySet;
    private float resetTimer;
    private float startHeight;
    private float lastBounceZPos = 0f;
    private float initialDriveSpeed;
    private float boostSpeed;

    ///////////////////////////////////////////////
    /// MONOBEHAVIOR METHODS
    ///////////////////////////////////////////////
    void Start ()
    {
        // Find lanes and sort them (Lane1, Lane2, ...)
        lanes = GameObject.FindGameObjectsWithTag("Lane");
        Array.Sort(lanes, CompareObjectNames);

        // If the number wasn't set in the editor, try to grab it from the gameObject name
        if (skipperNumber == 0)
        {
            skipperNumber = int.Parse("" + name[name.Length - 1]);
        }

        trafficGenerator = GameObject.Find("TrafficGenerator").GetComponent<TrafficGenerator>();
        startHeight = transform.position.y;
        resetPosition = transform.position;
        initialDriveSpeed = driveSpeed;
        boostSpeed = driveSpeed * 2;

        InitializeCurrentLaneIndex();
        BounceSkipper();
    }
	void Update ()
    {
        if (!difficultySet)
        {
            SetDifficulty();
        }
        if (finished)
        {
            return;
        }
        if (resetting)
        {
            resetTimer += Time.deltaTime;
            return;
        }
    }
    void FixedUpdate()
    {
        if (finished)
        {
            return;
        }
        if (resetting)
        {
            return;
        }

        SkipperMovement();
    }

    ///////////////////////////////////////////////
    /// PUBLIC METHODS
    ///////////////////////////////////////////////
    public void ResetSkipper()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        resetTimer = 0;
        failedAttemps++;
        ResetCurrentLaneIndex();
        StartCoroutine("BeginResettingSkipper", resetTime);
        StartCoroutine("Blink", resetTime);
    }
    public void Finish()
    {
        if (finished)
        {
            return;
        }

        finished = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.useGravity = false;

        GetComponent<Animator>().enabled = false;

        GameManager.Instance.SetStandingOfSkipper(gameObject.name);
    }
    public void BounceSkipper()
    {
        // Cancel y velocity
        Vector3 vel = GetComponent<Rigidbody>().velocity;
        vel.y = 0;
        GetComponent<Rigidbody>().velocity = vel;

        Vector3 pos = transform.position;
        pos.y = startHeight;
        transform.position = pos;

        GetComponent<Animator>().Play("Jumping");

        // Apply the bounce force
        GetComponent<Rigidbody>().AddForce(new Vector3(0, bounceForce, 0));

        if (resetting)
        {
            nextBounceIndex = previousBounceIndex;
        }
        else
        {
            failedAttemps = 0;
        }

        ChangeToNextBestLane();
        lastBounceZPos = transform.position.z;
        previousBounceIndex = nextBounceIndex;
        nextBounceIndex++;
    }
    public void Boost()
    {
        previousBounceIndex = nextBounceIndex;
        nextBounceIndex++;
        driveSpeed = boostSpeed;
        boosted = true;
    }
    public void ResetSpeed()
    {
        driveSpeed = initialDriveSpeed;
        boosted = false;
    }

    ///////////////////////////////////////////////
    /// PRIVATE METHODS
    ///////////////////////////////////////////////
    private void ChangeToNextBestLane()
    {
        if (nextBounceIndex > trafficGenerator.GetListCount() - 1)
        {
            return;
        }

        List<GameObject> bounceableObjects = trafficGenerator.GetBounceableObjectsAtIndex(nextBounceIndex);

        if (bounceableObjects != null)
        {
            float minDistanceAway = 1000; // Set to some large number
            Vector3 bestObjPos = Vector3.zero;

            while (bestObjPos == Vector3.zero)
            {
                foreach (GameObject obj in bounceableObjects)
                {
                    float distanceAway = Mathf.Abs(obj.transform.position.x) - Mathf.Abs(transform.position.x);

                    if (distanceAway < minDistanceAway)
                    {
                        // Add randomness/variability to which of the best positions
                        // is chosen by the Skipper AI
                        float variabilityChance = UnityEngine.Random.value * 100;

                        if (variabilityChance <= variabilityPercentage)
                        {
                            minDistanceAway = distanceAway;
                            bestObjPos = obj.transform.position;
                        }
                    }
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

            if (failedAttemps < MAX_FAILED_ATEMPTS)
            {
                // Add random chance of error
                float errorChance = UnityEngine.Random.value * 100;

                if (errorChance <= errorPercentage)
                {
                    bestLaneIndex = UnityEngine.Random.Range(0, 3);

                    while (bestLaneIndex == currentLaneIndex)
                    {
                        bestLaneIndex = UnityEngine.Random.Range(0, 3);
                    }
                }
            }

            if (bestLaneIndex != -1)
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
        
        // If the skipper is changing lanes, do the lane change movement
        if (leftLaneChange)
        {
            newPosition = new Vector3(transform.position.x - laneChangeSpeed, transform.position.y, transform.position.z + driveSpeed);

            if (skipperNumber == 1)
            {
                if (newPosition.x <= lanePosition.x - 0.75f)
                {
                    leftLaneChange = false;
                }
            }
            else if (skipperNumber == 2)
            {
                if (newPosition.x <= lanePosition.x + 0.75f)
                {
                    leftLaneChange = false;
                }
            }
        }
        else if (rightLaneChange)
        {
            newPosition = new Vector3(transform.position.x + laneChangeSpeed, transform.position.y, transform.position.z + driveSpeed);

            if (skipperNumber == 1)
            {
                if (newPosition.x >= lanePosition.x - 0.75f)
                {
                    rightLaneChange = false;
                }
            }
            else if (skipperNumber == 2)
            {
                if (newPosition.x >= lanePosition.x + 0.75f)
                {
                    rightLaneChange = false;
                }
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
            previousLaneIndex = currentLaneIndex;
            currentLaneIndex--;
            leftLaneChange = true;
            currentLane = lanes[currentLaneIndex];
        }
    }
    private void RightLaneChange()
    {
        if (currentLaneIndex != FAR_RIGHT_LANE)
        {
            previousLaneIndex = currentLaneIndex;
            currentLaneIndex++;
            rightLaneChange = true;
            currentLane = lanes[currentLaneIndex];
        }
    }
    private void InitializeCurrentLaneIndex()
    {
        // Set the current lane index to be out of bounds initially, this
        // will enable ChangeToNextBestLane() to function properly
        switch (skipperNumber)
        {
            case 1:
                currentLaneIndex = FAR_LEFT_LANE - 1;
                break;
            case 2:
                currentLaneIndex = FAR_RIGHT_LANE + 1;
                break;
        }
    }
    private void ResetCurrentLaneIndex()
    {
        if (currentLaneIndex == 0 || currentLaneIndex == 1)
        {
            currentLaneIndex = FAR_LEFT_LANE - 1;
        }
        else if (currentLaneIndex == 2 || currentLaneIndex == 3)
        {
            currentLaneIndex = FAR_LEFT_LANE + 1;
        }
        else
        {
            currentLaneIndex = FAR_LEFT_LANE - 1;
        }
    }
    private void SetDifficulty()
    {
        switch (GameManager.Instance.AIDifficulty)
        {
            case GameManager.AI_DIFFICULTY.EASY:
                errorPercentage = 40;
                boostPercentage = 10;
                difficultySet = true;
                break;
            case GameManager.AI_DIFFICULTY.MEDIUM:
                errorPercentage = 20;
                boostPercentage = 30;
                difficultySet = true;
                break;
            case GameManager.AI_DIFFICULTY.HARD:
                errorPercentage = 0;
                boostPercentage = 60;
                difficultySet = true;
                break;
            case GameManager.AI_DIFFICULTY.NOT_SET:
                break;
        }
    }
    private int CompareObjectNames(GameObject x, GameObject y)
    {
        return x.name.CompareTo(y.name);
    }

    ///////////////////////////////////////////////
    /// COROUTINES
    ///////////////////////////////////////////////
    private IEnumerator BeginResettingSkipper()
    {
        resetPosition.z = lastBounceZPos;
        transform.position = resetPosition;
        currentLaneIndex = previousLaneIndex;
        GetComponent<Rigidbody>().useGravity = false;
        resetting = true;

        yield return new WaitForSeconds(resetTime);

        GetComponent<Rigidbody>().useGravity = true;
        BounceSkipper();
        resetting = false; // Needs to be at the end of the function
    }
    private IEnumerator Blink(float blinkTime)
    {
        while (resetTimer < blinkTime)
        {
            Renderer renderer = GetComponentInChildren<Renderer>();
            renderer.enabled = false;
            yield return new WaitForSeconds(0.2f);
            renderer.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
    }


}
