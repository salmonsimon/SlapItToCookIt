using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{
    #region Register



    #endregion

    #region Login
    private void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    private void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful login / account creation!");
        

        //WritePlayerProgress(DEFAULT_PLAYER_DATA);
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log("Error while loggin in / creating new account");
        Debug.Log(error.GenerateErrorReport());
    }

    #endregion

    #region Progress Save

    public void GetPlayerProgress()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnPlayerProgressRead, OnError);
    }

    public void WritePlayerProgress(Dictionary<string, string> dataToSave)
    {
        var request = new UpdateUserDataRequest
        {
            Data = dataToSave
        };

        PlayFabClientAPI.UpdateUserData(request, OnPlayerProgressWrite, OnError);
    }

    private void OnPlayerProgressWrite(UpdateUserDataResult result)
    {
        Debug.Log("Player progress successfully writen");
    }

    private void OnPlayerProgressRead(GetUserDataResult result)
    {
        Debug.Log("Player Progress Successfully Read");

        if (result.Data!= null && result.Data.ContainsKey(Config.API_COINS_KEY) && result.Data.ContainsKey(Config.API_RUBIES_KEY) 
            && result.Data.ContainsKey(Config.API_HAND_UPGRADES_DONE_KEY) && result.Data.ContainsKey(Config.API_NEW_HAND_UPGRADE_REDEEM_TIME_KEY) 
            && result.Data.ContainsKey(Config.API_MULTIPLIER_UPGRADES_DONE_KEY) && result.Data.ContainsKey(Config.API_NEW_MULTIPLIER_UPGRADE_REDEEM_TIME_KEY))
        {
            GameManager.instance.GetProgressManager().UpdateProgress(result.Data[Config.API_COINS_KEY].Value, 
                                                                     result.Data[Config.API_RUBIES_KEY].Value,
                                                                     result.Data[Config.API_HAND_UPGRADES_DONE_KEY].Value, 
                                                                     result.Data[Config.API_NEW_HAND_UPGRADE_REDEEM_TIME_KEY].Value,
                                                                     result.Data[Config.API_MULTIPLIER_UPGRADES_DONE_KEY].Value, 
                                                                     result.Data[Config.API_NEW_MULTIPLIER_UPGRADE_REDEEM_TIME_KEY].Value
                                                                     );
        }
    }

    public Dictionary<string, string> DEFAULT_PLAYER_DATA = new Dictionary<string, string>
    {
        { Config.API_COINS_KEY, "0" },
        { Config.API_RUBIES_KEY, "100" },

        { Config.API_HAND_UPGRADES_DONE_KEY, "0" },
        { Config.API_NEW_HAND_UPGRADE_REDEEM_TIME_KEY, "-1" },

        { Config.API_MULTIPLIER_UPGRADES_DONE_KEY, "0" },
        { Config.API_NEW_MULTIPLIER_UPGRADE_REDEEM_TIME_KEY, "-1" }
    };

    #endregion

    #region Leaderboard

    public void WriteLeaderboard(float timePlayed, int slapCount)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Time Played",
                    Value = (int)timePlayed
                },
                new StatisticUpdate
                {
                    StatisticName = "Slap Count",
                    Value = slapCount
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successful leaderboard sent");
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "Time Played",
            StartPosition = 0,
            MaxResultsCount = 10,
        };

        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    private void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (var item in result.Leaderboard)
        {
            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
        }
    }

    #endregion
}
