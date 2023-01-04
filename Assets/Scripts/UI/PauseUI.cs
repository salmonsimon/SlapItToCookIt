using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    private bool isGamePaused = false;
    public bool IsGamePaused { get { return isGamePaused; } }

    [SerializeField] Button pauseButton;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject settingsPanel;

    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;

    public void PauseGame()
    {
        GameManager.instance.GetSFXManager().PlaySound(Config.PAUSE_SFX);
        SetGamePaused(true);

        pauseButton.gameObject.SetActive(false);
        pausePanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void ResumeGame()
    {
        SetGamePaused(false);

        pauseButton.gameObject.SetActive(true);
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void SetGamePaused(bool value)
    {
        isGamePaused = value;

        if (value)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    public void SetAudioSlidersVolumesPauseMenu()
    {
        musicVolumeSlider.value = Settings.Instance.musicVolume;
        sfxVolumeSlider.value = Settings.Instance.SFXVolume;
    }

    public void ToMainMenu()
    {
        GameManager.instance.ToMainMenu();
    }

    public void UpdateMusicVolume(float value)
    {
        GameManager.instance.GetMusicManager().UpdateVolume(value);
    }

    public void UpdateSFXVolume(float value)
    {
        GameManager.instance.GetSFXManager().UpdateVolume(value);
    }
}
