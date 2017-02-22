using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BounceableTraffic : MonoBehaviour {

    public KeyCode bounceKey;

    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

                if (!playerController.controlBouncing)
                {
                    playerController.BouncePlayer();
                }
                break;
            case "Skipper":
                other.gameObject.GetComponent<SkipperController>().BounceSkipper();
                break;
        }
    }
    void OnTriggerStay(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

                if (playerController.controlBouncing)
                {
                    if (Input.GetKeyDown(bounceKey) || Input.GetKeyUp(bounceKey))
                    {
                        playerController.BouncePlayer();
                    }
                }
                break;
        }
    }

}
