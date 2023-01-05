using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Upgrade
{
    public enum HandUpgrade
    {
        SecondHand,
        ThirdHand
    }

    public enum TemperatureUpgrade
    {
        FirstLevel,
        SecondLevel,
        ThirdLevel
    }

    public static int GetHandUpgradeValue(HandUpgrade handUpgrade)
    {
        int handCountValue = 1;

        switch (handUpgrade)
        {
            case HandUpgrade.SecondHand:
                handCountValue = 2;
                break;

            case HandUpgrade.ThirdHand:
                handCountValue = 3; 
                break;
        }

        return handCountValue;
    }

    public static float GetTemperatureUpgradeValue(TemperatureUpgrade temperatureUpgrade)
    {
        float temperatureMultiplierValue = 0f;

        switch (temperatureUpgrade)
        {
            case TemperatureUpgrade.FirstLevel:
                temperatureMultiplierValue = 1.2f;
                break;

            case TemperatureUpgrade.SecondLevel:
                temperatureMultiplierValue = 1.5f;
                break;

            case TemperatureUpgrade.ThirdLevel:
                temperatureMultiplierValue = 2f;
                break;
        }

        return temperatureMultiplierValue;
    }
}
