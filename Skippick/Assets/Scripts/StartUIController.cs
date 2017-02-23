using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUIController : MonoBehaviour {
    
	void Start ()
    {
        GameManager.Instance.PauseGame();
	}
	void Update ()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            GameManager.Instance.ContinueGame();
            HidePanel();
        }
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }

}
