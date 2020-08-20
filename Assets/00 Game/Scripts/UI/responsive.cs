using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class responsive : MonoBehaviour
{
    public float x,y,z;

    private void Update()
    {
        Camera camera = GetComponent<Camera>();
        float cameraHeight = x; // Нужное значение размера камеры
        float desiredAspect = y / z; // Соотношение под которое подобран размер
        float aspect = camera.aspect;
        float ratio = desiredAspect / aspect;
        camera.orthographicSize = cameraHeight * ratio;
    }
    
}
