using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour
{
    private AudioSource audioSource;
    private float sfxVolume = 1f;

    [Header("UI Sounds")]
    [SerializeField] private AudioClip clickSFX;
    [SerializeField] private AudioClip pauseSFX;
    [SerializeField] private AudioClip wrongSFX;
    [SerializeField] private AudioClip stageClearedSFX;
    [SerializeField] private AudioClip ovenSFX;

    [Header("Slap Sounds")]
    [SerializeField] private List<AudioClip> slapClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> fireEffectClips = new List<AudioClip>();

    [Header("Congratulations Sounds")]
    [SerializeField] private List<AudioClip> congratulationsClips = new List<AudioClip>();

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        UpdateVolume( Settings.Instance.SFXVolume);
    }

    public void UpdateVolume(float value)
    {
        sfxVolume = value;

        audioSource.volume = sfxVolume;
        Settings.Instance.SFXVolume = sfxVolume;

        Settings.Save();
    }

    public void PlaySound(string str)
    {
        switch (str)
        {
            case Config.CLICK_SFX:
                audioSource.PlayOneShot(clickSFX);
                break;

            case Config.PAUSE_SFX:
                audioSource.PlayOneShot(pauseSFX);
                break;

            case Config.WRONG_SFX:
                audioSource.PlayOneShot(wrongSFX);
                break;

            case Config.STAGE_CLEARED_SFX:
                audioSource.PlayOneShot(stageClearedSFX);
                break;

            case Config.OVEN_SFX:
                audioSource.PlayOneShot(ovenSFX);
                break;
        }
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void PlayRandomAudioClip(List<AudioClip> audioClips)
    {
        int randomClip = Random.Range(0, audioClips.Count);

        PlaySound(audioClips[randomClip]);
    }

    #region Slap Sounds

    public void PlayRandomSlapClip()
    {
        PlayRandomAudioClip(slapClips);
    }

    public void PlayRandomFireSlapClip()
    {
        PlayRandomAudioClip(slapClips);
        PlayRandomAudioClip(fireEffectClips);
    }

    #endregion

    public void PlayRandomCongratulationsSound()
    {
        PlayRandomAudioClip(congratulationsClips);
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }
}
