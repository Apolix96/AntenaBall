using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControllGame : MonoBehaviour
{
    [SerializeField] private GameObject _panelRestartGame;

    public void VisiblePanelRestart()
    {
        Debug.Log("Complete visible panel");
        _panelRestartGame.SetActive(true);
    }

    public void InvisiblePanelRestart()
    {
        _panelRestartGame.SetActive(false);
        //Time.time = Time.timeSinceLevelLoad;
    }

    public void RestartLevelGame()
    {

    }

}
