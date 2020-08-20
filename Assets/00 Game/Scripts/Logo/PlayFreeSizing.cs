using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayFreeSizing : MonoBehaviour
{
    private bool movingAnim = false;
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Sequence mySequence = DOTween.Sequence();
        
        if (!movingAnim)
        {

            movingAnim = true;
            Vector3 startScale = transform.localScale;

            mySequence.Append(transform.DOScale(startScale + new Vector3(0.1f,0.1f, 0f), 0.5f))
                .Append(transform.DOScale(startScale, 0.5f)).OnComplete(() =>
                {
                    movingAnim = false;
                });
        }
    }
}
