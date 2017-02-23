using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCrash : MonoBehaviour {
    
    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                other.gameObject.GetComponent<PlayerController>().ResetPlayer();
                break;
            case "Skipper":
                other.gameObject.GetComponent<SkipperController>().ResetSkipper();
                break;
        }
    }

}
