using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Utils;
using PlayFab.Json;
using System.Globalization;

public class ShopUI : MonoBehaviour
{
    #region Game Objects References

    private ProgressManager progressManager;
    private CurrencyManager currencyManager;

    #endregion

    #region Upgrade Tables Resources

    private UpgradeTable handUpgradeTable;
    private UpgradeTable multiplierUpgradeTable;

    #endregion

    #region Progress Counters UI References

    [Header("Progress Counters")]
    [SerializeField] private Text coinsCountText;
    [SerializeField] private Text rubiesCountText;
    [SerializeField] private Text handsCountText;
    [SerializeField] private Text slapMultiplierText;

    #endregion

    #region Hands Upgrades UI References

    [Header("Hands Upgrades")]
    [SerializeField] private Button buyHandButton;
    [SerializeField] private Button waitHandButton;
    [SerializeField] private Text waitingTimeHandButtonText;
    [SerializeField] private Button redeemHandButton;
    [SerializeField] private GameObject maximumHandsReachedPanel;

    [SerializeField] private GameObject handPurchasePanel;
    [SerializeField] private Text handUpgradeCoinCostText;
    [SerializeField] private Text handUpgradeRubyCostText;

    [SerializeField] private GameObject handFastForwardPanel;
    [SerializeField] private Text handFastForwardRubyCostText;
    [SerializeField] private Text waitingTimeHandUpgradeFFText;

    #endregion

    #region Slap Upgrades UI References

    [Header("Slap Upgrades")]
    [SerializeField] private Button buyMultiplierButton;
    [SerializeField] private Button waitMultiplierButton;
    [SerializeField] private Text waitingTimeMultiplierButtonText;
    [SerializeField] private Button redeemMultiplierButton;
    [SerializeField] private GameObject maximumMultiplierReachedPanel;

    [SerializeField] private GameObject multiplierPurchasePanel;
    [SerializeField] private Text multiplierUpgradeCoinCostText;
    [SerializeField] private Text multiplierUpgradeRubyCostText;

    [SerializeField] private GameObject multiplierFastForwardPanel;
    [SerializeField] private Text multiplierFastForwardRubyCostText;
    [SerializeField] private Text waitingTimeMultiplierUpgradeFFText;


    #endregion

    #region Errors UI References

    [Header("Error Management")]
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private Text errorText;

    #endregion

    #region Loading UI References

    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingPanel;

    #endregion

    #region Logic Variables

    private bool waitingForHandUpgrade = false;
    private bool waitingForMultiplierUpgrade = false;

    private float waitingDurationHandUpgrade = -1;
    private float waitingDurationMultiplierUpgrade = -1;

    private int multiplierUpgradesDone = 0;

    private int updatesOnCourse = 0;

    #endregion

    private void Awake()
    {
        progressManager = GameManager.instance.GetProgressManager();
        currencyManager = GameManager.instance.GetCurrencyManager();

        handUpgradeTable = Resources.Load(Config.HAND_UPGRADES_FILE) as UpgradeTable;
        multiplierUpgradeTable = Resources.Load(Config.MULTIPLIER_UPGRADES_FILE) as UpgradeTable;
    }

    private void OnEnable()
    {
        UpdateHandUpgradeWaitingDuration();
        UpdateMultiplierUpgradeWaitingDuration();

        progressManager.OnProgressUpdateStart += OnUpdateStart;
        progressManager.OnProgressUpdateEnd += OnUpdateFinish;

        currencyManager.OnVirtualCurrenciesUpdateStart += OnUpdateStart;
        currencyManager.OnVirtualCurrenciesUpdateEnd += OnUpdateFinish;

        GetMultiplierUpgradesDone();
    }

    private void OnDisable()
    {
        progressManager.OnProgressUpdateStart -= OnUpdateStart;
        progressManager.OnProgressUpdateEnd -= OnUpdateFinish;

        currencyManager.OnVirtualCurrenciesUpdateStart -= OnUpdateStart;
        currencyManager.OnVirtualCurrenciesUpdateEnd -= OnUpdateFinish;
    }

    private void Update()
    {
        if (waitingForHandUpgrade)
        {
            if (waitingDurationHandUpgrade > 0)
            {
                waitingDurationHandUpgrade -= Time.deltaTime;

                if (waitingDurationHandUpgrade > 0)
                {
                    waitingTimeHandButtonText.text = FloatToTimeFormat(waitingDurationHandUpgrade);
                    waitingTimeHandUpgradeFFText.text = FloatToTimeFormat(waitingDurationHandUpgrade);
                }
                else
                {
                    DisplayHandUpgradeButtons();
                    waitingDurationHandUpgrade = -1;
                }
            }
        }

        if (waitingForMultiplierUpgrade)
        {
            if (waitingDurationMultiplierUpgrade > 0)
            {
                waitingDurationMultiplierUpgrade -= Time.deltaTime;

                if (waitingDurationMultiplierUpgrade > 0)
                {
                    waitingTimeMultiplierButtonText.text = FloatToTimeFormat(waitingDurationMultiplierUpgrade);
                    waitingTimeMultiplierUpgradeFFText.text = FloatToTimeFormat(waitingDurationMultiplierUpgrade);
                }
                else
                {
                    DisplayMultiplierUpgradeButtons();
                    waitingDurationHandUpgrade = -1;
                }
            }
        }
    }

    private void GetMultiplierUpgradesDone()
    {
        switch (progressManager.TemperatureIncreaseMultiplier)
        {
            case Config.BASE_MULTIPLIER:
                multiplierUpgradesDone = 0;
                break;
            case Config.MULTIPLIER_UPGRADE_1:
                multiplierUpgradesDone = 1;
                break;
            case Config.MULTIPLIER_UPGRADE_2:
                multiplierUpgradesDone = 2;
                break;
            case Config.MULTIPLIER_UPGRADE_3:
                multiplierUpgradesDone = 3;
                break;
        }
    }

    #region Shop UI

    public void DisplayProgressInShop()
    {
        DisplayHandUpgradeButtons();
        DisplayMultiplierUpgradeButtons();

        coinsCountText.text = currencyManager.Coins.ToString();
        rubiesCountText.text = currencyManager.Rubies.ToString();
        handsCountText.text = progressManager.HandsCount.ToString();
        slapMultiplierText.text = "x" + progressManager.TemperatureIncreaseMultiplier.ToString();
    }

    private void OnUpdateStart()
    {
        updatesOnCourse++;

        loadingPanel.SetActive(true);
    }

    private void OnUpdateFinish()
    {
        updatesOnCourse--;

        if (updatesOnCourse <= 0)
        {
            GetMultiplierUpgradesDone();
            DisplayProgressInShop();
            loadingPanel.SetActive(false);

            if (updatesOnCourse < 0)
            {
                updatesOnCourse = 0;
            }
        }
    }

    #endregion

    #region Hand Upgrades

    #region UI

    private void DisplayHandUpgradeButtons()
    {
        if (progressManager.HandsCount < Config.MAX_HAND_COUNT)
        {
            if (waitingForHandUpgrade && waitingDurationHandUpgrade > 0)
            {
                waitHandButton.gameObject.SetActive(true);

                buyHandButton.gameObject.SetActive(false);
                redeemHandButton.gameObject.SetActive(false);
                maximumHandsReachedPanel.SetActive(false);
            }
            else if (waitingForHandUpgrade && waitingDurationHandUpgrade < 0)
            {
                redeemHandButton.gameObject.SetActive(true);

                buyHandButton.gameObject.SetActive(false);
                waitHandButton.gameObject.SetActive(false);
                maximumHandsReachedPanel.SetActive(false);
            }
            else 
            {
                buyHandButton.gameObject.SetActive(true);

                waitHandButton.gameObject.SetActive(false);
                redeemHandButton.gameObject.SetActive(false);
                maximumHandsReachedPanel.SetActive(false);
            }
        }
        else
        {
            maximumHandsReachedPanel.SetActive(true);

            buyHandButton.gameObject.SetActive(false);
            waitHandButton.gameObject.SetActive(false);
            redeemHandButton.gameObject.SetActive(false);
        }
    }

    public void DisplayHandPurchasePanelIfAvailable()
    {
        int handsCount = progressManager.HandsCount;

        if (handsCount < Config.MAX_HAND_COUNT)
        {
            int handUpgradesDone = handsCount - 1;

            handUpgradeCoinCostText.text = (handUpgradeTable.UpgradeInfo[handUpgradesDone].CoinPrice).ToString();
            handUpgradeRubyCostText.text = (handUpgradeTable.UpgradeInfo[handUpgradesDone].RubyPrice).ToString();

            handPurchasePanel.SetActive(true);
        }
        else
        {
            GameManager.instance.GetSFXManager().PlaySound(Config.WRONG_SFX);

            DisplayHandUpgradeButtons();
        }
    }

    public void DisplayHandUpgradeFastForwardPanelIfAvailable()
    {
        int handsCount = progressManager.HandsCount;

        if (handsCount < Config.MAX_HAND_COUNT)
        {
            int handUpgradesDone = handsCount - 1;

            handFastForwardRubyCostText.text = (handUpgradeTable.UpgradeInfo[handUpgradesDone].FastForwardRubyPrice).ToString();

            handFastForwardPanel.SetActive(true);
        }
        else
        {
            GameManager.instance.GetSFXManager().PlaySound(Config.WRONG_SFX);

            DisplayHandUpgradeButtons();
        }
    }

    #endregion

    #region HAND UPGRADE BUTTONS

    public void CoinBuyHandUpgradeButton()
    {
        CoinBuyHandUpgrade();
    }

    public void RubyBuyHandUpgradeButton() 
    {
        RubyBuyHandUpgrade();
    }

    public void RubyFastForwardHandUpgradeButton()
    {
        RubyFastForwardHandUpgrade();
    }

    public void RedeemHandUpgradeButton()
    {
        RedeemHandUpgrade();
    }

    #endregion

    #region COIN BUY HAND UPGRADE

    private void CoinBuyHandUpgrade() 
    {
        var initializePlayerRequest = new ExecuteCloudScriptRequest()
        {
            FunctionName = Config.API_BUY_HAND_UPGRADE_COINS_FUNCTION_NAME,
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(initializePlayerRequest, OnCoinBuyHandUpgradeResponse, OnError);
    }

    private void OnCoinBuyHandUpgradeResponse(ExecuteCloudScriptResult result)
    {
        var lastLog = result.Logs[result.Logs.Count - 1];

        if (lastLog.Level == "Error")
        {
            StartCoroutine(GameManager.instance.GetErrorUI().ShowErrorMessage(lastLog.Message));

        }
        else
        {
            float waitingTime = float.Parse(Utils.ToDictionary(lastLog.Data)["waitingTime"]);

            waitingForHandUpgrade = true;
            waitingDurationHandUpgrade = waitingTime;

            handPurchasePanel.SetActive(false);

            GameManager.instance.GetCurrencyManager().GetVirtualCurrencies();
        }
    }

    #endregion

    #region RUBY BUY HAND UPGRADE

    private void RubyBuyHandUpgrade()
    {
        var initializePlayerRequest = new ExecuteCloudScriptRequest()
        {
            FunctionName = Config.API_BUY_HAND_UPGRADE_RUBIES_FUNCTION_NAME,
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(initializePlayerRequest, OnRubyBuyHandUpgradeResponse, OnError);
    }

    private void OnRubyBuyHandUpgradeResponse(ExecuteCloudScriptResult result)
    {
        var lastLog = result.Logs[result.Logs.Count - 1];

        if (lastLog.Level == "Error")
        {
            StartCoroutine(GameManager.instance.GetErrorUI().ShowErrorMessage(lastLog.Message));
        }
        else
        {
            handPurchasePanel.SetActive(false);

            GameManager.instance.GetCurrencyManager().GetVirtualCurrencies();
            GameManager.instance.GetProgressManager().UpdateProgress();
        }
    }

    #endregion

    #region RUBY FAST FORWARD HAND UPGRADE

    private void RubyFastForwardHandUpgrade()
    {
        var initializePlayerRequest = new ExecuteCloudScriptRequest()
        {
            FunctionName = Config.API_FAST_FORWARD_HAND_UPGRADE_COINS_FUNCTION_NAME,
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(initializePlayerRequest, OnRubyFastForwardHandUpgradeResponse, OnError);
    }

    private void OnRubyFastForwardHandUpgradeResponse(ExecuteCloudScriptResult result)
    {
        var lastLog = result.Logs[result.Logs.Count - 1];

        if (lastLog.Level == "Error")
        {
            StartCoroutine(GameManager.instance.GetErrorUI().ShowErrorMessage(lastLog.Message));
        }
        else
        {
            handFastForwardPanel.SetActive(false);

            waitingForHandUpgrade = false;
            waitingDurationHandUpgrade = -1;

            GameManager.instance.GetCurrencyManager().GetVirtualCurrencies();
            GameManager.instance.GetProgressManager().UpdateProgress();
        }
    }

    #endregion

    #region REDEEM HAND UPGRADE

    private void RedeemHandUpgrade() 
    {
        var initializePlayerRequest = new ExecuteCloudScriptRequest()
        {
            FunctionName = Config.API_REDEEM_HAND_UPGRADE_COINS_FUNCTION_NAME,
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(initializePlayerRequest, OnRedeemHandUpgradeResponse, OnError);
    }

    private void OnRedeemHandUpgradeResponse(ExecuteCloudScriptResult result)
    {
        var lastLog = result.Logs[result.Logs.Count - 1];

        if (lastLog.Level == "Error")
        {
            StartCoroutine(GameManager.instance.GetErrorUI().ShowErrorMessage(lastLog.Message));
        }
        else
        {
            waitingForHandUpgrade = false;
            waitingDurationHandUpgrade = -1;

            GameManager.instance.GetCurrencyManager().GetVirtualCurrencies();
            GameManager.instance.GetProgressManager().UpdateProgress();
        }
    }

    #endregion

    #region WAITING DURATION HAND UPGRADE REDEEM 

    private void UpdateHandUpgradeWaitingDuration()
    {
        var initializePlayerRequest = new ExecuteCloudScriptRequest()
        {
            FunctionName = Config.API_NEXT_HAND_REDEEM_WT_FUNCTION_NAME,
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(initializePlayerRequest, OnUpdateHandUpgradeWaitingDurationResponse, OnError);
    }

    private void OnUpdateHandUpgradeWaitingDurationResponse(ExecuteCloudScriptResult result)
    {
        var lastLog = result.Logs[result.Logs.Count - 1];

        if (lastLog.Level == "Error")
        {
            if (!lastLog.Message.Equals(Config.API_HAND_ERROR_MSG))
                StartCoroutine(GameManager.instance.GetErrorUI().ShowErrorMessage(lastLog.Message));
        }
        else
        {
            if (lastLog.Message.Equals(Config.API_HAND_SUCCESS_MSG_1))
            {
                float waitingTime = float.Parse(Utils.ToDictionary(lastLog.Data)["waitingTime"]);

                waitingForHandUpgrade = true;
                waitingDurationHandUpgrade = waitingTime;

                DisplayProgressInShop();
            }
            else if (lastLog.Message.Equals(Config.API_HAND_SUCCESS_MSG_2))
            {
                waitingForHandUpgrade = false;
                waitingDurationHandUpgrade = -1;

                GameManager.instance.GetCurrencyManager().GetVirtualCurrencies();
                GameManager.instance.GetProgressManager().UpdateProgress();
            }

        }
    }

    #endregion


    #endregion

    #region Slap Multiplier Upgrades

    #region UI

    private void DisplayMultiplierUpgradeButtons()
    {
        if (multiplierUpgradesDone < Config.MAX_MULTIPLIER_UPGRADES)
        {
            if (waitingForMultiplierUpgrade && waitingDurationMultiplierUpgrade > 0)
            {
                waitMultiplierButton.gameObject.SetActive(true);

                buyMultiplierButton.gameObject.SetActive(false);
                redeemMultiplierButton.gameObject.SetActive(false);
                maximumMultiplierReachedPanel.gameObject.SetActive(false);
            }
            else if (waitingForMultiplierUpgrade && waitingDurationMultiplierUpgrade < 0)
            {
                redeemMultiplierButton.gameObject.SetActive(true);

                buyMultiplierButton.gameObject.SetActive(false);
                waitMultiplierButton.gameObject.SetActive(false);
                maximumMultiplierReachedPanel.gameObject.SetActive(false);
            }
            else 
            { 
                buyMultiplierButton.gameObject.SetActive(true);

                waitMultiplierButton.gameObject.SetActive(false);
                redeemMultiplierButton.gameObject.SetActive(false);
                maximumMultiplierReachedPanel.gameObject.SetActive(false);
            }
        }
        else
        {
            maximumMultiplierReachedPanel.gameObject.SetActive(true);

            buyMultiplierButton.gameObject.SetActive(false);
            waitMultiplierButton.gameObject.SetActive(false);
            redeemMultiplierButton.gameObject.SetActive(false);
        }
    }

    public void DisplayMultiplierPurchasePanelIfAvailable()
    {
        if (multiplierUpgradesDone < Config.MAX_MULTIPLIER_UPGRADES)
        {
            multiplierUpgradeCoinCostText.text = (multiplierUpgradeTable.UpgradeInfo[multiplierUpgradesDone].CoinPrice).ToString();
            multiplierUpgradeRubyCostText.text = (multiplierUpgradeTable.UpgradeInfo[multiplierUpgradesDone].RubyPrice).ToString();

            multiplierPurchasePanel.SetActive(true);
        }
        else
        {
            GameManager.instance.GetSFXManager().PlaySound(Config.WRONG_SFX);

            DisplayMultiplierUpgradeButtons();
        }
    }

    public void DisplayMultiplierUpgradeFastForwardPanelIfAvailable()
    {
        if (multiplierUpgradesDone < Config.MAX_MULTIPLIER_UPGRADES)
        {
            multiplierFastForwardRubyCostText.text = (multiplierUpgradeTable.UpgradeInfo[multiplierUpgradesDone].FastForwardRubyPrice).ToString();

            multiplierFastForwardPanel.SetActive(true);
        }
        else
        {
            GameManager.instance.GetSFXManager().PlaySound(Config.WRONG_SFX);

            DisplayMultiplierUpgradeButtons();
        }
    }

    #endregion

    #region MULTIPLIER UPGRADE BUTTONS

    public void CoinBuyMultiplierUpgradeButton() 
    {
        CoinBuyMultiplierUpgrade();
    }

    public void RubyBuyMultiplierUpgradeButton()
    {
        RubyBuyMultiplierUpgrade();
    }

    public void RubyFastForwardMultiplierUpgradeButton()
    {
        RubyFastForwardMultiplierUpgrade();
    }

    public void RedeemMultiplierUpgradeButton()
    {
        RedeemMultiplierUpgrade();
    }

    #endregion

    #region COIN BUY MULTIPLIER UPGRADE

    private void CoinBuyMultiplierUpgrade()
    {
        var initializePlayerRequest = new ExecuteCloudScriptRequest()
        {
            FunctionName = Config.API_BUY_MULTIPLIER_UPGRADE_COINS_FUNCTION_NAME,
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(initializePlayerRequest, OnCoinBuyMultiplierUpgradeResponse, OnError);
    }

    private void OnCoinBuyMultiplierUpgradeResponse(ExecuteCloudScriptResult result)
    {
        var lastLog = result.Logs[result.Logs.Count - 1];

        if (lastLog.Level == "Error")
        {
            StartCoroutine(GameManager.instance.GetErrorUI().ShowErrorMessage(lastLog.Message));
        }
        else
        {
            float waitingTime = float.Parse(Utils.ToDictionary(lastLog.Data)["waitingTime"]);

            waitingForMultiplierUpgrade = true;
            waitingDurationMultiplierUpgrade = waitingTime;

            multiplierPurchasePanel.SetActive(false);

            GameManager.instance.GetCurrencyManager().GetVirtualCurrencies();
        }
    }

    #endregion

    #region RUBY BUY MULTIPLIER UPGRADE

    private void RubyBuyMultiplierUpgrade()
    {
        var initializePlayerRequest = new ExecuteCloudScriptRequest()
        {
            FunctionName = Config.API_BUY_MULTIPLIER_UPGRADE_RUBIES_FUNCTION_NAME,
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(initializePlayerRequest, OnRubyBuyMultiplierUpgradeResponse, OnError);
    }

    private void OnRubyBuyMultiplierUpgradeResponse(ExecuteCloudScriptResult result)
    {
        var lastLog = result.Logs[result.Logs.Count - 1];

        if (lastLog.Level == "Error")
        {
            StartCoroutine(GameManager.instance.GetErrorUI().ShowErrorMessage(lastLog.Message));
        }
        else
        {
            multiplierPurchasePanel.SetActive(false);

            GameManager.instance.GetCurrencyManager().GetVirtualCurrencies();
            GameManager.instance.GetProgressManager().UpdateProgress();
        }
    }

    #endregion

    #region RUBY FAST FORWARD MULTIPLIER UPGRADE

    private void RubyFastForwardMultiplierUpgrade()
    {
        var initializePlayerRequest = new ExecuteCloudScriptRequest()
        {
            FunctionName = Config.API_FAST_FORWARD_MULTIPLIER_UPGRADE_COINS_FUNCTION_NAME,
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(initializePlayerRequest, OnRubyFastForwardMultiplierUpgradeResponse, OnError);
    }

    private void OnRubyFastForwardMultiplierUpgradeResponse(ExecuteCloudScriptResult result)
    {
        var lastLog = result.Logs[result.Logs.Count - 1];

        if (lastLog.Level == "Error")
        {
            StartCoroutine(GameManager.instance.GetErrorUI().ShowErrorMessage(lastLog.Message));
        }
        else
        {
            multiplierFastForwardPanel.SetActive(false);

            waitingForMultiplierUpgrade = false;
            waitingDurationMultiplierUpgrade = -1;

            GameManager.instance.GetCurrencyManager().GetVirtualCurrencies();
            GameManager.instance.GetProgressManager().UpdateProgress();
        }
    }

    #endregion

    #region REDEEM MULTIPLIER UPGRADE

    private void RedeemMultiplierUpgrade()
    {
        var initializePlayerRequest = new ExecuteCloudScriptRequest()
        {
            FunctionName = Config.API_REDEEM_MULTIPLIER_UPGRADE_COINS_FUNCTION_NAME,
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(initializePlayerRequest, OnRedeemMultiplierUpgradeResponse, OnError);
    }

    private void OnRedeemMultiplierUpgradeResponse(ExecuteCloudScriptResult result)
    {
        var lastLog = result.Logs[result.Logs.Count - 1];

        if (lastLog.Level == "Error")
        {
            StartCoroutine(GameManager.instance.GetErrorUI().ShowErrorMessage(lastLog.Message));
        }
        else
        {
            multiplierUpgradesDone++;

            waitingForMultiplierUpgrade = false;
            waitingDurationMultiplierUpgrade = -1;

            GameManager.instance.GetCurrencyManager().GetVirtualCurrencies();
            GameManager.instance.GetProgressManager().UpdateProgress();
        }
    }

    #endregion

    #region WAITING DURATION MULTIPLIER UPGRADE REDEEM

    private void UpdateMultiplierUpgradeWaitingDuration()
    {
        var initializePlayerRequest = new ExecuteCloudScriptRequest()
        {
            FunctionName = Config.API_NEXT_MULTIPLIER_REDEEM_WT_FUNCTION_NAME,
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(initializePlayerRequest, OnUpdateMultiplierUpgradeWaitingDurationResponse, OnError);
    }

    private void OnUpdateMultiplierUpgradeWaitingDurationResponse(ExecuteCloudScriptResult result)
    {
        var lastLog = result.Logs[result.Logs.Count - 1];

        if (lastLog.Level == "Error")
        {
            if (!lastLog.Message.Equals(Config.API_MULTIPLIER_ERROR_MSG))
                StartCoroutine(GameManager.instance.GetErrorUI().ShowErrorMessage(lastLog.Message));
        }
        else
        {
            if (lastLog.Message.Equals(Config.API_MULTIPLIER_SUCCESS_MSG_1))
            {
                float waitingTime = float.Parse(Utils.ToDictionary(lastLog.Data)["waitingTime"]);

                waitingForMultiplierUpgrade = true;
                waitingDurationMultiplierUpgrade = waitingTime;

                DisplayProgressInShop();
            }
            else if (lastLog.Message.Equals(Config.API_MULTIPLIER_SUCCESS_MSG_2))
            {
                waitingForMultiplierUpgrade = false;
                waitingDurationMultiplierUpgrade = -1;

                GameManager.instance.GetCurrencyManager().GetVirtualCurrencies();
                GameManager.instance.GetProgressManager().UpdateProgress();
            }
        }
    }

    #endregion

    #endregion

    #region API COMMON

    private void OnError(PlayFabError error)
    {
        StartCoroutine(GameManager.instance.GetErrorUI().ShowErrorMessage(error.ErrorMessage));
    }

    #endregion
}
