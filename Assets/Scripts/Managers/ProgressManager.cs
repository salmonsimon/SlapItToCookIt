using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.UIElements;
using UnityEngine;
using static Upgrade;

public class ProgressManager : MonoBehaviour
{
    #region Upgrades

    [Header("Upgrades")]
    [SerializeField] private int handsCount = 1;
    public int HandsCount { get { return handsCount; } private set { handsCount = value; } }

    [SerializeField] private float temperatureIncreaseMultiplier = 1.0f;
    public float TemperatureIncreaseMultiplier { get { return temperatureIncreaseMultiplier; } private set { temperatureIncreaseMultiplier = value; } }

    #endregion

    #region Score Record

    [Header("Score Record")]
    [SerializeField] private float recordTime = -1f;
    public float RecordTime { get { return recordTime; } private set { recordTime = value; } }

    [SerializeField] private int recordSlaps = -1;
    public int RecordSlaps { get { return recordSlaps; } private set { recordSlaps = value; } }

    #endregion

    #region Events

    public delegate void OnProgressUpdateStartDelegate();
    public event OnProgressUpdateStartDelegate OnProgressUpdateStart;

    public delegate void OnProgressUpdateEndDelegate();
    public event OnProgressUpdateEndDelegate OnProgressUpdateEnd;

    #endregion

    private void Start()
    {
        UpdateProgress();
    }

    public void SetProgress(int handsCount, float temperatureIncreaseMultiplier, int recordSlaps, float recordtime)
    {
        HandsCount = handsCount;
        TemperatureIncreaseMultiplier = temperatureIncreaseMultiplier;

        RecordSlaps = recordSlaps;
        RecordTime = recordtime;

        if (OnProgressUpdateEnd != null)
            OnProgressUpdateEnd();
    }

    #region API Calls

    public void UpdateProgress()
    {
        PlayFabClientAPI.GetUserReadOnlyData(new GetUserDataRequest(), OnGetUserReadOnlyDataResult, OnError);

        if (OnProgressUpdateStart != null)
            OnProgressUpdateStart();
    }

    private void OnGetUserReadOnlyDataResult(GetUserDataResult result)
    {
        var data = result.Data;

        if (data.ContainsKey(Config.API_PLAYER_DATA_INITIALIZED_KEY))
        {
            SetProgress(Int32.Parse(data[Config.API_PLAYER_DATA_HANDS_COUNT_KEY].Value), float.Parse(data[Config.API_PLAYER_DATA_MULTIPLIER_KEY].Value),
                           Int32.Parse(data[Config.API_PLAYER_DATA_RECORD_SLAPS_KEY].Value), float.Parse(data[Config.API_PLAYER_DATA_RECORD_TIME_KEY].Value));
        }
        else
        {
            StartCoroutine(GameManager.instance.GetErrorUI().ShowErrorMessage(Config.API_PLAYER_NOT_INITIALIZED_MSG));
        }
    }

    private void OnError(PlayFabError error)
    {
        StartCoroutine(GameManager.instance.GetErrorUI().ShowErrorMessage(error.ErrorMessage));

        if (OnProgressUpdateEnd != null)
            OnProgressUpdateEnd();
    }

    #endregion
}
