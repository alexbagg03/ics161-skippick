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
        if (player.transform.position.z >= transform.position.z)
        {
            GameManager.Instance.GameOver();
        }
        if (skipper1.transform.position.z >= transform.position.z)
        {
            skipper1.GetComponent<SkipperController>().Finish();
        }
        if (skipper2.transform.position.z >= transform.position.z)
        {
            skipper2.GetComponent<SkipperController>().Finish();
        }
    }
}
