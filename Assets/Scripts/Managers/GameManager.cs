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

    #region UI

    [SerializeField] private MainMenuUI mainMenu;
    [SerializeField] private PauseUI pauseMenu;

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

            Destroy(mainMenu.gameObject);
            Destroy(pauseMenu.gameObject);
            //Destroy(heatUI.gameObject);
        }
        else
        {
            instance = this;

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
            mainMenu.gameObject.SetActive(false);

            pauseMenu.gameObject.SetActive(true);

            //heatUI.gameObject.SetActive(true);
            //healthUI.Activate();
        }
        else
        {
            mainMenu.ResetMainMenu();
            mainMenu.gameObject.SetActive(true);

            pauseMenu.gameObject.SetActive(false);

            //heatUI.gameObject.SetActive(false);
        }

        levelLoader.FinishTransition();
    }

    public void ToMainMenu()
    {
        //dialogueManager.ClearDialogues();

        pauseMenu.ResumeGame();
        

        SetIsOnMainMenu(true);

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

    /*

    public CinemachineShake GetCinemachineShake()
    {
        return cinemachineShake;
    }

    public DialogueManager GetDialogueManager()
    {
        return dialogueManager;
    }

    public HealthUI GetHeatUI()
    {
        return heatUI;
    }

    */

    #endregion

}
