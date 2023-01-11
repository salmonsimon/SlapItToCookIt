using System.Collections.Generic;

public class Config 
{
    #region General

    public const int COMPLETED_LEVEL_REWARD = 50;

    #endregion

    #region Camera Shake

    public const string SHAKE_FILE = "Cinemachine/6D Shake";

    public const float CAMERASHAKE_HIT_AMPLITUDE = 4f;
    public const float CAMERASHAKE_HIT_DURATION = .1f;

    #endregion

    #region Scene Names

    public const string LOGIN_SCENE_NAME = "Login";
    public const string MAIN_MENU_SCENE_NAME = "Main Menu";
    public const string MAIN_SCENE_NAME = "Main";

    #endregion

    #region API

    public const string API_TITLE_ID = "F11D5";

    #region Currency

    public const string API_COINS_KEY = "Coins";
    public const string API_COINS_ID = "SC";
    public const string API_RUBIES_KEY = "Rubies";
    public const string API_RUBIES_ID = "HC";

    #endregion

    #region Player Data

    public const string API_PLAYER_DATA_INITIALIZED_KEY = "Initialized";
    public const string API_PLAYER_DATA_HANDS_COUNT_KEY = "HandsCount";
    public const string API_PLAYER_DATA_MULTIPLIER_KEY = "TemperatureIncreaseMultiplier";
    public const string API_PLAYER_DATA_RECORD_SLAPS_KEY = "RecordSlaps";
    public const string API_PLAYER_DATA_RECORD_TIME_KEY = "RecordTime";

    #endregion

    #region Function Names

    public const string API_INITIALIZE_PLAYER_FUNCTION_NAME = "InitializePlayerIfNeeded";

    public const string API_ON_COMPLETED_LEVEL_FUNCTION_NAME = "OnCompletedLevel";

    #region Hand Upgrade Functions

    public const string API_BUY_HAND_UPGRADE_COINS_FUNCTION_NAME = "TryToBuyNextHandUpgradeWithCoins";
    public const string API_REDEEM_HAND_UPGRADE_COINS_FUNCTION_NAME = "TryToRedeemHandUpgradeBoughtWithCoins";
    public const string API_FAST_FORWARD_HAND_UPGRADE_COINS_FUNCTION_NAME = "TryToFastForwardNextHandUpgrade";
    public const string API_NEXT_HAND_REDEEM_WT_FUNCTION_NAME = "TryToGetNextHandRedeemWaitingTime";
    public const string API_BUY_HAND_UPGRADE_RUBIES_FUNCTION_NAME = "TryToBuyNextHandUpgradeWithRubies";

    #endregion

    #region Multiplier Upgrade Functions

    public const string API_BUY_MULTIPLIER_UPGRADE_COINS_FUNCTION_NAME = "TryToBuyNextMultiplierUpgradeWithCoins";
    public const string API_REDEEM_MULTIPLIER_UPGRADE_COINS_FUNCTION_NAME = "TryToRedeemMultiplierUpgradeBoughtWithCoins";
    public const string API_FAST_FORWARD_MULTIPLIER_UPGRADE_COINS_FUNCTION_NAME = "TryToFastForwardNextMultiplierUpgrade";
    public const string API_NEXT_MULTIPLIER_REDEEM_WT_FUNCTION_NAME = "TryToGetNextMultiplierRedeemWaitingTime";
    public const string API_BUY_MULTIPLIER_UPGRADE_RUBIES_FUNCTION_NAME = "TryToBuyNextMultiplierUpgradeWithRubies";

    #endregion

    #endregion

    #region Error Messages

    public const string API_HAND_ERROR_MSG = "No hand upgrade on course";

    public const string API_MULTIPLIER_ERROR_MSG = "No multiplier upgrade on course";

    public const string API_LOGIN_INVALID_ERROR_MSG = "Invalid email address or password";

    #endregion

    #region Other Messages

    public const string API_HAND_SUCCESS_MSG_1 = "Got hand redeem waiting time";
    public const string API_HAND_SUCCESS_MSG_2 = "New hand upgrade bought successfully";

    public const string API_MULTIPLIER_SUCCESS_MSG_1 = "Got multiplier redeem waiting time";
    public const string API_MULTIPLIER_SUCCESS_MSG_2 = "New multiplier upgrade redeemed successfully";

    public const string API_PLAYER_NOT_INITIALIZED_MSG = "Player not initialized";

    public const string API_PASS_TOO_SHORT_MSG = "Password must contain at least 6 characters";
    public const string API_REGISTERED_MSG = "Registered, logging in...";
    public const string API_LOGIN_MSG = "logging in...";
    public const string API_PASS_RESET_MSG = "Password reset email sent";

    #endregion

    #region Login / Register

    public const int API_MIN_PASSWORD_LENGTH = 6;
    public const int API_RANDOM_PASSWORD_LENGTH = 8;

    #endregion

    #endregion

    #region Time Delays

    public const float SMALL_DELAY = .1f;
    public const float MEDIUM_DELAY = .2f;
    public const float LARGE_DELAY = .3f;
    public const float BIG_DELAY = .5f;

    #endregion

    #region Scene Transitions
    public const string CROSSFADE_TRANSITION = "Crossfade";
    public const string CROSSFADE_START_TRIGGER = "Start";
    public const string CROSSFADE_END_TRIGGER = "End";
    public const float START_TRANSITION_DURATION = .5f;
    public const float END_TRANSITION_DURATION = 1f;

    #endregion

    #region Upgrades

    public const int MAX_HAND_COUNT = 3;
    public const int MAX_MULTIPLIER_UPGRADES = 3;
    public const float MAX_MULTIPLIER_VALUE = 2f;

    public const float BASE_MULTIPLIER = 1f;
    public const float MULTIPLIER_UPGRADE_1 = 1.2f;
    public const float MULTIPLIER_UPGRADE_2 = 1.5f;
    public const float MULTIPLIER_UPGRADE_3 = 2f;
    
    #endregion

    #region UI

    public const string ANIMATOR_SHOW_COUNTERS = "Appear";
    public const string ANIMATOR_HIDE_COUNTERS = "Disappear";

    public const float ERROR_MESSAGE_DURATION = 2f;

    public const float TEMPERATURE_UI_MAX_HEIGHT = 1225f;
    public const float TEMPERATURE_UI_MIN_HEIGHT = 180f;

    #endregion

    #region Resource File Paths

    public const string HAND_UPGRADES_FILE = "Upgrades/HandsUpgrade";
    public const string MULTIPLIER_UPGRADES_FILE = "Upgrades/MultiplierUpgrade";

    #endregion

    #region Audio

    #region SFX

    public const string CLICK_SFX = "Click";
    public const string PAUSE_SFX = "Pause";
    public const string WRONG_SFX = "Wrong";
    public const string STAGE_CLEARED_SFX = "Stage Cleared";
    public const string OVEN_SFX = "Oven";

    public const float CONGRATULATIONS_SFX_DELAY = 3.4f;

    #endregion

    #endregion
}
