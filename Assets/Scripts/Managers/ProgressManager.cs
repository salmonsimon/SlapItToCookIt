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
    public int Coins { get { return coins; } private set { coins = value; } }

    [SerializeField] private int rubies = 100;
    public int Rubies { get { return rubies; } private set { rubies = value; } }

    #endregion

    #region Upgrades

    [Header("Upgrades")]
    [SerializeField] private int handsCount = 1;
    public int HandsCount { get { return handsCount; } private set { handsCount = value; } }

    [SerializeField] private float temperatureIncreaseMultiplier = 1.0f;
    public float TemperatureIncreaseMultiplier { get { return temperatureIncreaseMultiplier; } private set { temperatureIncreaseMultiplier = value; } }

    [Header("Upgrades - New Hands")]
    [SerializeField] private int handUpgradesDone = 0;
    public int HandUpgradesDone { get { return handUpgradesDone; } private set { handUpgradesDone = value; } }

    [SerializeField] private float newHandUpgradeRedeemTime = -1f;
    public float NewHandUpgradeRedeemTime { get { return newHandUpgradeRedeemTime; } private set { newHandUpgradeRedeemTime = value; } }

    [Header("Upgrades - New Temperature")]
    [SerializeField] private int multiplierUpgradesDone = 0;
    public int MultiplierUpgradesDone { get { return multiplierUpgradesDone; } private set { multiplierUpgradesDone = value; } }

    [SerializeField] private float newMultiplierUpgradeRedeemTime = -1f;
    public float NewMultiplierUpgradeRedeemTime { get { return newMultiplierUpgradeRedeemTime; } private set { newMultiplierUpgradeRedeemTime = value; } }

    #endregion

    #region Score Record

    [Header("Score Record")]
    [SerializeField] private float recordTime = -1f;
    public float RecordTime { get { return recordTime; } private set { recordTime = value; } }

    [SerializeField] private int recordSlaps = -1;
    public int RecordSlaps { get { return recordSlaps; } private set { recordSlaps = value; } }

    #endregion

    public void UpdateProgress(string coins, string rubies, string handUpgradesDone, string newHandUpgradeRedeemTime, string multiplierUpgradesDone, string newMultiplierUpgradeRedeemTime)
    {
        Coins = int.Parse(coins);
        Rubies = int.Parse(rubies);

        HandUpgradesDone = int.Parse(handUpgradesDone);
        NewHandUpgradeRedeemTime = float.Parse(newHandUpgradeRedeemTime);

        MultiplierUpgradesDone = int.Parse(multiplierUpgradesDone);
        NewMultiplierUpgradeRedeemTime = float.Parse(newMultiplierUpgradeRedeemTime);
    }

    public bool CheckIfNewRecord(float newTimePlayed, int newSlapCount)
    {
        bool newRecord = false;

        if (recordTime == -1 || recordSlaps == -1)
        {
            WriteNewRecord(newTimePlayed, newSlapCount);

            newRecord = true;
        }
        else if (newTimePlayed < recordTime)
        {
            WriteNewRecord(newTimePlayed, newSlapCount);

            newRecord = true;
        }

        return newRecord;
    }

    private void WriteNewRecord(float newTimePlayed, int newSlapCount)
    {
        // TODO: DO API CALL TO WRITE IN SERVER
        RecordTime = newTimePlayed;
        RecordSlaps = newSlapCount;

        //GameManager.instance.GetPlayfabManager().WriteLeaderboard(newTimePlayed, newSlapCount);
    }

    public void EarnCoins(int amountEarned)
    {
        Coins += amountEarned;

        WriteCoinsAmountAPICall();
    }

    public void PayCoins(int amountPayed)
    {
        Coins -= amountPayed;

        WriteCoinsAmountAPICall();
    }

    public void PayRubies(int amountPayed)
    {
        Rubies -= amountPayed;

        WriteCurrentRubiesAmountAPICall();
    }

    private void WriteCoinsAmountAPICall()
    {
        // TODO: DO API CALL TO WRITE IN SERVER
    }

    private void WriteCurrentRubiesAmountAPICall()
    {
        // TODO: DO API CALL TO WRITE IN SERVER
    }

    public void RedeemNewHandUpgrade()
    {
        HandsCount = GetHandUpgradeValue((HandUpgrade)handUpgradesDone);
        handUpgradesDone++;

        WriteHandsUpgradesAPICall();
    }

    public void RedeemNewMultiplierUpgrade()
    {
        TemperatureIncreaseMultiplier = GetTemperatureUpgradeValue((TemperatureUpgrade)MultiplierUpgradesDone);
        multiplierUpgradesDone++;

        WriteHandsUpgradesAPICall();
    }

    private void WriteHandsUpgradesAPICall()
    {
        // TODO: DO API CALL TO WRITE IN SERVER
        // handsCount , handUpgradesDone
    }

    private void WriteMultiplierUpgradesAPICall()
    {
        // TODO: DO API CALL TO WRITE IN SERVER
        // temperatureIncreaseMultiplayer / multiplierUpgradesDone
    }

    /*
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

    */

    #region API Calls

    public void UpdateCountersAPICall()
    {
        // TODO: DO ALL THE API CALLS FROM HERE
        // TO GET COINS / RUBIES / HANDS COUNT / TEMPERATURE MULTIPLIER
    }

    public void UpdateRecordsAPICall()
    {
        // TODO: READ FROM SERVER OUR CURRENT RECORD
        // TO GET RECORD TIME AND RECORD SLAPS
    }

    public float GetServerTimeAPICall()
    {
        // TODO: CHANGE TIME.TIME TO BE GET FROM SERVER
        return Time.time;
    }

    public void WriteNewHandUpgradeRedeemTimeAPICall(float delayInSeconds)
    {
        // TODO: WRITE NEW HAND UPGRADE REDEEM TIME IN SERVER
        NewHandUpgradeRedeemTime = Time.time + delayInSeconds;
    }

    public float GetNewHandUpgradeRedeemTimeAPICall()
    {
        // TODO: CHANGE THIS TO BE GET FROM SERVER, IN SERVER IF NOT WAITING WILL BE SET TO -1
        return newHandUpgradeRedeemTime;
    }

    public void ResetNewHandUpgradeRedeemTimeAPICall()
    {
        // TODO: CHANGE THIS TO WRITE INTO THE SERVER
        newHandUpgradeRedeemTime = -1;
    }

    public void WriteNewMultiplierUpgradeRedeemTimeAPICall(float delayInSeconds)
    {
        // TODO: WRITE NEW HAND UPGRADE REDEEM TIME IN SERVER
        NewMultiplierUpgradeRedeemTime = Time.time + delayInSeconds;
    }

    public float GetNewMultiplierUpgradeRedeemTimeAPICall()
    {
        // TODO: CHANGE THIS TO BE GET FROM SERVER, IN SERVER IF NOT WAITING WILL BE SET TO -1
        return newMultiplierUpgradeRedeemTime;
    }

    public void ResetNewMultiplierUpgradeRedeemTimeAPICall()
    {
        // TODO: CHANGE THIS TO WRITE INTO THE SERVER
        newMultiplierUpgradeRedeemTime = -1;
    }

    #endregion
}
