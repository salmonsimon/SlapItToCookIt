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
    [SerializeField] private GameObject pauseMenu;
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

            // TODO: GET SETTINGS FROM PLAYFAB
            //ProgressManager.Instance.Reset();

            //Settings.Load();
            //Settings.Instance.Deserialize();
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
        if (!isGamePaused && !isOnMainMenu && Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
        else if (isGamePaused && !isOnMainMenu && Input.GetKeyDown(KeyCode.Escape))
            ResumeGame();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!isOnMainMenu)
        {
            mainMenu.SetActive(false);

            //countersUI.gameObject.SetActive(true);

            //heatUI.gameObject.SetActive(true);
            //healthUI.Activate();

            currentProgressManager.ResetCounters();
            currentProgressManager.gameObject.SetActive(true);
        }
        else
        {
            mainMenu.SetActive(true);

            pauseMenu.SetActive(false);
            currentProgressManager.gameObject.SetActive(false);

            //countersUI.gameObject.SetActive(false);
            //heatUI.gameObject.SetActive(false);
        }

        levelLoader.FinishTransition();
    }

    public void ToMainMenu()
    {
        //dialogueManager.ClearDialogues();

        SetGamePaused(false);

        isOnMainMenu = true;

        levelLoader.LoadLevel(Config.MAIN_MENU_SCENE_NAME, Config.CROSSFADE_TRANSITION);

        pauseMenu.SetActive(false);
    }

    public void PauseGame()
    {
        GetSFXManager().PlaySound(Config.PAUSE_SFX);
        SetGamePaused(true);

        pauseMenu.SetActive(true);
        pauseMenu.transform.Find("Pause Panel").gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        SetGamePaused(false);

        pauseMenu.SetActive(false);
        pauseMenu.transform.Find("Settings Panel").gameObject.SetActive(false);
    }

    #region Getters and Setters

    public bool IsGamePaused()
    {
        return isGamePaused;
    }

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

    public void SetGamePaused(bool value)
    {
        isGamePaused = value;

        if (value)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
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

    #region Utils (have to reattach to other script later)

    /*

    public void SetAudioSlidersVolumesPauseMenu()
    {
        Slider musicVolumeSlider = null;
        Slider sfxVolumeSlider = null;

        if (currentProgressManager.CurrentFightingRoute == FightingRoute.None)
        {
            musicVolumeSlider = GameObject.Find("Pause Menu UI/Settings Panel/Music/Music Slider").GetComponent<Slider>();
            sfxVolumeSlider = GameObject.Find("Pause Menu UI/Settings Panel/SFX/SFX Slider").GetComponent<Slider>();
        }
        else
        {
            musicVolumeSlider = GameObject.Find("Pause Menu UI - Fighting Route/Settings Panel/Music/Music Slider").GetComponent<Slider>();
            sfxVolumeSlider = GameObject.Find("Pause Menu UI - Fighting Route/Settings Panel/SFX/SFX Slider").GetComponent<Slider>();
        }

        musicVolumeSlider.value = Settings.Instance.musicVolume;
        sfxVolumeSlider.value = Settings.Instance.SFXVolume;
    }
    */

    #endregion
}
