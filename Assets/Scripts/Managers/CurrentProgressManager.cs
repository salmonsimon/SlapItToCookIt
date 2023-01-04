using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class CurrentProgressManager : MonoBehaviour
{
    [SerializeField] private float timePlayed = 0f;
    [SerializeField] private int slapCount = 0;

    private bool finishedSlapping = false;
    public bool FinishedSlapping { get { return finishedSlapping; } set { finishedSlapping = value; } }

    private void Update()
    {
        if (!GameManager.instance.IsOnMainMenu() && !FinishedSlapping)
            timePlayed += Time.deltaTime;
    }

    public void IncreaseSlapCount(int value)
    {
        slapCount += value;
        GameManager.instance.GetCountersUI().UpdateCounters();
    }

    public void ResetCounters()
    {
        timePlayed = 0f;
        slapCount = 0;

        GameManager.instance.GetCountersUI().UpdateCounters();
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
