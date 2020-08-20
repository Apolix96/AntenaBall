using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayWindow : WindowController
{
    [Header("References")] [SerializeField]
    private TextMeshProUGUI wavesText;
    
    public override void UpdateView()
    {
        
    }

    private void Update()
    {
        wavesText.text = "Waves remain: " +  LevelController.Instance.WavesRemains;
    }
}
