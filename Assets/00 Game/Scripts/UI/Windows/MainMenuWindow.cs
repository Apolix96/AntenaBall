using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuWindow : WindowController
{
    public override void UpdateView()
    {
        
    }

    public void StartGame()
    {
        GameController.Instance.LoadCurrentLevel();
        CloseWindow();
    }
}
