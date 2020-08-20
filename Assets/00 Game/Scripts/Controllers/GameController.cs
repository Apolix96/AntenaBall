using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : GameSingleton<GameController>
{
    public int currentLevel = 1;
    
    public override void Awake()
    {
        if (Instance == null)
        {
            Application.targetFrameRate = 60;
        }
        
        base.Awake();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            WindowsManager.Instance.CreateWindow<MainMenuWindow>("Main Menu Window");
        }
        else
        {
            WindowsManager.Instance.CreateWindow<GameplayWindow>("Gameplay Window");
        }
    }

    public void LoadCurrentLevel()
    {
        SceneManager.LoadScene(currentLevel);
        WindowsManager.Instance.CreateWindow<GameplayWindow>("Gameplay Window");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
        WindowsManager.Instance.CreateWindow<MainMenuWindow>("Main Menu Window");
    }
}
