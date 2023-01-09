using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Utils;

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
    [SerializeField] private GameObject maximumMultiplierReachedPanel;

    [SerializeField] private GameObject multiplierPurchasePanel;
    [SerializeField] private Text multiplierUpgradeCoinCostText;
    [SerializeField] private Text multiplierUpgradeRubyCostText;

    [SerializeField] private GameObject multiplierFastForwardPanel;
    [SerializeField] private Text multiplierFastForwardRubyCostText;
    [SerializeField] private Text waitingTimeMultiplierUpgradeFFText;


    #endregion

    #region Logic Variables

    private float waitingDurationHandUpgrade = -1;
    private float waitingDurationMultiplierUpgrade = -1;

    #endregion

    private void Awake()
    {
        progressManager = GameManager.instance.GetProgressManager();
        currencyManager = GameManager.instance.GetCurrencyManager();

        handUpgradeTable = Resources.Load(Config.HAND_UPGRADES_FILE) as UpgradeTable;
        multiplierUpgradeTable = Resources.Load(Config.MULTIPLIER_UPGRADES_FILE) as UpgradeTable;
    }

    private void Update()
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
                if (CheckToRedeemHandUpgrade())
                {
                    progressManager.RedeemNewHandUpgrade();
                    progressManager.ResetNewHandUpgradeRedeemTimeAPICall();

                    DisplayProgressInShop();
                }
            }
        }
        else
            waitingDurationHandUpgrade = -1;


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
                if (CheckToRedeemMultiplierUpgrade())
                {
                    progressManager.RedeemNewMultiplierUpgrade();
                    progressManager.ResetNewMultiplierUpgradeRedeemTimeAPICall();

                    DisplayProgressInShop();
                }
            }
        }
        else
            waitingDurationMultiplierUpgrade = -1;
    }

    public void DisplayProgressInShop()
    {
        progressManager.UpdateCountersAPICall();
        currencyManager.GetVirtualCurrencies();

        UpdateHandUpgradeWaitingDuration();
        UpdateMultiplierUpgradeWaitingDuration();

        DisplayHandUpgradeButtons();
        DisplaySlapMultiplierUpgradeButtons();

        coinsCountText.text = currencyManager.Coins.ToString();
        rubiesCountText.text = currencyManager.Rubies.ToString();
        handsCountText.text = progressManager.HandsCount.ToString();
        slapMultiplierText.text = "x" + progressManager.TemperatureIncreaseMultiplier.ToString();
    }

    #region Hands Count Upgrades

    private void DisplayHandUpgradeButtons()
    {
        if (progressManager.HandUpgradesDone < 2)
        {
            if (waitingDurationHandUpgrade > 0)
            {
                waitHandButton.gameObject.SetActive(true);

                buyHandButton.gameObject.SetActive(false);
                maximumHandsReachedPanel.SetActive(false);
            }
            else
            {
                buyHandButton.gameObject.SetActive(true);

                waitHandButton.gameObject.SetActive(false);
                maximumHandsReachedPanel.SetActive(false);
            }
        }
        else
        {
            maximumHandsReachedPanel.SetActive(true);

            buyHandButton.gameObject.SetActive(false);
            waitHandButton.gameObject.SetActive(false);
        }
    }

    public void DisplayHandPurchasePanelIfAvailable()
    {
        progressManager.UpdateCountersAPICall();
        int handsCount = progressManager.HandsCount;

        if (handsCount < 3)
        {
            int handUpgradesDone = progressManager.HandUpgradesDone;

            handUpgradeCoinCostText.text = (handUpgradeTable.UpgradeInfo[handUpgradesDone].CoinPrice).ToString();
            handUpgradeRubyCostText.text = (handUpgradeTable.UpgradeInfo[handUpgradesDone].RubyPrice).ToString();

            handPurchasePanel.SetActive(true);
        }
        else
        {
            GameManager.instance.GetSFXManager().PlaySound(Config.WRONG_SFX);

            buyHandButton.gameObject.SetActive(false);
            maximumHandsReachedPanel.gameObject.SetActive(true);
        }
    }

    public void DisplayHandUpgradeFastForwardPanelIfAvailable()
    {
        progressManager.UpdateCountersAPICall();
        int handsCount = progressManager.HandsCount;

        if (handsCount < 3)
        {
            int handUpgradesDone = progressManager.HandUpgradesDone;

            handFastForwardRubyCostText.text = (handUpgradeTable.UpgradeInfo[handUpgradesDone].FastForwardRubyPrice).ToString();

            handFastForwardPanel.SetActive(true);
        }
        else
        {
            GameManager.instance.GetSFXManager().PlaySound(Config.WRONG_SFX);

            waitHandButton.gameObject.SetActive(false);
            maximumHandsReachedPanel.gameObject.SetActive(true);
        }
    }

    public void CoinBuyHandUpgrade()
    {
        progressManager.UpdateCountersAPICall();
        currencyManager.GetVirtualCurrencies();

        int coinsOwned = currencyManager.Coins;

        int handUpgradesDone = progressManager.HandUpgradesDone;
        int handUpgradeCoinPrice = handUpgradeTable.UpgradeInfo[handUpgradesDone].CoinPrice;

        #region TO DO: CHANGE THIS FOR AN API CALL TO A CUSTOM CLOUDSCRIPT METHOD

        // TO DO: HERE JUST CHANGE THE WAY OF CHECKING IF WE HAVE ENOUGH MONEY

        /*
        if (coinsOwned >= handUpgradeCoinPrice)
        {
            currencyManager.SubstractCoins(handUpgradeCoinPrice);

            float waitingTime = handUpgradeTable.UpgradeInfo[handUpgradesDone].WaitingTimeInSeconds;
            progressManager.WriteNewHandUpgradeRedeemTimeAPICall(waitingTime);
            waitingDurationHandUpgrade = waitingTime;

            handPurchasePanel.SetActive(false);
            DisplayProgressInShop();
        }
        else
        {
            GameManager.instance.GetSFXManager().PlaySound(Config.WRONG_SFX);
        }
        */

        #endregion
    }

    public void RubyBuyHandUpgrade()
    {
        progressManager.UpdateCountersAPICall();
        currencyManager.GetVirtualCurrencies();

        int rubiesOwned = currencyManager.Rubies;

        int handUpgradesDone = progressManager.HandUpgradesDone;
        int handUpgradeRubyPrice = handUpgradeTable.UpgradeInfo[handUpgradesDone].RubyPrice;

        #region TO DO: CHANGE THIS IF TO BE BASED ON A CUSTOM CLOUDSCRIPT METHOD
        /*
        if (rubiesOwned >= handUpgradeRubyPrice)
        {
            currencyManager.SubstractRubies(handUpgradeRubyPrice);
            progressManager.RedeemNewHandUpgrade();

            handPurchasePanel.SetActive(false);
            DisplayProgressInShop();
        }
        else
        {
            GameManager.instance.GetSFXManager().PlaySound(Config.WRONG_SFX);
        }
        */
        #endregion
    }

    public void RubyFastForwardHandUpgrade()
    {
        progressManager.UpdateCountersAPICall();
        currencyManager.GetVirtualCurrencies();
        UpdateHandUpgradeWaitingDuration();

        if (waitingDurationHandUpgrade < 0)
        {
            handFastForwardPanel.SetActive(false);
            DisplayHandUpgradeButtons();
        }

        int rubiesOwned = currencyManager.Rubies;

        int handUpgradesDone = progressManager.HandUpgradesDone;
        int handFastForwardRubyPrice = handUpgradeTable.UpgradeInfo[handUpgradesDone].FastForwardRubyPrice;

        #region TO DO: CHANGE THIS IF TO BE CHECKED BASED ON A CUSTOM CLOUDSCRIPT METHOD
        /*
        if (rubiesOwned >= handFastForwardRubyPrice)
        {
            currencyManager.SubstractRubies(handFastForwardRubyPrice);
            progressManager.RedeemNewHandUpgrade();

            progressManager.ResetNewHandUpgradeRedeemTimeAPICall();

            handFastForwardPanel.SetActive(false);
            DisplayProgressInShop();
        }
        else
        {
            GameManager.instance.GetSFXManager().PlaySound(Config.WRONG_SFX);
        }
        */
        #endregion
    }

    private void UpdateHandUpgradeWaitingDuration()
    {
        float currentServerTime = progressManager.GetServerTimeAPICall();

        float redeemTime = progressManager.GetNewHandUpgradeRedeemTimeAPICall();

        if (redeemTime < 0)
        {
            waitingDurationHandUpgrade = -1;
            return;
        }

        float newDuration = redeemTime - currentServerTime;

        if (newDuration > 0)
            waitingDurationHandUpgrade = newDuration;
        else
            waitingDurationHandUpgrade = -1;
    }

    private bool CheckToRedeemHandUpgrade()
    {
        // TO DO: CHANGE THIS TO BE THE RESULT OF A CLOUDSCRIPT METHOD

        UpdateHandUpgradeWaitingDuration();

        if (waitingDurationHandUpgrade == -1)
            return true;
        else
            return false;
    }

    #endregion

    #region Slap Multiplier Upgrades

    private void DisplaySlapMultiplierUpgradeButtons()
    {
        if (progressManager.MultiplierUpgradesDone < 3)
        {
            if (waitingDurationMultiplierUpgrade > 0)
            {
                waitMultiplierButton.gameObject.SetActive(true);

                buyMultiplierButton.gameObject.SetActive(false);
                maximumMultiplierReachedPanel.gameObject.SetActive(false);
            }
            else
            {
                buyMultiplierButton.gameObject.SetActive(true);

                waitMultiplierButton.gameObject.SetActive(false);
                maximumMultiplierReachedPanel.gameObject.SetActive(false);
            }
        }
        else
        {
            maximumMultiplierReachedPanel.gameObject.SetActive(true);

            buyMultiplierButton.gameObject.SetActive(false);
            waitMultiplierButton.gameObject.SetActive(false);
        }
    }

    public void DisplayMultiplierPurchasePanelIfAvailable()
    {
        progressManager.UpdateCountersAPICall();
        int multiplierUpgradesDone = progressManager.MultiplierUpgradesDone;

        if (multiplierUpgradesDone < 3)
        {
            multiplierUpgradeCoinCostText.text = (multiplierUpgradeTable.UpgradeInfo[multiplierUpgradesDone].CoinPrice).ToString();
            multiplierUpgradeRubyCostText.text = (multiplierUpgradeTable.UpgradeInfo[multiplierUpgradesDone].RubyPrice).ToString();

            multiplierPurchasePanel.SetActive(true);
        }
        else
        {
            GameManager.instance.GetSFXManager().PlaySound(Config.WRONG_SFX);

            buyMultiplierButton.gameObject.SetActive(false);
            maximumMultiplierReachedPanel.gameObject.SetActive(true);
        }
    }

    public void DisplayMultiplierUpgradeFastForwardPanelIfAvailable()
    {
        progressManager.UpdateCountersAPICall();
        int multiplierUpgradesDone = progressManager.MultiplierUpgradesDone;

        if (multiplierUpgradesDone < 3)
        {
            multiplierFastForwardRubyCostText.text = (multiplierUpgradeTable.UpgradeInfo[multiplierUpgradesDone].FastForwardRubyPrice).ToString();

            multiplierFastForwardPanel.SetActive(true);
        }
        else
        {
            GameManager.instance.GetSFXManager().PlaySound(Config.WRONG_SFX);

            waitMultiplierButton.gameObject.SetActive(false);
            maximumMultiplierReachedPanel.gameObject.SetActive(true);
        }
    }

    public void CoinBuyMultiplierUpgrade()
    {
        progressManager.UpdateCountersAPICall();
        currencyManager.GetVirtualCurrencies();

        int coinsOwned = currencyManager.Coins;

        int multiplierUpgradesDone = progressManager.MultiplierUpgradesDone;
        int multiplierUpgradeCoinPrice = multiplierUpgradeTable.UpgradeInfo[multiplierUpgradesDone].CoinPrice;

        #region TO DO: DO THIS IF CHECK VIA A CLOUDSCRIPT METHOD
        /*
        if (coinsOwned >= multiplierUpgradeCoinPrice)
        {
            currencyManager.SubstractCoins(multiplierUpgradeCoinPrice);

            float waitingTime = multiplierUpgradeTable.UpgradeInfo[multiplierUpgradesDone].WaitingTimeInSeconds;
            progressManager.WriteNewMultiplierUpgradeRedeemTimeAPICall(waitingTime);
            waitingDurationMultiplierUpgrade = waitingTime;

            multiplierPurchasePanel.SetActive(false);
            DisplayProgressInShop();
        }
        else
        {
            GameManager.instance.GetSFXManager().PlaySound(Config.WRONG_SFX);
        }
        */
        #endregion
    }

    public void RubyBuyMultiplierUpgrade()
    {
        progressManager.UpdateCountersAPICall();
        currencyManager.GetVirtualCurrencies();

        int rubiesOwned = currencyManager.Rubies;

        int multiplierUpgradesDone = progressManager.MultiplierUpgradesDone;
        int multiplierUpgradeRubyPrice = multiplierUpgradeTable.UpgradeInfo[multiplierUpgradesDone].RubyPrice;

        #region TO DO: DO THIS IF CHECK VIA A CLOUDSCRIPT METHOD
        /*
        if (rubiesOwned >= multiplierUpgradeRubyPrice)
        {
            currencyManager.SubstractRubies(multiplierUpgradeRubyPrice);
            progressManager.RedeemNewMultiplierUpgrade();

            multiplierPurchasePanel.SetActive(false);
            DisplayProgressInShop();
        }
        else
        {
            GameManager.instance.GetSFXManager().PlaySound(Config.WRONG_SFX);
        }
        */
        #endregion
    }

    public void RubyFastForwardMultiplierUpgrade()
    {
        progressManager.UpdateCountersAPICall();
        currencyManager.GetVirtualCurrencies();

        UpdateMultiplierUpgradeWaitingDuration();

        if (waitingDurationMultiplierUpgrade < 0)
        {
            multiplierFastForwardPanel.SetActive(false);
            DisplaySlapMultiplierUpgradeButtons();
        }

        int rubiesOwned = currencyManager.Rubies;

        int multiplierUpgradesDone = progressManager.MultiplierUpgradesDone;
        int multiplierFastForwardRubyPrice = multiplierUpgradeTable.UpgradeInfo[multiplierUpgradesDone].FastForwardRubyPrice;

        #region TO DO: CHANGE THIS IF CHECK TO BE BASED ON A CLOUDSCRIPT METHOD
        /*
        if (rubiesOwned >= multiplierFastForwardRubyPrice)
        {
            currencyManager.SubstractRubies(multiplierFastForwardRubyPrice);
            progressManager.RedeemNewMultiplierUpgrade();

            progressManager.ResetNewMultiplierUpgradeRedeemTimeAPICall();

            multiplierFastForwardPanel.SetActive(false);
            DisplayProgressInShop();
        }
        else
        {
            GameManager.instance.GetSFXManager().PlaySound(Config.WRONG_SFX);
        }
        */
        #endregion
    }

    private void UpdateMultiplierUpgradeWaitingDuration()
    {
        float currentServerTime = progressManager.GetServerTimeAPICall();

        float redeemTime = progressManager.GetNewMultiplierUpgradeRedeemTimeAPICall();

        if (redeemTime < 0)
        {
            waitingDurationMultiplierUpgrade = -1;
            return;
        }

        float newDuration = redeemTime - currentServerTime;

        if (newDuration > 0)
            waitingDurationMultiplierUpgrade = newDuration;
        else
            waitingDurationMultiplierUpgrade = -1;
    }

    private bool CheckToRedeemMultiplierUpgrade()
    {
        // TO DO: CHANGE THIS TO BE BASED ON A CLOUDSCRIPT METHOD

        UpdateMultiplierUpgradeWaitingDuration();

        if (waitingDurationMultiplierUpgrade == -1)
            return true;
        else
            return false;
    }

    #endregion
}
