using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUIController : MonoBehaviour {

    ///////////////////////////////////////////////
    /// MEMBERS
    ///////////////////////////////////////////////
    public Dropdown AIDifficultyDropdown;

    ///////////////////////////////////////////////
    /// MONOBEHAVIOR METHODS
    ///////////////////////////////////////////////
    void Start ()
    {
        GameManager.Instance.PauseGame();
	}
	void Update ()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            HidePanel();
            GameManager.Instance.ContinueGame();
        }
    }

    ///////////////////////////////////////////////
    /// PUBLIC METHODS
    ///////////////////////////////////////////////
    public void HidePanel()
    {
        switch (AIDifficultyDropdown.value)
        {
            case 0:
                GameManager.Instance.SetAIDifficulty(GameManager.AI_DIFFICULTY.EASY);
                break;
            case 1:
                GameManager.Instance.SetAIDifficulty(GameManager.AI_DIFFICULTY.MEDIUM);
                break;
            case 2:
                GameManager.Instance.SetAIDifficulty(GameManager.AI_DIFFICULTY.HARD);
                break;
        }
       
        gameObject.SetActive(false);
    }

}
