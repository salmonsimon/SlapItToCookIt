using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using static Utils;

public class SlapManager : MonoBehaviour
{

    [Header("Progress Counters")]
    [SerializeField] private float timePlayed = 0f;
    [SerializeField] private int slapCount = 0;

    [Header("Object References")]
    [SerializeField] private Button slapButton;
    [SerializeField] private Text temperatureText;
    [SerializeField] private Image thermometerFillerImage;

    [Header("Stage Cleared References")]
    [SerializeField] private GameObject stageClearedPanel;
    [SerializeField] private Text timePlayedText;
    [SerializeField] private Text slapCountText;
    [SerializeField] private Text newRecordText;
    [SerializeField] private Text coinsEarnedText;

    [SerializeField] private GameObject errorPanel;
    [SerializeField] private Text errorText;

    #region Logic Variables

    private float temperatureIncreaseMultiplier = 1f;
    private int handsCount = 1;

    private float currentTemperature = 0f;
    private float goalTemperature = 60f;

    private bool reachedGoalTemperature = false;
    public bool ReachedGoalTemperature { get { return reachedGoalTemperature; } set { reachedGoalTemperature = value; } }


    #endregion

    #region Parameters

    private float temperatureIncreaseMagnitude = 1f;
    private float temperatureLossMagnitude = .5f;

    #endregion

    #region Temperature UI

    private float fillerWidth = 0;
    private float fillerMaxHeight = 1225f;
    private float fillerMinHeight = 180f;

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
        SpawnHands();
    }

    private void Update()
    {
        if (!ReachedGoalTemperature)
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

        if (GameManager.instance.GetProgressManager().CheckIfNewRecord(timePlayed, slapCount))
            newRecordText.gameObject.SetActive(true);

        OnCompletedeLevel();
    }

    private void OnCompletedeLevel()
    {
        var initializePlayerRequest = new ExecuteCloudScriptRequest()
        {
            FunctionName = "OnCompletedLevel",
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(initializePlayerRequest, OnCompletedLevelResponse, OnError);
    }

    private void OnCompletedLevelResponse(ExecuteCloudScriptResult result)
    {
        var lastLog = result.Logs[result.Logs.Count - 1];

        if (lastLog.Level == "Error")
        {
            StartCoroutine(ShowErrorMessage(lastLog.Message));
        }
        else
        {
            int coinsEarned = CalculateCoinsEarned();

            coinsEarnedText.text = coinsEarned.ToString();

            stageClearedPanel.gameObject.SetActive(true);
        }
    }

    private int CalculateCoinsEarned()
    {
        return 50;
    }

    private IEnumerator PlayStageClearedSounds()
    {
        GameManager.instance.GetSFXManager().PlaySound(Config.STAGE_CLEARED_SFX);

        yield return new WaitForSeconds(3.5f);

        GameManager.instance.GetSFXManager().PlayRandomCongratulationsSound();
    }

    private void SetUpgrades()
    {
        GameManager.instance.GetProgressManager().UpdateCountersAPICall();

        handsCount = GameManager.instance.GetProgressManager().HandsCount;
        temperatureIncreaseMultiplier = GameManager.instance.GetProgressManager().TemperatureIncreaseMultiplier;

        if (handsCount > 3) handsCount = 3;
        if (temperatureIncreaseMultiplier > 2f) temperatureIncreaseMultiplier = 2f;
    }

    private void SpawnHands()
    {

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
        return FloatToTimeFormat(timePlayed);
    }

    public void PlayGame()
    {
        GameManager.instance.SetIsOnMainMenu(false);

        GameManager.instance.GetLevelLoader().LoadLevel(Config.MAIN_SCENE_NAME, Config.CROSSFADE_TRANSITION);
    }

    public void ToMainMenu()
    {
        GameManager.instance.ToMainMenu();
    }

    private void OnError(PlayFabError error)
    {
        StartCoroutine(ShowErrorMessage(error.ErrorMessage));
    }

    public void ClearMessageText()
    {
        errorText.text = string.Empty;
    }

    private IEnumerator ShowErrorMessage(string message)
    {
        errorText.text = message;
        errorPanel.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        errorPanel.gameObject.SetActive(false);
        ClearMessageText();
    }
}
