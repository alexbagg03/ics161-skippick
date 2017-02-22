using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    ///////////////////////////////////////////////
    /// MEMBERS
    ///////////////////////////////////////////////
    // Singleton instance
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public bool gameOver = false;
    public GameObject gameOverUI;

    ///////////////////////////////////////////////
    /// MONOBEHAVIOR METHODS
    ///////////////////////////////////////////////
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    void Update ()
    {
		// Nothing yet
	}

    ///////////////////////////////////////////////
    /// METHODS
    ///////////////////////////////////////////////
    public void GameOver()
    {
        gameOver = true;
        gameOverUI.SetActive(true);
    }

}
