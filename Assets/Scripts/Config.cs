public class Config 
{
    #region Scene Names

    public const string MAIN_MENU_SCENE_NAME = "Main Menu";
    public const string MAIN_SCENE_NAME = "Main";

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

    #region Audio

    #region SFX

    public const string CLICK_SFX = "Click";
    public const string PAUSE_SFX = "Pause";
    public const string STAGE_CLEARED_SFX = "Stage Cleared";
    public const string OVEN_SFX = "Oven";

    #endregion

    #endregion
}
