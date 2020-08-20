using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverWindow : WindowController
{
    public override void UpdateView()
    {
        
    }

    public void RestartLevel()
    {
        GameController.Instance.LoadCurrentLevel();
        CloseWindow();
    }
}
