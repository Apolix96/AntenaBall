using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LaserBehaviour : MonoBehaviour
{
    private Vector3 startPosition;
    public Vector3 endPosition;

    public float duration = 1;
    // Start is called before the first frame update

    private void Awake()
    {
        startPosition = this.transform.position;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.Equals(startPosition))
        {
            transform.DOMove(endPosition, duration);
        }
        if (transform.position.Equals(endPosition))
        {
            transform.DOMove(startPosition, duration);
        }

    }
}
