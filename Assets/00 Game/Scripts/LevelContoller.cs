using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelContoller : MonoBehaviour
{
    [SerializeField] private int _lvlScene;

    
    IEnumerator loadLevel()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(_lvlScene);
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            _lvlScene++;
            StartCoroutine(loadLevel());
        }
    }
}
