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
    public float bounceForce = 400f;
    public float resetTime = 1f;
    public bool finished = false;
    public bool boosted = false;

    private static int FAR_RIGHT_LANE = 3;
    private static int FAR_LEFT_LANE = 0;
    private GameObject[] lanes;
    private GameObject currentLane;
    private TrafficGenerator trafficGenerator;
    private Vector3 newPosition;
    private Vector3 lanePosition;
    private Vector3 resetPosition;
    private int currentLaneIndex = -1;
    private bool leftLaneChange = false;
    private bool rightLaneChange = false;
    private bool resetting = false;
    private float startHeight;
    private float resetTimer;
    private float previousZ;
    private float lastBounceZPos = 0f;
    private enum MOVE_STATE
    {
        LEFT,
        RIGHT,
        STILL
    }
    private MOVE_STATE currentMoveState;

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
        resetPosition = transform.position;

        BouncePlayer();
    }
	void Update ()
    {
        if (finished)
        {
            return;
        }
        if (resetting)
        {
            resetTimer += Time.deltaTime;
            return;
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            currentMoveState = MOVE_STATE.LEFT;
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            currentMoveState = MOVE_STATE.RIGHT;
        }
        else
        {
            currentMoveState = MOVE_STATE.STILL;
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

        PlayerMovement();
    }

    ///////////////////////////////////////////////
    /// PUBLIC METHODS
    ///////////////////////////////////////////////
    public void ResetPlayer()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        resetTimer = 0;
        StartCoroutine("BeginResettingPlayer", resetTime);
        StartCoroutine("Blink", resetTime);
    }
    public void BouncePlayer()
    {
        float distance = transform.position.z - previousZ;
        print("Bounce Distance = " + distance);
        previousZ = transform.position.z;

        // Cancel y velocity
        Vector3 vel = GetComponent<Rigidbody>().velocity;
        vel.y = 0;
        GetComponent<Rigidbody>().velocity = vel;

        // Set player y position to a fixed position (for consistent bouncing distance)
        Vector3 pos = transform.position;
        pos.y = startHeight;
        transform.position = pos;

        // Save this as the last bounce z position of the player, then use that to remove old traffic
        lastBounceZPos = transform.position.z;

        if (boosted)
        {
            driveSpeed /= 2;
            boosted = false;
        }

        GetComponent<Animator>().Play("Jumping");

        // Apply the bounce force
        GetComponent<Rigidbody>().AddForce(new Vector3(0, bounceForce, 0));
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
        GameManager.Instance.GameOver();
    }
    public void TieFinish()
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

        GameManager.Instance.TieGame();
        GameManager.Instance.GameOver();
    }
    public void Boost()
    {
        driveSpeed *= 2;
        boosted = true;
    }

    ///////////////////////////////////////////////
    /// PRIVATE METHODS
    ///////////////////////////////////////////////
    private void PlayerMovement()
    {
        newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + driveSpeed);

        if (currentMoveState == MOVE_STATE.LEFT)
        {
            if (newPosition.x >= lanes[0].transform.position.x)
            {
                newPosition.x -= Time.deltaTime * laneChangeSpeed * 100;
            }
        }
        else if (currentMoveState == MOVE_STATE.RIGHT)
        {
            if (newPosition.x <= lanes[3].transform.position.x)
            {
                newPosition.x += Time.deltaTime * laneChangeSpeed * 100;
            }
        }

        // Move to new postion
        GetComponent<Rigidbody>().MovePosition(newPosition);
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

    ///////////////////////////////////////////////
    /// COROUTINES
    ///////////////////////////////////////////////
    private IEnumerator BeginResettingPlayer()
    {
        resetPosition.z = lastBounceZPos;
        transform.position = resetPosition;
        GetComponent<Rigidbody>().useGravity = false;
        currentLaneIndex = -1;
        resetting = true;

        yield return new WaitForSeconds(resetTime);

        GetComponent<Rigidbody>().useGravity = true;
        resetting = false;
        BouncePlayer();
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
