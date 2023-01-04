using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlapManager : MonoBehaviour
{
    private CurrentProgressManager currentProgressManager;

    [SerializeField] private Button slapButton;
    [SerializeField] private Text temperatureText;
    [SerializeField] private Image thermometerFillerImage;

    private float fillerWidth = 0;
    private float fillerMaxHeight = 1225f;
    private float fillerMinHeight = 180f;

    private float currentTemperature = 0f;
    private float goalTemperature = 60f;

    private bool reachedGoalTemperature = false;

    private float temperatureIncreaseMagnitude = 1f;
    private float temperatureIncreaseMultiplier = 1f;

    private float temperatureLossMagnitude = .5f;

    private void Start()
    {
        currentProgressManager = GameManager.instance.GetCurrentProgressManager();

        fillerWidth = thermometerFillerImage.rectTransform.rect.width;

        UpdateTemperatureUI();
    }

    private void Update()
    {
        if (!reachedGoalTemperature)
        {
            if (currentTemperature > 0)
            {
                currentTemperature -= Time.deltaTime * temperatureLossMagnitude;

                if (currentTemperature < 0)
                    currentTemperature = 0;
            }

            UpdateTemperatureUI();
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
        //TODO: THIS ONE WILL DEPEND ON HOW MANY HANDS WE HAVE
        int slaps = 1;

        GameManager.instance.GetSFXManager().PlayRandomSlapClip();

        currentProgressManager.IncreaseSlapCount(slaps);

        float temperatureIncrease = slaps * temperatureIncreaseMagnitude * temperatureIncreaseMultiplier;

        currentTemperature += temperatureIncrease;

        CheckIfReachedGoalTemperature();
    }

    private void CheckIfReachedGoalTemperature()
    {
        if (currentTemperature > goalTemperature)
        {
            GameManager.instance.GetCurrentProgressManager().FinishedSlapping = true;

            reachedGoalTemperature = true;
            temperatureText.text = goalTemperature.ToString() + "°C";

            slapButton.gameObject.SetActive(false);
            GameManager.instance.GetSFXManager().PlaySound(Config.OVEN_SFX);
        }
    }
}
