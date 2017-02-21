using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCrash : MonoBehaviour {
    
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            coll.gameObject.GetComponent<PlayerController>().Crash();
        }
    }

}
