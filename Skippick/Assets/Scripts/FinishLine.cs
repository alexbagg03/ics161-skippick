using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour {

    GameObject player;

	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");	
	}
	void Update ()
    {
        if (player.transform.position.z >= transform.position.z)
        {
            GameManager.Instance.GameOver();
        }	
	}
}
