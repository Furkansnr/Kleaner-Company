using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundManager : MonoBehaviour
{
    public static GameSoundManager instance;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource soundAudioSource;

    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip skillcheck;
    [SerializeField] private AudioClip camsil;
    [SerializeField] private AudioClip fill;
    [SerializeField] private AudioClip end;
    [SerializeField] private AudioClip button;

    private void OnEnable()
    {
        GameManager.instance.GameEnd += GameEnd;
        GameManager.instance.SkillCheckSuccesfull += Fill;
        GameManager.instance.SkillCheckAppears += SkillCheck;
    }

    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
    }

    void Start()
    {
        musicAudioSource.volume = PlayerPrefs.GetFloat("musicVolume", 100);
        soundAudioSource.volume = PlayerPrefs.GetFloat("soundVolume", 100);
    }

    void GameEnd()
    {
        musicAudioSource.clip = end;
        musicAudioSource.Play();
    }

    void Fill(string a,int b)
    {
        soundAudioSource.PlayOneShot(fill);
    }
    void SkillCheck(string a)
    {
        soundAudioSource.PlayOneShot(fill);
    }

    public void Button()
    {
        soundAudioSource.PlayOneShot(button);
    }

    private void OnDisable()
    {
        GameManager.instance.GameEnd -= GameEnd;
        GameManager.instance.SkillCheckSuccesfull -= Fill;
        GameManager.instance.SkillCheckAppears -= SkillCheck;
    }
}
