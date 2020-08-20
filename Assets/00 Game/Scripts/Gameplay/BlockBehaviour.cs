using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockBehaviour : MonoBehaviour
{
    public enum AnimType
    {
        First, Minimalizm, Puzzle
    }

    public AnimType animType = AnimType.First;
    
    public int signalsNeeded = 1;
    public bool destructable = true;
    public bool rand = true;

    [Tooltip("Group value, 0 - not in group")]
    public int pointGroup = 0;

    public Sprite[] numbers;
    public GameObject sparkleEffectPrefab;
    public GameObject sparkleTouchEffectPrefab;
    public GameObject minusOnePrefab;
    public GameObject minusFivePrefab;
    

    [Header("Refernces")] [SerializeField] private TextMeshPro signalsText;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private LineRenderer groupLine;

    public bool ConditionMet { get; private set; }

    private bool _isOn;
    private bool movingAnim = false;

    public bool IsOn
    {
        get => _isOn;
        set
        {
            _isOn = value;
            if (IsOn)
                spriteRenderer.color = Color.green;
        }
    }

    private Transform nextPoint
    {
        get
        {
            var parent = transform.parent;

            for (var i = transform.GetSiblingIndex() + 1; i < parent.childCount; i++)
            {
                var point = parent.GetChild(i).GetComponent<LevelPointBehaviour>();
                if (point.pointGroup == pointGroup)
                    return point.transform;
            }

            return null;
        }
    }

    private List<WaveBehaviour> currentWaves = new List<WaveBehaviour>();

    private void InitSignalsCount()
    {
        if (destructable)
        {
            if (rand)
                signalsNeeded = Random.Range(-2, signalsNeeded);

            
            if (destructable)
            {
                if (signalsNeeded > 0)
                    try
                    {
                        GetComponent<SpriteRenderer>().sprite = numbers[signalsNeeded-1];
                    }
                    catch (Exception e)
                    {
                        Debug.Log(gameObject.name);
                        throw;
                    }

                else
                {
                    GetComponent<SpriteRenderer>().enabled = false;
                    GetComponent<BoxCollider2D>().enabled = false;
                    gameObject.SetActive(false);
                }
                
            }
            else 
                GetComponent<SpriteRenderer>().sprite = numbers[numbers.Length - 1];
            
        }
    }

    private void DecreaseSignalsCount(int minus)
    {
        signalsNeeded-=minus;
    }

    private void Awake()
    {
        InitSignalsCount();
//        signalsText.text = signalsNeeded.ToString();
    }

    private void Start()
    {
        if (pointGroup != 0 && nextPoint != null)
        {
            groupLine.SetPosition(0, transform.position);
            groupLine.SetPosition(1, nextPoint.position);
        }
    }

    public void TouchAnim(WaveBehaviour wb)
    {
        
        Sequence mySequence = DOTween.Sequence();
        //Instantiate(sparkleTouchEffectPrefab, transform);
        //mySequence.Append(transform.DOScale(0.35f, 0.1f)).Append(transform.DOScale(0.30f, 0.1f));

        if (!movingAnim)
        {
            Vector3 startPosition = transform.position;
            movingAnim = true;
            
            //Debug.Log(wb.transform.position + " " +wb.transform.position.normalized);
            //mySequence.Append(transform.DOMove(startPosition - wb.transform.position.normalized * 0.1f, 0.25f)).
            mySequence.Append(transform.DOMove(startPosition + Vector3.up * 0.1f, 0.25f)).
                Append(transform.DOMove(startPosition, 0.25f)).OnComplete(() => { movingAnim = false; });
            

        }
        
        
        if (destructable && signalsNeeded > 0)
                GetComponent<SpriteRenderer>().sprite = numbers[signalsNeeded - 1];
    }
    
    
    public void TouchAnimMinusOne(WaveBehaviour waveBehaviour)
    {
        if (waveBehaviour.gameObject.CompareTag("BoostWave"))
            Instantiate(minusFivePrefab, transform);
        else
            Instantiate(minusOnePrefab, transform);

        if (destructable && signalsNeeded > 0)
            GetComponent<SpriteRenderer>().sprite = numbers[signalsNeeded - 1];
    }   
    

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Chaser"))
        {
            if (animType == AnimType.First)
                DestroyAnim();
        }
        
    }

    public void TouchAndDestroyAnim()
    {
        Sequence mySequence = DOTween.Sequence();
        transform.DOScale(0.35f, 0.1f).OnComplete(() =>
        {
            Instantiate(sparkleEffectPrefab, transform);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        });

    }
    
    public void DestroyAnim()
    {
        Sequence mySequence = DOTween.Sequence();

            Instantiate(sparkleEffectPrefab, transform);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;


    }
    
    public void DestroyFlyAwayAnim()
    {
        Sequence mySequence = DOTween.Sequence();
        
        if (!movingAnim)
        {
            
            GetComponent<PolygonCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().sortingOrder++;
                
            movingAnim = true;

            Vector3 randUpDirection = new Vector3(Random.Range(-50f, 50f), 100f, 0);
            
            mySequence.Join(transform.DOMove(transform.position + randUpDirection, 15))
                .Join(transform.DORotate(new Vector3(360,360, 360), 2f, RotateMode.FastBeyond360)).OnComplete(() =>
                {
                    movingAnim = false;
                    GetComponent<BoxCollider2D>().enabled = false;
                });
        }

    }

    public void TouchByWave(WaveBehaviour waveBehaviour)
    {

        if (currentWaves.Contains(waveBehaviour)) return;
        
        currentWaves.Add(waveBehaviour);
        
        if (waveBehaviour.gameObject.CompareTag("BoostWave"))
        {
            DecreaseSignalsCount(5);
        }
        else
            DecreaseSignalsCount(1);

        if (destructable && signalsNeeded <= 0)
        {
            //LevelController.Instance.score++;
            
            if (animType == AnimType.First || animType == AnimType.Minimalizm)
                DestroyAnim();
            if (animType == AnimType.Puzzle)
                DestroyFlyAwayAnim();
            //DestroyAnim();
        }
        else
        {
            if (animType == AnimType.First || animType == AnimType.Puzzle)
                TouchAnim(waveBehaviour);
            if (animType == AnimType.Minimalizm)
                TouchAnimMinusOne(waveBehaviour);
            //TouchAnimMinusOne(waveBehaviour);
        }
        


        /*ConditionMet = currentWaves.Count >= signalsNeeded;

        if (pointGroup == 0)
        {
            IsOn = ConditionMet;
        }
        else
        {
            LevelController.Instance.GetGroupCondition(pointGroup);
        }*/
    }

    public void RemoveWave(WaveBehaviour waveBehaviour)
    {
        if (!currentWaves.Contains(waveBehaviour)) return;
        currentWaves.Remove(waveBehaviour);

        if (pointGroup != 0)
        {
            ConditionMet = currentWaves.Count >= signalsNeeded;
        }
    }
}
