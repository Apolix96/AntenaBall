using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MinusOneBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    void Anim()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(GetComponent<SpriteRenderer>().DOFade(1, 0.25f)).Append(GetComponent<SpriteRenderer>().DOFade(0, 0.25f
            )).
            OnComplete(() =>
        {
            GetComponent<SpriteRenderer>().enabled = false;
        });
    }

    void Awake()
    {
        Anim();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
