using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    ///////////////////////////////////////////////
    /// MEMBERS
    ///////////////////////////////////////////////
    // Singleton instance
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public enum STANDING
    {
        FIRST_PLACE,
        SECOND_PLACE,
        THIRD_PLACE,
        TIE
    }
    public enum AI_DIFFICULTY
    {
        EASY,
        MEDIUM,
        HARD,
        NOT_SET
    }

    public bool gameOver = false;
    public bool gamePaused = false;
    public AI_DIFFICULTY AIDifficulty;

    private static string NONE = "none";
    private Dictionary<STANDING, string> standings = new Dictionary<STANDING, string>();

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
        
        AIDifficulty = AI_DIFFICULTY.NOT_SET;
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
        standings[STANDING.TIE] = "Player"; 
    }
    public void SetStandingOfSkipper(string name)
    {
        if (standings[STANDING.FIRST_PLACE] == NONE)
        {
            standings[STANDING.FIRST_PLACE] = name;
        }
        else if (standings[STANDING.SECOND_PLACE] == NONE)
        {
            standings[STANDING.SECOND_PLACE] = name;
        }
        else if (standings[STANDING.THIRD_PLACE] == NONE)
        {
            standings[STANDING.THIRD_PLACE] = name;
        }
    }
    public STANDING GetPlayerStanding()
    {
        STANDING playerStanding = STANDING.TIE;

        foreach (STANDING place in standings.Keys)
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
        gamePaused = true;
    }
    public void ContinueGame()
    {
        Time.timeScale = 1;
        gamePaused = false;
    }
    public void SetAIDifficulty(AI_DIFFICULTY difficulty)
    {
        AIDifficulty = difficulty;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    ///////////////////////////////////////////////
    /// PRIVATE METHODS
    ///////////////////////////////////////////////
    private void InitializeStandings()
    {
        standings[STANDING.FIRST_PLACE] = NONE;
        standings[STANDING.SECOND_PLACE] = NONE;
        standings[STANDING.THIRD_PLACE] = NONE;
    }

}
