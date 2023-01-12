using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using static Utils;

public class LeaderboardUI : MonoBehaviour
{
    #region Object References

    [Header("Display Name Panel")]
    [SerializeField] private GameObject displayNamePanel;
    [SerializeField] private InputField displayNameInputField;

    [Header("Leaderboard Panel")]
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private Transform playerScoresContainer;
    [SerializeField] private GameObject rowPrefab;

    #endregion

    #region Logic Variables

    private string displayName;
    private int minRankingShowed = 0;

    #endregion

    #region Parameters

    private int maxRowsToShow = 5;

    #endregion

    private void OnEnable()
    {
        GetDisplayName();
    }

    #region Buttons

    public void SetDisplayNameButton()
    {
        SetDisplayName();
    }

    public void GetLeaderboardPreviousBatchButton()
    {
        if (minRankingShowed == 0)
            return;

        minRankingShowed -= maxRowsToShow;

        if (minRankingShowed < 0)
            minRankingShowed = 0;

        GetLeaderboard(minRankingShowed);
    }

    public void GetLeaderboardNextBatchButton()
    {
        minRankingShowed += maxRowsToShow;

        GetLeaderboard(minRankingShowed);
    }

    #endregion

    #region Display Name

    private void GetDisplayName() 
    {
        var request = new GetPlayerProfileRequest()
        {
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true
            }
        };

        PlayFabClientAPI.GetPlayerProfile(request, OnGetDisplayNameResponse, OnError);
    }

    private void OnGetDisplayNameResponse(GetPlayerProfileResult result) 
    {
        string name = result.PlayerProfile.DisplayName;

        if (!string.IsNullOrEmpty(name))
        {
            displayName = name;
        }
        else 
        {
            displayNamePanel.transform.parent.gameObject.SetActive(true);
            displayNamePanel.SetActive(true);
        }
    }

    private void SetDisplayName() 
    {
        var request = new UpdateUserTitleDisplayNameRequest 
        {
            DisplayName = displayNameInputField.text
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnSetDisplayNameResponse, OnError);
    }

    private void OnSetDisplayNameResponse(UpdateUserTitleDisplayNameResult result) 
    {
        displayNamePanel.transform.parent.gameObject.SetActive(false);
        displayNamePanel.gameObject.SetActive(false);
    }

    #endregion

    #region Leaderboard

    public void DisplayLeaderboard() 
    {
        GetLeaderboardAroundPlayer();
    }

    private void GetLeaderboard(int startPosition) 
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "RecordTime",
            StartPosition = startPosition,
            MaxResultsCount = maxRowsToShow
        };

        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardResponse, OnError);
    }

    private void OnGetLeaderboardResponse(GetLeaderboardResult result) 
    {
        if (result.Leaderboard.Count == 0)
        {
            StartCoroutine(GameManager.instance.GetErrorUI().ShowErrorMessage("No more entries available in our leaderboard"));

            return;
        }

        foreach (Transform item in playerScoresContainer)
            Destroy(item.gameObject);

        foreach (var item in result.Leaderboard)
        {
            PlayerScoreRow newRow = Instantiate(rowPrefab, playerScoresContainer).GetComponent<PlayerScoreRow>();

            newRow.RankingText.text = (item.Position + 1).ToString();
            newRow.NameText.text = item.DisplayName;
            newRow.ScoreText.text = FloatToTimeMillisecondsFormat((float)-item.StatValue / 1000);

            if (item.DisplayName.Equals(displayName))
                newRow.HighlightRow();
        }
    }

    private void GetLeaderboardAroundPlayer() 
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "RecordTime",
            MaxResultsCount = maxRowsToShow
        };

        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnGetLeaderboardAroundPlayerResponse, OnError);
    }

    private void OnGetLeaderboardAroundPlayerResponse(GetLeaderboardAroundPlayerResult result) 
    {
        foreach (Transform item in playerScoresContainer)
            Destroy(item.gameObject);

        int minPosition = int.MaxValue;

        foreach(var item in result.Leaderboard)
        {
            PlayerScoreRow newRow = Instantiate(rowPrefab, playerScoresContainer).GetComponent<PlayerScoreRow>();

            newRow.RankingText.text = (item.Position + 1).ToString();
            newRow.NameText.text = item.DisplayName;
            newRow.ScoreText.text = FloatToTimeMillisecondsFormat((float)-item.StatValue / 1000);

            if (item.DisplayName.Equals(displayName))
                newRow.HighlightRow();

            if(item.Position < minPosition) 
                minPosition = item.Position;
        }

        minRankingShowed = minPosition;

        leaderboardPanel.transform.parent.gameObject.SetActive(true);
        leaderboardPanel.gameObject.SetActive(true);
    }

    #endregion

    #region API Common

    private void OnError(PlayFabError error) 
    {
        StartCoroutine(GameManager.instance.GetErrorUI().ShowErrorMessage(error.ErrorMessage));

        leaderboardPanel.transform.parent.gameObject.SetActive(false);
        leaderboardPanel.gameObject.SetActive(false);
    }

    #endregion
}
