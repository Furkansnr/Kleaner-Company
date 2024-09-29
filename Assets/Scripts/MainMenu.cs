using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private float musicVolume;
    private float soundVolume;

    [SerializeField] private Button StartB;
    [SerializeField] private Button CreditsB;
    [SerializeField] private Button QuitB;
    [SerializeField] private Button SettingsB;
    [SerializeField] private Button TutorialB;
    [SerializeField] private Button[] BackB;

    [SerializeField] private GameObject CreditsP;
    [SerializeField] private GameObject SettingsP;
    [SerializeField] private GameObject TutorialP;

    [SerializeField] private AudioSource MusicAS;
    [SerializeField] private AudioSource SoundAS;

    [SerializeField] private AudioClip soundC;

    //[SerializeField] private AudioClip musicC; Bu zaten source üzerinde olcak
    [SerializeField] private Slider soundSl;
    [SerializeField] private Slider musicSl;

    private int playerCount;
    [SerializeField] private GameObject[] playerS;
    [SerializeField] private Button tempAddB; // Lidar sensör ne kadar oyuncu algılarsa o kadar player olur

    [SerializeField]
    private Button tempLessB; //Lidar entegrasyonundan öncesinde karakter sayısını ayaarlamak için butonlar var


    void OnEnable()
    {
        StartB.onClick.AddListener(StartButton);
        CreditsB.onClick.AddListener(CreditsButton);
        QuitB.onClick.AddListener(QuitButton);
        SettingsB.onClick.AddListener(SettingsButton);
        TutorialB.onClick.AddListener(TutorialButton);

        playerCount = 0;
        GameData.instance.SetPlayerCount(playerCount+1);
        tempAddB.onClick.AddListener(TempAdd);
        tempLessB.onClick.AddListener(TempLess);
    }

    void OnDisable()
    {
        StartB.onClick.RemoveAllListeners();
        CreditsB.onClick.RemoveAllListeners();
        QuitB.onClick.RemoveAllListeners();
        SettingsB.onClick.RemoveAllListeners();
        TutorialB.onClick.RemoveAllListeners();

        musicVolume = PlayerPrefs.GetFloat("musicVolume", 100);
        soundVolume = PlayerPrefs.GetFloat("soundVolume", 100);

        musicSl.value = musicVolume;
        soundSl.value = soundVolume;

        musicSl.onValueChanged.AddListener((arg0 => MusicSlider()));
        soundSl.onValueChanged.AddListener((arg0 => SoundSlider()));

        tempAddB.onClick.RemoveAllListeners();
        tempLessB.onClick.RemoveAllListeners();
    }

    void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    void CreditsButton()
    {
        CreditsP.SetActive(true);
        BackB[0].onClick.AddListener(() => BackButton(CreditsP, 0));
    }

    void SettingsButton()
    {
        SettingsP.SetActive(true);
        BackB[1].onClick.AddListener(() => BackButton(SettingsP, 1));
    }

    void TutorialButton()
    {
        TutorialP.SetActive(true);
        BackB[2].onClick.AddListener(() => BackButton(TutorialP, 2));
    }

    void BackButton(GameObject closedPanel, int i)
    {
        closedPanel.SetActive(false);
        BackB[i].onClick.RemoveAllListeners();
    }

    void QuitButton()
    {
        Application.Quit();
    }

    void SoundSlider()
    {
        soundVolume = soundSl.value;
        SoundAS.volume = soundVolume;
        SoundAS.PlayOneShot(soundC);
    }

    void MusicSlider()
    {
        musicVolume = musicSl.value;
        MusicAS.volume = musicVolume;
    }

    void TempLess()
    {
        if (playerCount > 0)
        {
            playerS[playerCount].SetActive(false);
            playerCount--;
            GameData.instance.SetPlayerCount(playerCount+1);
        }
    }

    void TempAdd()
    {
        if (playerCount < 3)
        {
            playerCount++;
            GameData.instance.SetPlayerCount(playerCount+1);
            playerS[playerCount].SetActive(true);
        }
    }
}