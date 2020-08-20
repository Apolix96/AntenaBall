using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChaserBehaviour : MonoBehaviour
{
    public float movementSpeed = 0.005f;

    public float scalingTime = 0.25f;

    private bool scalingAnim = false;
    
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
        transform.position += Vector3.up * movementSpeed;
        Sequence mySequence = DOTween.Sequence();
        //mySequence.Append(transform.DOScale(0.35f, 0.1f)).Append(transform.DOScale(0.30f, 0.1f));
        
        if (Time.time > time && !accelerated)
        {
            movementSpeed += acceleration;
            accelerated = true;
        }
        
        if (!scalingAnim)
        {
            Vector3 startScale = transform.localScale;
            scalingAnim = true;
            mySequence.Append(transform.DOScaleY(startScale.y + 0.5f, scalingTime))
                .Append(transform.DOScaleY(startScale.y, scalingTime)).OnComplete(() => { scalingAnim = false; });
        }
    }
}
