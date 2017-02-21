using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObstacleTraffic : MonoBehaviour {

    void Start ()
    {
        // Nothing yet
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
                coll.gameObject.GetComponent<PlayerController>().Crash();
                break;
            case "Skipper":
                coll.gameObject.GetComponent<SkipperController>().Crash();
                break;
        }
    }

}
