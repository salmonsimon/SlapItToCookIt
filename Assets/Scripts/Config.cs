using System.Collections.Generic;

public class Config 
{
    #region Scene Names

    public const string LOGIN_SCENE_NAME = "Login";
    public const string MAIN_MENU_SCENE_NAME = "Main Menu";
    public const string MAIN_SCENE_NAME = "Main";

    #endregion

    #region API

    public const string API_TITLE_ID = "F11D5";

    public const string API_COINS_KEY = "Coins";
    public const string API_RUBIES_KEY = "Rubies";

    public const string API_HAND_UPGRADES_DONE_KEY = "Hand Upgrades Done";
    public const string API_NEW_HAND_UPGRADE_REDEEM_TIME_KEY = "New Hand Upgrade Redeem Time";

    public const string API_MULTIPLIER_UPGRADES_DONE_KEY = "Multiplier Upgrades Done";
    public const string API_NEW_MULTIPLIER_UPGRADE_REDEEM_TIME_KEY = "New Multiplier Upgrade Redeem Time";

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

    #region UI

    public const string ANIMATOR_SHOW_COUNTERS = "Appear";
    public const string ANIMATOR_HIDE_COUNTERS = "Disappear";

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

    #endregion

    #endregion
}
