using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUIController : MonoBehaviour {

    ///////////////////////////////////////////////
    /// MEMBERS
    ///////////////////////////////////////////////
    // Singleton instance
    private static GameOverUIController _instance;
    public static GameOverUIController Instance { get { return _instance; } }

    public GameObject finishText;
    public GameObject firstPlaceText;
    public GameObject secondPlaceText;
    public GameObject thirdPlaceText;
    public GameObject tieText;
    public GameObject playAgainButton;

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
    void Update()
    {
        if (GameManager.Instance.gameOver)
        {
            Cursor.visible = true;

            if (Input.GetKey(KeyCode.Space))
            {
                GameManager.Instance.RestartGame();
            }
        }
    }

    ///////////////////////////////////////////////
    /// PUBLIC METHODS
    ///////////////////////////////////////////////
    public void ShowPlayerStandingText()
    {
        finishText.SetActive(true);

        switch (GameManager.Instance.GetPlayerStanding())
        {
            case GameManager.STANDING.FIRST_PLACE:
                firstPlaceText.SetActive(true);
                break;
            case GameManager.STANDING.SECOND_PLACE:
                secondPlaceText.SetActive(true);
                break;
            case GameManager.STANDING.THIRD_PLACE:
                thirdPlaceText.SetActive(true);
                break;
            case GameManager.STANDING.TIE:
                tieText.SetActive(true);
                finishText.SetActive(false);
                break;
        }

        playAgainButton.SetActive(true);
    }

}
