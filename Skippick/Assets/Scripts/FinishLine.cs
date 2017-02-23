using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour {

    public GameObject player;
    public GameObject skipper1;
    public GameObject skipper2;

	void Start ()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (skipper1 == null)
        {
            skipper1 = GameObject.Find("Skipper1");
        }
        if (skipper2 == null)
        {
            skipper2 = GameObject.Find("Skipper2");
        }
    }
	void Update ()
    {
        bool playerCrossed = player.transform.position.z >= transform.position.z;
        bool skipper1Crossed = skipper1.transform.position.z >= transform.position.z;
        bool skipper2Crossed = skipper2.transform.position.z >= transform.position.z;

        if (playerCrossed && skipper1Crossed && skipper2Crossed)
        {
            player.GetComponent<PlayerController>().TieFinish();
            skipper1.GetComponent<SkipperController>().Finish();
            skipper2.GetComponent<SkipperController>().Finish();
        }
        else if (playerCrossed && skipper1Crossed)
        {
            player.GetComponent<PlayerController>().TieFinish();
            skipper1.GetComponent<SkipperController>().Finish();
        }
        else if(playerCrossed && skipper2Crossed)
        {
            player.GetComponent<PlayerController>().TieFinish();
            skipper2.GetComponent<SkipperController>().Finish();
        }
        else if (playerCrossed)
        {
            player.GetComponent<PlayerController>().Finish();
        }
        else if (skipper1Crossed)
        {
            skipper1.GetComponent<SkipperController>().Finish();
        }
        else if (skipper2Crossed)
        {
            skipper2.GetComponent<SkipperController>().Finish();
        }
    }
}
