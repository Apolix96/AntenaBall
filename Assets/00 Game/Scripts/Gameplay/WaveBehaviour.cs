using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WaveBehaviour : MonoBehaviour
{
    [FormerlySerializedAs("parent")] public Transform waveParent;

    [Header("Wave Parameters")] [SerializeField]
    private float edgeWidth = 0.25f;

    public float speed;
    public float lifeTime;
    public LayerMask obstacleMask;
    public int minusModifier;
    public bool isUpper = false;

    [Header("References")] [SerializeField]
    private WaveView waveView;

    private float currentRaiuds = 0f;
    private float createdTimeStamp;

    private RaycastHit2D[] raycastHits2D = new RaycastHit2D[25];

    private List<Collider2D> hitedActiveObjects = new List<Collider2D>();
    
    public string sortingLayerName;        // The name of the sorting layer .
    public int sortingOrder;            //The sorting order

    private void Awake()
    {
        createdTimeStamp = Time.time;
        
        if (name == "UpperWave(Clone)" || name == "UpperWaveMinimal(Clone)" || name == "BigWaveWhite(Clone)")
        {
            GetComponentInChildren<Renderer>().sortingLayerName = "Default";
            GetComponentInChildren<Renderer>().sortingOrder = 11;
        }
    }

    private void Start()
    {
    }
    
    public float GetLifeTime()
    {
        return lifeTime;
    }

    public float GetCreatedTimestamp()
    {
        return createdTimeStamp;
    }

    private void FixedUpdate()
    {

        if (Time.time > (createdTimeStamp + 0.7f * lifeTime))
        {
            var alpha =  1 - (Time.time - createdTimeStamp) / lifeTime + 0.1f;
            gameObject.GetComponentInChildren<Renderer>().material.SetColor("_Color", new Color(1,1,1, alpha));

        }
        
        currentRaiuds += speed * Time.fixedDeltaTime;

        waveView.viewRadius = currentRaiuds;

        var collidersCount =
            Physics2D.CircleCastNonAlloc(transform.position, currentRaiuds, Vector2.zero, raycastHits2D, 0f);

        //transform.localScale = currentRaiuds * 2 * Vector3.one;

        if (collidersCount > 0)
        {
            for (var ci = 0; ci < raycastHits2D.Length; ci++)
            {
                if (ci < collidersCount)
                {
                    var colliderInWave = raycastHits2D[ci].collider;

                    var wavePosition = (Vector2) transform.position;
                    var pointPosition = raycastHits2D[ci].point;

                    var distance = Vector2.Distance(wavePosition, pointPosition);

                    var inRange = distance >= currentRaiuds - edgeWidth;
                    var hit = Physics2D.Raycast(wavePosition, pointPosition - wavePosition, distance, obstacleMask);

                    var isSignalClosed = hit.collider != null && hit.collider != colliderInWave;
                    
                    if (!isSignalClosed)
                    {
                        switch (colliderInWave.tag)
                        {
                            case "Mirror":
                                var mirror = colliderInWave.GetComponent<MirrorBehaviour>();

                                if (waveParent == LevelController.Instance.transform &&
                                    !hitedActiveObjects.Contains(colliderInWave) && waveParent != mirror.transform)
                                {
                                    hitedActiveObjects.Add(colliderInWave);

                                    var point = raycastHits2D[ci].point;
                                    var newPoint = point + (wavePosition - point).normalized * 0.05f;

                                    mirror.SpawnWave(newPoint);
                                }

                                break;
                            case "Repeater":
                                var repeater = colliderInWave.GetComponent<RepeaterBehaviour>();

                                if (!hitedActiveObjects.Contains(colliderInWave) && waveParent != repeater.transform)
                                {
                                    hitedActiveObjects.Add(colliderInWave);
                                    repeater.SpawnWave();
                                }

                                break;
                            default:
                                break;
                        }
                    }

                    /*var levelPoint = colliderInWave.GetComponent<LevelPointBehaviour>();

                    if (!levelPoint) continue;

                    if (inRange && !isSignalClosed)
                        levelPoint.TouchByWave(this);
                    else
                        levelPoint.RemoveWave(this);*/
                    
                    var block = colliderInWave.GetComponent<BlockBehaviour>();
                    if (!block) continue;
                    block.TouchByWave(this);
                    
                }
                else
                {
                    var oldCollider = raycastHits2D[ci].collider;

                    if (oldCollider == null || oldCollider.GetComponent<Collider>()) continue;


                    var levelPoint = oldCollider.GetComponent<LevelPointBehaviour>();

                    if (levelPoint)
                    {
                        levelPoint.RemoveWave(this);
                    }
                }
            }
        }
        else
        {
            foreach (var hit in raycastHits2D)
            {
                var oldCollider = hit.collider;
                if (oldCollider == null) continue;

                var levelPoint = oldCollider.GetComponent<LevelPointBehaviour>();

                if (levelPoint)
                {
                    levelPoint.RemoveWave(this);
                }
            }
        }

        if (Time.time - createdTimeStamp >= lifeTime)
            Destroy(gameObject);
    }
}