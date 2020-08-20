using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraContoller : MonoBehaviour
{
    public float movementSpeed = 0.005f;

    public float time = 5f;

    public float acceleration = 0.003f;

    private bool accelerated = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > time && !accelerated)
        {
            movementSpeed += acceleration;
            accelerated = true;
        }
        
        transform.position += Vector3.up * movementSpeed;

    }
}
