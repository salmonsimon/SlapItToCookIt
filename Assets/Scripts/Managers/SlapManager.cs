using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Utils;

public class SlapManager : MonoBehaviour
{

    [Header("Progress Counters")]
    [SerializeField] private float timePlayed = 0f;
    [SerializeField] private int slapCount = 0;

    [Header("Object References")]
    [SerializeField] private GameObject tapCanvas;
    [SerializeField] private Button slapButton;
    [SerializeField] private Text temperatureText;
    [SerializeField] private Image thermometerFillerImage;
    [SerializeField] private List<GameObject> comicImagesList;

    [Header("Stage Cleared References")]
    [SerializeField] private GameObject stageClearedPanel;
    [SerializeField] private Text timePlayedText;
    [SerializeField] private Text slapCountText;
    [SerializeField] private Text newRecordText;
    [SerializeField] private Text coinsEarnedText;

    [Header("Error Management")]
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private Text errorText;

    #region Logic Variables

    private float temperatureIncreaseMultiplier = 1f;
    private int handsCount = 1;

    private float currentTemperature = 0f;
    private float goalTemperature = 60f;

    private bool reachedGoalTemperature = false;
    public bool ReachedGoalTemperature { get { return reachedGoalTemperature; } set { reachedGoalTemperature = value; } }

    private bool hasSlapped = false;


    #endregion

    #region Parameters

    private float temperatureIncreaseMagnitude = .3f;
    private float temperatureLossMagnitude = .1f;

    #endregion

    #region Temperature UI

    private float fillerWidth = 0;
    private float fillerMaxHeight = Config.TEMPERATURE_UI_MAX_HEIGHT;
    private float fillerMinHeight = Config.TEMPERATURE_UI_MIN_HEIGHT;

    #endregion

    #region Events

    public delegate void OnCountersChangeDelegate();
    public event OnCountersChangeDelegate OnCountersChange;

    #endregion

    private void Start()
    {
        fillerWidth = thermometerFillerImage.rectTransform.rect.width;

        UpdateTemperatureUI();

        SetUpgrades();
    }

    private void Update()
    {
        if (hasSlapped && !ReachedGoalTemperature)
        {
            if (currentTemperature > 0)
            {
                currentTemperature -= Time.deltaTime * temperatureLossMagnitude;

                if (currentTemperature < 0)
                    currentTemperature = 0;
            }

            UpdateTemperatureUI();

            timePlayed += Time.deltaTime;
        }
    }

    private void UpdateTemperatureUI()
    {
        temperatureText.text = ((int)currentTemperature).ToString() + "°C";

        var newRectHeight = Mathf.Lerp(fillerMinHeight, fillerMaxHeight, currentTemperature / goalTemperature);
        thermometerFillerImage.rectTransform.sizeDelta = new Vector2(fillerWidth, newRectHeight);
    }

    public void OnSlap()
    {
        GameManager.instance.GetSFXManager().PlayRandomSlapClip();
        GameManager.instance.GetCinemachineShake().ShakeCamera(Config.CAMERASHAKE_HIT_AMPLITUDE, Config.CAMERASHAKE_HIT_DURATION * 2);
        ShowRandomComicImage();

        if (!hasSlapped) 
        { 
            hasSlapped = true;
            tapCanvas.SetActive(false);
        }

        IncreaseSlapCount(handsCount);

        float temperatureIncrease = handsCount * temperatureIncreaseMagnitude * temperatureIncreaseMultiplier;

        currentTemperature += temperatureIncrease;

        CheckIfReachedGoalTemperature();
    }

    private void CheckIfReachedGoalTemperature()
    {
        if (currentTemperature > goalTemperature)
        {
            ReachedGoalTemperature = true;
            temperatureText.text = goalTemperature.ToString() + "°C";
            thermometerFillerImage.rectTransform.sizeDelta = new Vector2(fillerWidth, fillerMaxHeight);

            slapButton.gameObject.SetActive(false);
            GameManager.instance.GetSFXManager().PlaySound(Config.OVEN_SFX);

            StageCleared();
        }
    }

    private void StageCleared()
    {
        StartCoroutine(PlayStageClearedSounds());

        slapCountText.text = slapCount.ToString("#,##0");
        timePlayedText.text = ShowCurrentTimePlayed();

        OnCompletedeLevel();

        float currentRecordTime = GameManager.instance.GetProgressManager().RecordTime;

        if (currentRecordTime < 0 || timePlayed < currentRecordTime)
            SetNewRecord();
    }

    private IEnumerator PlayStageClearedSounds()
    {
        GameManager.instance.GetSFXManager().PlaySound(Config.STAGE_CLEARED_SFX);

        yield return new WaitForSeconds(Config.CONGRATULATIONS_SFX_DELAY);

        GameManager.instance.GetSFXManager().PlayRandomCongratulationsSound();
    }

    private void SetUpgrades()
    {
        handsCount = GameManager.instance.GetProgressManager().HandsCount;
        temperatureIncreaseMultiplier = GameManager.instance.GetProgressManager().TemperatureIncreaseMultiplier;

        if (handsCount > Config.MAX_HAND_COUNT) handsCount = Config.MAX_HAND_COUNT;
        if (temperatureIncreaseMultiplier > Config.MAX_MULTIPLIER_VALUE) temperatureIncreaseMultiplier = Config.MAX_MULTIPLIER_VALUE;
    }

    private void IncreaseSlapCount(int value)
    {
        slapCount += value;

        OnCountersChange();
    }

    public void ResetCounters()
    {
        timePlayed = 0f;
        slapCount = 0;

        OnCountersChange();
    }

    public int GetCurrentSlapCount()
    {
        return slapCount;
    }

    public string ShowCurrentTimePlayed()
    {
        return FloatToTimeMillisecondsFormat(timePlayed);
    }

    private void ShowRandomComicImage()
    {
        bool show = Random.Range(0f, 1f) < .3f;

        if(show)
        {
            int randomImage = Random.Range(0, comicImagesList.Count);

            StartCoroutine(ShowImage(comicImagesList[randomImage], Config.CAMERASHAKE_HIT_DURATION));
        }
    }

    private IEnumerator ShowImage(GameObject image, float duration)
    {
        image.SetActive(true);

        yield return new WaitForSeconds(duration);

        image.SetActive(false);
    }

    #region Buttons

    public void PlayGame()
    {
        GameManager.instance.SetIsOnMainMenu(false);

        GameManager.instance.GetLevelLoader().LoadLevel(Config.MAIN_SCENE_NAME, Config.CROSSFADE_TRANSITION);
    }

    public void ToMainMenu()
    {
        GameManager.instance.ToMainMenu();
    }

    public void LeaderboardButton()
    {
        GameManager.instance.GetLeaderboardUI().DisplayLeaderboard();
    }

    #endregion

    #region API Calls

    private void SetNewRecord()
    {
        var recordData = new { RecordSlaps = slapCount, RecordTime = timePlayed };

        var initializePlayerRequest = new ExecuteCloudScriptRequest()
        {
            FunctionName = Config.API_SET_NEW_RECORD_FUNCTION_NAME,
            FunctionParameter = recordData,
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(initializePlayerRequest, OnSetNewRecordResponse, OnError);
    }

    private void OnSetNewRecordResponse(ExecuteCloudScriptResult result)
    {
        var lastLog = result.Logs[result.Logs.Count - 1];

        if (lastLog.Level == "Error")
        {
            StartCoroutine(GameManager.instance.GetErrorUI().ShowErrorMessage(lastLog.Message));
        }
        else
        {
            newRecordText.gameObject.SetActive(true);
        }

        GameManager.instance.GetProgressManager().UpdateProgress();
    }

    private void OnCompletedeLevel()
    {
        var initializePlayerRequest = new ExecuteCloudScriptRequest()
        {
            FunctionName = Config.API_ON_COMPLETED_LEVEL_FUNCTION_NAME,
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(initializePlayerRequest, OnCompletedLevelResponse, OnError);
    }

    private void OnCompletedLevelResponse(ExecuteCloudScriptResult result)
    {
        var lastLog = result.Logs[result.Logs.Count - 1];

        if (lastLog.Level == "Error")
        {
            StartCoroutine(GameManager.instance.GetErrorUI().ShowErrorMessage(lastLog.Message));
        }
        else
        {
            int coinsEarned = CalculateCoinsEarned();

            coinsEarnedText.text = coinsEarned.ToString();

            stageClearedPanel.gameObject.SetActive(true);
        }

        GameManager.instance.GetCurrencyManager().GetVirtualCurrencies();
    }

    private void OnError(PlayFabError error)
    {
        StartCoroutine(GameManager.instance.GetErrorUI().ShowErrorMessage(error.ErrorMessage));
    }

    public void ClearMessageText()
    {
        errorText.text = string.Empty;
    }

    private int CalculateCoinsEarned()
    {
        return Config.COMPLETED_LEVEL_REWARD;
    }

    #endregion
}
