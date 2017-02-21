using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TrafficController : MonoBehaviour {

    ///////////////////////////////////////////////
    /// MEMBERS
    ///////////////////////////////////////////////
    public static float lastPlayerBounceZPos = 0f;
    public float bounceForce = 400f;
    
    private TrafficGenerator trafficGenerator;

    ///////////////////////////////////////////////
    /// MONOBEHAVIOR METHODS
    ///////////////////////////////////////////////
    void Start ()
    {
        trafficGenerator = GameObject.Find("TrafficGenerator").GetComponent<TrafficGenerator>();
    }
	void Update ()
    {
        // Nothing yet
	}
    void OnCollisionEnter(Collision coll)
    {
        switch (coll.gameObject.tag)
        {
            case "Player":
                BouncePlayer(coll.gameObject);
                break;
            case "Skipper":
                BounceSkipper(coll.gameObject);
                break;
        }
    }

    ///////////////////////////////////////////////
    /// PRIVATE METHODS
    ///////////////////////////////////////////////
    private void BouncePlayer(GameObject player)
    {
        float startHeight = player.GetComponent<PlayerController>().startHeight;

        // Cancel y velocity
        Vector3 vel = player.GetComponent<Rigidbody>().velocity;
        vel.y = 0;
        player.GetComponent<Rigidbody>().velocity = vel;

        // Set player y position to a fixed position (for consistent bouncing distance)
        Vector3 pos = player.transform.position;
        pos.y = startHeight;
        player.transform.position = pos;

        // Save this as the last bounce z position of the player, then use that to remove old traffic
        lastPlayerBounceZPos = player.transform.position.z;
        trafficGenerator.RemoveOldTraffic();

        // Apply the bounce force
        player.GetComponent<Rigidbody>().AddForce(new Vector3(0, bounceForce, 0));
    }
    private void BounceSkipper(GameObject skipper)
    {
        float startHeight = skipper.GetComponent<SkipperController>().startHeight;

        // Cancel y velocity
        Vector3 vel = skipper.GetComponent<Rigidbody>().velocity;
        vel.y = 0;
        skipper.GetComponent<Rigidbody>().velocity = vel;

        // Set skipper y position to a fixed position (for consistent bouncing distance)
        Vector3 pos = skipper.transform.position;
        pos.y = startHeight;
        skipper.transform.position = pos;

        // Apply the bounce force
        skipper.GetComponent<Rigidbody>().AddForce(new Vector3(0, bounceForce, 0));

        skipper.GetComponent<SkipperController>().RandomLaneChange();
    }

}
