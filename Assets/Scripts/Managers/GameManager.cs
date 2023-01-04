using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region GameObjects

    [SerializeField] private LevelLoader levelLoader;
    //[SerializeField] private CinemachineShake cinemachineShake;
    //[SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private SFXManager sfxManager;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private CurrentProgressManager currentProgressManager;

    #region UI

    [SerializeField] private GameObject mainMenu;

    [SerializeField] private PauseUI pauseMenu;

    //[SerializeField] private CountersUI countersUI;
    //[SerializeField] private HeatUI heatUI;

    #endregion

    #endregion

    #region Logic Variables

    [SerializeField] private bool isOnMainMenu = true;

    private bool isGamePaused;
    private bool isTeleporting;

    #endregion

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(levelLoader.gameObject);
            //Destroy(cinemachineShake.gameObject);
            //Destroy(dialogueManager.gameObject);
            Destroy(sfxManager.gameObject);
            Destroy(musicManager.gameObject);
            Destroy(currentProgressManager.gameObject);

            Destroy(mainMenu.gameObject);
            Destroy(pauseMenu.gameObject);
            //Destroy(countersUI.gameObject);
            //Destroy(heatUI.gameObject);
        }
        else
        {
            instance = this;

            currentProgressManager.ResetCounters();

            // TODO: GET PROGRESS FROM PLAYFAB
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (!pauseMenu.IsGamePaused && !isOnMainMenu && Input.GetKeyDown(KeyCode.Escape))
            pauseMenu.PauseGame();
        else if (pauseMenu.IsGamePaused && !isOnMainMenu && Input.GetKeyDown(KeyCode.Escape))
            pauseMenu.ResumeGame();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!isOnMainMenu)
        {
            mainMenu.SetActive(false);

            pauseMenu.gameObject.SetActive(true);

            //countersUI.gameObject.SetActive(true);

            //heatUI.gameObject.SetActive(true);
            //healthUI.Activate();

            currentProgressManager.ResetCounters();
            currentProgressManager.gameObject.SetActive(true);
        }
        else
        {
            mainMenu.SetActive(true);

            pauseMenu.gameObject.SetActive(false);

            currentProgressManager.gameObject.SetActive(false);

            //countersUI.gameObject.SetActive(false);
            //heatUI.gameObject.SetActive(false);
        }

        levelLoader.FinishTransition();
    }

    public void ToMainMenu()
    {
        //dialogueManager.ClearDialogues();

        pauseMenu.SetGamePaused(false);

        isOnMainMenu = true;

        levelLoader.LoadLevel(Config.MAIN_MENU_SCENE_NAME, Config.CROSSFADE_TRANSITION);

        pauseMenu.gameObject.SetActive(false);
    }

    #region Getters and Setters

    public bool IsOnMainMenu()
    {
        return isOnMainMenu;
    }

    public void SetIsOnMainMenu(bool value)
    {
        isOnMainMenu = value;
    }

    public bool IsTeleporting()
    {
        return isTeleporting;
    }

    public void SetIsTeleporting(bool value)
    {
        isTeleporting = value;
    }

    public PauseUI GetPauseUI()
    {
        return pauseMenu;
    }

    public SFXManager GetSFXManager()
    {
        return sfxManager;
    }

    public MusicManager GetMusicManager()
    {
        return musicManager;
    }

    public LevelLoader GetLevelLoader()
    {
        return levelLoader;
    }

    public CurrentProgressManager GetCurrentProgressManager()
    {
        return currentProgressManager;
    }

    /*

    public CinemachineShake GetCinemachineShake()
    {
        return cinemachineShake;
    }

    public DialogueManager GetDialogueManager()
    {
        return dialogueManager;
    }

    public CountersUI GetCountersUI()
    {
        return countersUI;
    }

    public HealthUI GetHeatUI()
    {
        return heatUI;
    }

    */

    #endregion

}
