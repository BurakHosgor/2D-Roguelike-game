using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  public static GameManager Instance;
  public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        LevelUp
    }
    //Store the current state of the game
    public GameState currentState;
    // Store the previous state of the game
    public GameState previousState;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultScreen;
    public GameObject levelUpScreen;

    [Header("Current Stat Displays")]
    public Text CurrentHealthDisplay;
    public Text CurrentRecoveryDisplay;
    public Text CurrentMoveSpeedDisplay;
    public Text CurrentMightDisplay;
    public Text CurrentProjectileSpeedDisplay;
    public Text CurrentMagnetDisplay;

    [Header("Results Screen Display")]
    public Image chosenCharacterImage;
    public Text chosenCharacterName;
    public Text levelReachedDisplay;
    public Text timeSurvivedDisplay;
    public List<Image> chosenWeaponsUI = new List<Image>(6);
    public List<Image> chosenPassiveItemsUI = new List<Image>(6);


    [Header("StopWatch")]
    public float timeLimit;
    float stopWatchTime;
    public Text stopWatchDisplay;


    // flag to check if the game is over
    public bool isGameOver = false;

    // Flag to check if the player is choosing their upgrades
    public bool choosingUpgrade;

    public GameObject playerObject;

    void Awake()
    {
        //Warning Check to see if there is another singleton of this kind in the game
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("EXTRA" + this + "DELETED");
        }
        DisableScreen();
    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.Gameplay:
                CheckForPauseAndResume();
                UpdateStopWatch();
                break;
            
            case GameState.Paused:
                CheckForPauseAndResume();
                break;
            
            case GameState.GameOver: 
                if (!isGameOver)
                {
                    isGameOver = true;
                    Debug.Log("Game is Over");
                    Time.timeScale = 0;
                    DisplayResults();
                }
                break;

            case GameState.LevelUp:
                if (!choosingUpgrade)
                {
                    choosingUpgrade = true;
                    Time.timeScale = 0; // Pause the game for now
                    Debug.Log("Upgrades shown");
                    levelUpScreen.SetActive(true);
                }
                break;

            default:
                Debug.LogWarning("State does not exist");
                break;
        }
    }
    //Define the method to change the state of the game
    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }
    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0; // Stop the game
            pauseScreen.SetActive(true);
            Debug.Log("Game is Paused");
        }
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused) 
        {
            ChangeState(previousState);
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
            Debug.Log("Game is Resumed");

        }
    }

    void CheckForPauseAndResume()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    void DisableScreen()
    {
        pauseScreen.SetActive(false);
        resultScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }

    public void GameOver()
    {
        timeSurvivedDisplay.text =stopWatchDisplay.text;
        ChangeState(GameState.GameOver);
    }

    void DisplayResults()
    {
        resultScreen.SetActive(true);
    }

    public void AssignChosenCharacterUI(CharacterScriptableObject chosenCharacterData)
    {
        chosenCharacterImage.sprite = chosenCharacterData.Icon;
        chosenCharacterName.text = chosenCharacterData.Name;
    }
    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }

    public void AssignChosenWeaponsAndPassiveItemsUI(List<Image> chosenWeaponsData, List<Image> chosenPassiveItemsData)
    {
        if (chosenWeaponsData.Count != chosenWeaponsUI.Count || chosenPassiveItemsData.Count != chosenPassiveItemsUI.Count)
        {
            Debug.Log("Chosen weapon and passive items data list have different lengths");
            return;
        }
        for (int i = 0; i < chosenWeaponsUI.Count; i++)
        {
            if (chosenWeaponsData[i].sprite)
            {
                chosenWeaponsUI[i].enabled = true;
                chosenWeaponsUI[i].sprite = chosenWeaponsData[i].sprite;

            }
            else
            {
                chosenWeaponsUI[i].enabled = false;
            }
        }

        for (int i = 0; i < chosenPassiveItemsUI.Count; i++)
        {
            if (chosenPassiveItemsData[i].sprite)
            {
                chosenPassiveItemsUI[i].enabled = true;
                chosenPassiveItemsUI[i].sprite = chosenPassiveItemsData[i].sprite;

            }
            else
            {
                chosenPassiveItemsUI[i].enabled = false;
            }
        }
    }

    void UpdateStopWatch()
    {
        stopWatchTime += Time.deltaTime;

        UpdateStopWatchDisplay();

        if(stopWatchTime >= timeLimit) 
        {
            GameOver();
        }
    }

    void UpdateStopWatchDisplay()
    {
        //Calculate the number of minutes and seconds that have elapsed
        int minutes = Mathf.FloorToInt(stopWatchTime / 60);
        int seconds = Mathf.FloorToInt(stopWatchTime % 60);

        // Update the stopwatch text to display the elapsed time
        stopWatchDisplay.text = string.Format("{0:00}:{1:00}",minutes,seconds);
    }

    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgrades"); 
    }

    public void EndLevelUp()
    {
        choosingUpgrade = false;
        Time.timeScale = 1f; // Resume the game
        levelUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
    }
}
