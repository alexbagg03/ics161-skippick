using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficController : MonoBehaviour {

    public float bounceForce = 100f;

	void Start ()
    {
		
	}
	void Update ()
    {
		
	}
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Rigidbody playerRigidbody = coll.gameObject.GetComponent<Rigidbody>();
            Vector3 vel = playerRigidbody.velocity;
            vel.y = 0;
            playerRigidbody.velocity = vel;
            playerRigidbody.AddForce(new Vector3(0, bounceForce, 0));
        }
    }

}
