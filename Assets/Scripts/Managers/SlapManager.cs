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
    [SerializeField] private Button slapButton;
    [SerializeField] private Text temperatureText;
    [SerializeField] private Image thermometerFillerImage;

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
        }
    }

    private void SetUpgrades()
    {
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
}
