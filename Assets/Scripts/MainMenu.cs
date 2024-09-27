using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button StartB;
    [SerializeField] private Button CreditsB;
    [SerializeField] private Button QuitB;
    [SerializeField] private Button SettingsB;
    [SerializeField] private Button SoundB;
    [SerializeField] private Button[] BackB;

    [SerializeField] private GameObject CreditsP;
    [SerializeField] private GameObject SettingsP;
    void OnEnable()
    {
        StartB.onClick.AddListener(StartButton);
        CreditsB.onClick.AddListener(CreditsButton);
        QuitB.onClick.AddListener(QuitButton);
        SettingsB.onClick.AddListener(SettingsButton);
        SoundB.onClick.AddListener(SoundButton);
    }
    void OnDisable()
    {
        StartB.onClick.RemoveAllListeners();
        CreditsB.onClick.RemoveAllListeners();
        QuitB.onClick.RemoveAllListeners();
        SettingsB.onClick.RemoveAllListeners();
        SoundB.onClick.RemoveAllListeners();
    }
    


    void StartButton()
    {
        SceneManager.LoadScene(1);
    }
    void CreditsButton()
    {
        CreditsP.SetActive(true);
        BackB[0].onClick.AddListener(() => BackButton(CreditsP,0));
    }
    void SettingsButton()
    {
        SettingsP.SetActive(true);
        BackB[1].onClick.AddListener(() => BackButton(SettingsP,1));
    }
    void SoundButton()
    {
        
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
}
