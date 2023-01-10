using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] private int coins;
    public int Coins { get { return coins; } private set { coins = value; } }

    [SerializeField] private int rubies;
    public int Rubies { get { return rubies; } private set { rubies = value; } }

    #region Events

    public delegate void OnVirtualCurrenciesUpdateStartDelegate();
    public event OnVirtualCurrenciesUpdateStartDelegate OnVirtualCurrenciesUpdateStart;

    public delegate void OnVirtualCurrenciesUpdateEndDelegate();
    public event OnVirtualCurrenciesUpdateEndDelegate OnVirtualCurrenciesUpdateEnd;

    #endregion

    private void Start()
    {
        GetVirtualCurrencies();
    }

    public void GetVirtualCurrencies()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnError);

        if (OnVirtualCurrenciesUpdateStart != null)
            OnVirtualCurrenciesUpdateStart();
    }

    private void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {
        Coins = result.VirtualCurrency[Config.API_COINS_ID];
        Rubies = result.VirtualCurrency[Config.API_RUBIES_ID];

        if (OnVirtualCurrenciesUpdateEnd != null)
            OnVirtualCurrenciesUpdateEnd();
    }

    private void OnError(PlayFabError error)
    {
        GameManager.instance.GetSFXManager().PlaySound(Config.WRONG_SFX);

        StartCoroutine(GameManager.instance.GetErrorUI().ShowErrorMessage(error.ErrorMessage));

        if (OnVirtualCurrenciesUpdateEnd != null)
            OnVirtualCurrenciesUpdateEnd();
    }
}
