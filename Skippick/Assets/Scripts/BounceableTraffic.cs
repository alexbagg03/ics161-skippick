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
                PlayerController player = other.gameObject.GetComponent<PlayerController>();
                player.BouncePlayer();
                break;
            case "Skipper":
                SkipperController skipper = other.gameObject.GetComponent<SkipperController>();
                skipper.BounceSkipper();

                float boostChance = Random.value * 100;

                if (boostChance <= skipper.boostPercentage)
                {
                    skipper.Boost();
                }
                break;
        }
    }
    void OnTriggerStay(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                PlayerController player = other.gameObject.GetComponent<PlayerController>();
                if (Input.GetKeyDown(bounceKey) || Input.GetKeyUp(bounceKey))
                {
                    player.Boost();
                }
                break;
        }
    }

}
