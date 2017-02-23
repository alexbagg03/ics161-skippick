using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BounceableTraffic : MonoBehaviour {

    public KeyCode bounceKey;

    private PlayerController player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    void FixedUpdate()
    {
        //Vector3 newPostion = transform.position;
        //newPostion.z += player.driveSpeed / 2;
        //GetComponent<Rigidbody>().MovePosition(newPostion);
    }
    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
                playerController.BouncePlayer();
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

                // Possibly going to be used for a special move instead
                //if (Input.GetKeyDown(bounceKey) || Input.GetKeyUp(bounceKey))
                //{
                //    playerController.BouncePlayer();
                //}
                break;
        }
    }

}
