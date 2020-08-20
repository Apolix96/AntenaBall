using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoostBehaviour : MonoBehaviour
{
    public float waveLifeTime = 3f;
    public float waveSpeedModificator = 1f;
    private bool movingAnim = false;
    
    public void SpawnBigWave()
    {
        
        var waveBehaviour = LevelController.Instance.CreateBoostWave(transform.position, transform, 5);
        waveBehaviour.speed *= waveSpeedModificator;
        waveBehaviour.lifeTime = waveLifeTime;
        //waveBehaviour.
    }

    private void Anim()
    {
        Sequence mySequence = DOTween.Sequence();

        if (!movingAnim)
        {
            Vector3 startScale = transform.localScale;
            movingAnim = true;

            mySequence.Append(transform.DOScale(startScale + new Vector3(0.1f, 0.1f, 0) , 0.25f))
                .Append(transform.DOScale(startScale, 0.25f)).OnComplete(() =>
                {
                    GetComponent<SpriteRenderer>().enabled = false;
                    GetComponent<CircleCollider2D>().enabled = false;
                    movingAnim = false;
                });
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && GetComponent<CircleCollider2D>().enabled)
            Anim();
    }

}
