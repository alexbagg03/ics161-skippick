using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCrash : MonoBehaviour {
    
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
