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

    private void Start()
    {
        GetVirtualCurrencies();
    }

    #region Get

    public void GetVirtualCurrencies()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnError);
    }

    private void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {
        Coins = result.VirtualCurrency["SC"];
        Rubies = result.VirtualCurrency["HC"];
    }

    #endregion

    private void OnError(PlayFabError error)
    {
        GameManager.instance.GetSFXManager().PlaySound(Config.WRONG_SFX);

        Debug.Log("Error: " + error.ErrorMessage);
    }

}
