using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject activeMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;

    public PlayerController playerScript;
    public GameObject player;
    public Image playerHPBar;
    public GameObject playerDamageFlash;
    public GameObject playerSpawnPosition;

    public bool isPaused;
    int enemyCount;
    //Awake used to initialize the game manager first 
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        playerSpawnPosition = GameObject.FindGameObjectWithTag("Player Spawn Pos");
    }

    void Update() 
    {
        if(Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            PausedState();
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);
        }
       CollectedScrap();
        
        if (playerScript != null)
        {
            playerScript.updatePlayerUI();
        }
    }

    public void PausedState()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void UnpausedState()
    {
        isPaused = !isPaused;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);
        activeMenu = null;
    }
    public void GameGoalUpdate(int amount)
    {
        enemyCount += amount;
        if(enemyCount <= 0)
        {
            PausedState();
            activeMenu = winMenu;
            activeMenu.SetActive(true);
        }
    }
    public void youLose(){
        PausedState();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }

    void CollectedScrap()
    {
        // Access the ScrapTracker instance
        ScrapTracker scrapTracker = ScrapTracker.instance;

        // Check if the instance is not null
        if (scrapTracker != null)
        {
            // Use scrapTracker to access its methods or properties
            int totalScrap = scrapTracker.TotalScrap;
            // ... (other actions)
        }
        else
        {
            Debug.LogError("ScrapTracker instance not found!");
        }
    }
}

