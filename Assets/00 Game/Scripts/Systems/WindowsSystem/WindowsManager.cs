using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WindowsManager : GameSingleton<WindowsManager>
{
    [SerializeField] private Transform screensContainer;
    [SerializeField] private Transform windowsContainer;
    [SerializeField] private Transform effectsContainer;
    
    [Header("Other References")] 
    [SerializeField] CanvasGroup fadeGroup;
    [SerializeField] private CanvasScaler canvasScaler;

    public bool IsWindowOnTop(WindowController windowController)
    {
        return windowsContainer.childCount - 1 == windowController.transform.GetSiblingIndex();
    }
    
    public void CloseCurrentScreen()
    {
        if (screensContainer.childCount > 0)
            screensContainer.GetChild(0).GetComponent<WindowController>().CloseWindow();
    }

    public void CloseTopWindow()
    {
        windowsContainer.GetChild(windowsContainer.childCount - 1).GetComponent<WindowController>().CloseWindow();
    }

    public void CloseAllWindows()
    {
        for (var i = 0; i < windowsContainer.childCount; i++)
        {
            windowsContainer.GetChild(i).GetComponent<WindowController>().CloseWindow();
        }
    }

    public void ChangeFade(float fadeValue, float delay = 0f, float fadeTime = 0.5f, System.Action action = null)
    {
        fadeGroup.DOFade(fadeValue, fadeTime).SetDelay(delay).OnComplete(() => { action?.Invoke(); });
    }

    public T SearchForWindow<T>(Transform container) where T : WindowController
    {
        for (var i = 0; i < container.childCount; i++)
        {
            if (container.GetChild(i).GetComponent<T>())
            {
                return container.GetChild(i).GetComponent<T>();
            }
        }

        return null;
    }

    public T CreatScreen<T>(string screenName) where T : WindowController
    {
        var screen = SearchForWindow<T>(screensContainer);

        if (screen != null)
        {
            screen.UpdateView();
            return screen;
        }
        
        CloseCurrentScreen();
        var windowPrefab = Resources.Load<GameObject>("Screens/" + screenName);
            
        screen = Instantiate(windowPrefab, windowsContainer).GetComponent<T>();
        screen.OpenWindow();

        return screen;
    }

    public T CreateWindow<T>(string windowName) where T : WindowController
    {
        var window = SearchForWindow<T>(windowsContainer);

        if (window == null)
        {
            var windowPrefab = Resources.Load<GameObject>("Windows/" + windowName);
            
            window = Instantiate(windowPrefab, windowsContainer).GetComponent<T>();
            window.OpenWindow();
        }
        else
        {
            MakeWindowOnTop(window);
            window.UpdateView();
        }

        return window;
    }

    public void MakeWindowOnTop(WindowController window)
    {
        window.transform.SetSiblingIndex(windowsContainer.childCount);
    }
    
    //Convert to canvas position
    public Vector3 UnscaleEventDelta(Vector3 vec)
    {
        var referenceResolution = canvasScaler.referenceResolution;
        var currentResolution = new Vector2(Screen.width, Screen.height);

        var widthRatio = currentResolution.x / referenceResolution.x;
        var heightRatio = currentResolution.y / referenceResolution.y;
        var ratio = Mathf.Lerp(widthRatio, heightRatio, canvasScaler.matchWidthOrHeight);

        return vec / ratio;
    }
}