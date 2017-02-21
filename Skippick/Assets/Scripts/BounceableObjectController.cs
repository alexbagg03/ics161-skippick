using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BounceableObjectController : MonoBehaviour {

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
                coll.gameObject.GetComponent<PlayerController>().BouncePlayer();
                break;
            case "Skipper":
                coll.gameObject.GetComponent<SkipperController>().BounceSkipper();
                break;
        }
    }

}
