using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Upgrade;

public class ProgressManager : MonoBehaviour
{

    #region Currency

    [Header("Currency")]
    [SerializeField] private int coins = 0;
    [SerializeField] private int rubies = 100;

    #endregion

    #region Upgrades

    [Header("Upgrades")]
    [SerializeField] private int handsCount = 1;
    public int HandsCount { get { return handsCount; } private set { handsCount = value; } }

    [SerializeField] private float temperatureIncreaseMultiplier = 1.0f;
    public float TemperatureIncreaseMultiplier { get { return temperatureIncreaseMultiplier; } }

    [Header("Upgrades - New Hands")]
    [SerializeField] private int handUpgradesDone = 0;
    [SerializeField] private float newHandUpgradeRedeemTime = 0f;

    [Header("Upgrades - New Temperature")]
    [SerializeField] private int temperatureUpgradesDone = 0;
    [SerializeField] private float newTemperatureUpgradeRedeemTime = 0f;

    #endregion

    public delegate void OnProgressChangeDelegate();
    public event OnProgressChangeDelegate OnProgressChange;

    private void ShowCurrentHandUpgradeCostCoins()
    {

    }

    private void ShowCurrentHandUpgradeCostRubies()
    {

    }

    private void BuyNewHandUpgradeCoins()
    {

    }

    private void BuyNewHandUpgradeRubies()
    {

    }

    private void ShowCurrentHandUpgradeFastForwardPrice()
    {

    }

    private void FastForwardCurrentHandUpgradeRedeem()
    {

    }

    private void RedeemNewHandUpgrade()
    {
        // TODO: Update hand redeem time from API
        float newHandUpgradeServerRedeemTime = GetNewHandUpgradeRedeemTimeAPICall();

        if (newHandUpgradeRedeemTime < 0)
            Debug.Log("No hand upgrade on course");

        if (UpgradeReadyToRedeem(newHandUpgradeRedeemTime))
        {
            HandsCount = GetHandUpgradeValue((HandUpgrade)handUpgradesDone);
            handUpgradesDone++;

            // TO DO: SAVE NEW INFO TO API

            ResetNewHandUpgradeRedeemTimeAPICall();
        }
        else
        {
            Debug.Log("Processing hand upgrade");
        }
    }

    private void RedeemNewTemperatureUpgrade()
    {
        // TODO: Update temperature redeem time
        float newTemperatureUpgradeServerRedeemTime = GetNewTemperatureUpgradeRedeemTimeAPICall();

        if (newTemperatureUpgradeServerRedeemTime < 0)
            Debug.Log("No temperature upgrade on course");

        if (UpgradeReadyToRedeem(newTemperatureUpgradeServerRedeemTime))
        {
            temperatureIncreaseMultiplier = GetTemperatureUpgradeValue((TemperatureUpgrade)temperatureUpgradesDone);
            temperatureUpgradesDone++;

            // TO DO: SAVE NEW INFO TO API

            ResetNewTemperatureUpgradeRedeemTimeAPICall();
        }
        else
        {
            Debug.Log("Processing temperature upgrade");
        }
    }

    private bool UpgradeReadyToRedeem(float targetTime)
    {
        bool handUpgradeReady = false;

        float serverTime = GetServerTimeAPICall();

        if (serverTime > targetTime)
        {
            handUpgradeReady = true;
        }

        return handUpgradeReady;
    }

    #region API Calls

    private void GetHandsCountAPICall()
    {

    }

    private void GetTemperatureMultiplierAPICall()
    {

    }

    private float GetServerTimeAPICall()
    {
        // TODO: CHANGE TIME.TIME TO BE GET FROM SERVER
        return Time.time;
    }

    private float GetNewHandUpgradeRedeemTimeAPICall()
    {
        // TODO: CHANGE THIS TO BE GET FROM SERVER, IN SERVER IF NOT WAITING WILL BE SET TO -1
        return newHandUpgradeRedeemTime;
    }

    private void ResetNewHandUpgradeRedeemTimeAPICall()
    {
        // TODO: CHANGE THIS TO WRITE INTO THE SERVER
        newHandUpgradeRedeemTime = -1;
    }

    private float GetNewTemperatureUpgradeRedeemTimeAPICall()
    {
        // TODO: CHANGE THIS TO BE GET FROM SERVER, IN SERVER IF NOT WAITING WILL BE SET TO -1
        return newTemperatureUpgradeRedeemTime;
    }

    private void ResetNewTemperatureUpgradeRedeemTimeAPICall()
    {
        // TODO: CHANGE THIS TO WRITE INTO THE SERVER
        newTemperatureUpgradeRedeemTime = -1;
    }

    #endregion
}
