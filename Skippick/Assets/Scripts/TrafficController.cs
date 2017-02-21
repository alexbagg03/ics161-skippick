using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TrafficController : MonoBehaviour {

    public static float lastPlayerBounceZPos = 0f;
    public float bounceForce = 400f;
    
    private TrafficGenerator trafficGenerator;

	void Start ()
    {
        trafficGenerator = GameObject.Find("TrafficGenerator").GetComponent<TrafficGenerator>();
    }
	void Update ()
    {

	}
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            GameObject player = coll.gameObject;
            float startHeight = player.GetComponent<PlayerController>().startHeight;

            Vector3 vel = player.GetComponent<Rigidbody>().velocity;
            vel.y = 0;
            player.GetComponent<Rigidbody>().velocity = vel;

            Vector3 pos = player.transform.position;
            pos.y = startHeight;
            player.transform.position = pos;

            lastPlayerBounceZPos = player.transform.position.z;
            trafficGenerator.RemoveOldTraffic();

            player.GetComponent<Rigidbody>().AddForce(new Vector3(0, bounceForce, 0));
        }
    }

}
