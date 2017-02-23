using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceableTraffic : MonoBehaviour
{

    public KeyCode bounceKey;

    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                PlayerController player = other.gameObject.GetComponent<PlayerController>();

                if (player.boosted)
                {
                    player.ResetSpeed();
                }

                player.BouncePlayer();
                break;
            case "Skipper":
                SkipperController skipper = other.gameObject.GetComponent<SkipperController>();

                if (skipper.boosted)
                {
                    skipper.ResetSpeed();
                }

                float boostChance = Random.value * 100;

                if (boostChance <= skipper.boostPercentage)
                {
                    skipper.Boost();
                }

                skipper.BounceSkipper();
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