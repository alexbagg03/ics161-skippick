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

    public enum STANDINGS
    {
        FIRST_PLACE,
        SECOND_PLACE,
        THIRD_PLACE,
        TIE
    }

    public bool gameOver = false;

    private static string NONE = "none";
    private Dictionary<STANDINGS, string> standings = new Dictionary<STANDINGS, string>();

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

        InitializeStandings();
    }
    void Update ()
    {
		// Nothing yet
	}

    ///////////////////////////////////////////////
    /// PUBLIC METHODS
    ///////////////////////////////////////////////
    public void GameOver()
    {
        gameOver = true;
        GameOverUIController.Instance.ShowPlayerStandingText();
    }
    public void TieGame()
    {
        standings[STANDINGS.TIE] = "Player"; 
    }
    public void SetStandingOfSkipper(string name)
    {
        if (standings[STANDINGS.FIRST_PLACE] == NONE)
        {
            standings[STANDINGS.FIRST_PLACE] = name;
        }
        else if (standings[STANDINGS.SECOND_PLACE] == NONE)
        {
            standings[STANDINGS.SECOND_PLACE] = name;
        }
        else if (standings[STANDINGS.THIRD_PLACE] == NONE)
        {
            standings[STANDINGS.THIRD_PLACE] = name;
        }
    }
    public STANDINGS GetPlayerStanding()
    {
        STANDINGS playerStanding = STANDINGS.TIE;

        foreach (STANDINGS place in standings.Keys)
        {
            if (standings[place] == "Player")
            {
                playerStanding = place;
            }
        }

        return playerStanding;
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ContinueGame()
    {
        Time.timeScale = 1;
    }

    ///////////////////////////////////////////////
    /// PRIVATE METHODS
    ///////////////////////////////////////////////
    private void InitializeStandings()
    {
        standings[STANDINGS.FIRST_PLACE] = NONE;
        standings[STANDINGS.SECOND_PLACE] = NONE;
        standings[STANDINGS.THIRD_PLACE] = NONE;
    }

}
