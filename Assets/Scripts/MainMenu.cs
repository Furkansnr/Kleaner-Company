using System.Collections;
using System.Collections.Generic;
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
    void Start()
    {
        StartB.onClick.AddListener(StartButton);
        QuitB.onClick.AddListener(QuitButton);
    }


    void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    void QuitButton()
    {
        Application.Quit();
    }
}
