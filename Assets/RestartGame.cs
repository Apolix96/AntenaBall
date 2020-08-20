using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartGame : MonoBehaviour
{
    private Button _btnRestartGame;
    [SerializeField] private ControllGame _invisiblePanelRestartGame;

    private void Start()
    {
        _btnRestartGame = GetComponent<Button>();
        _btnRestartGame.onClick.AddListener( () => RestartLevelGame());
    }

    private void RestartLevelGame()
    {
        SceneManager.LoadScene("HCMinimal test1");
        _invisiblePanelRestartGame.InvisiblePanelRestart();
        
    }
    

}
