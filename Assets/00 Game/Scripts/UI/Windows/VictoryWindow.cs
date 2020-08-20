using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryWindow : WindowController
{
    public override void UpdateView()
    {
        
    }

    public void NextLevel()
    {
        var levelIndex = GameController.Instance.currentLevel + 1;
        
        if (levelIndex >= SceneManager.sceneCountInBuildSettings)
            levelIndex = 0;

        GameController.Instance.currentLevel = levelIndex;

        if (levelIndex != 0)
            GameController.Instance.LoadCurrentLevel();
        else
        {
            GameController.Instance.currentLevel = 1;
            GameController.Instance.GoToMainMenu();
        }

        CloseWindow();
        
    }
}
