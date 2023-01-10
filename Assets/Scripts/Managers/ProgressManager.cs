using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

        if (data.ContainsKey("Initialized"))
        {
            SetProgress(Int32.Parse(data["HandsCount"].Value), float.Parse(data["TemperatureIncreaseMultiplier"].Value),
                           Int32.Parse(data["RecordSlaps"].Value), float.Parse(data["RecordTime"].Value));
        }
        else
        {
            Debug.Log("Player not initialized");
        }
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log(error.ErrorMessage);

        if (OnProgressUpdateEnd != null)
            OnProgressUpdateEnd();
    }

    #endregion

    public void SetProgress(int handsCount, float temperatureIncreaseMultiplier,
                            int recordSlaps, float recordtime)
    {
        HandsCount = handsCount;
        TemperatureIncreaseMultiplier = temperatureIncreaseMultiplier;

        RecordSlaps = recordSlaps;
        RecordTime = recordtime;

        if (OnProgressUpdateEnd != null)
            OnProgressUpdateEnd();
    }
}
