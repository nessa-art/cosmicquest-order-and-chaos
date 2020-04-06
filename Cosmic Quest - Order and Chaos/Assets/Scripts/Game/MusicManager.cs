﻿using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioHelper.EntityAudioClip PlayingMusic;
    public AudioHelper.EntityAudioClip BossFightMusic;
    public AudioHelper.EntityAudioClip GameOverMusic;
    public AudioHelper.EntityAudioClip MenuMusic;
    public AudioHelper.EntityAudioClip PausedMusic;
    public AudioHelper.EntityAudioClip LoadingMusic;
    private AudioHelper.EntityAudioClip CurrentAudioLoop;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        CurrentAudioLoop = PlayingMusic;
        GameManager.Instance.onStateChange.AddListener(UpdateMusic);
        UpdateMusic();
    }

    private void Update()
    {
        if (CurrentAudioLoop.loopLength > 0 && CurrentAudioLoop.loopThreshold > 0)
        {
            if (audioSource.timeSamples > CurrentAudioLoop.loopThreshold * CurrentAudioLoop.clip.frequency)
            {
                audioSource.timeSamples -= Mathf.RoundToInt(CurrentAudioLoop.loopLength * CurrentAudioLoop.clip.frequency);
            }
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    private void UpdateMusic()
    {
        switch (GameManager.Instance.CurrentState)
        {
            case GameManager.GameState.Menu:
                break;
            case GameManager.GameState.Paused:
                break;
            case GameManager.GameState.Loading:
                break;
            case GameManager.GameState.Playing:
                CurrentAudioLoop = PlayingMusic;
                break;
            case GameManager.GameState.BossFight:
                CurrentAudioLoop = BossFightMusic;
                break;
            case GameManager.GameState.GameOver:
                break;
            default:
                break;
        }

        audioSource.clip = CurrentAudioLoop.clip;
        audioSource.pitch = CurrentAudioLoop.pitch;
        audioSource.volume = CurrentAudioLoop.volume;
        audioSource.timeSamples = Mathf.RoundToInt(CurrentAudioLoop.loopThreshold * CurrentAudioLoop.clip.frequency);
        audioSource.Play();
    }
}
