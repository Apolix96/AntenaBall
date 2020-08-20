using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StarBehaviour : MonoBehaviour
{
    private bool movingAnim = false;

    public GameObject sparkleEffectPrefab;
    // Start is called before the first frame update
    void Start()
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

            mySequence.Append(transform.DOScale(startScale + new Vector3(0.1f, 0.1f, 0f), 0.5f))
                .Append(transform.DOScale(startScale, 0.5f)).OnComplete(() => { movingAnim = false; });
        }
    }
    
    private void Anim()
    {
        Instantiate(sparkleEffectPrefab, transform);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && GetComponent<CircleCollider2D>().enabled)
            Anim();
    }

    
}
