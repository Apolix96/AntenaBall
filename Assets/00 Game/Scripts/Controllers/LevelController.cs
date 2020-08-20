using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelController : SceneSingleton<LevelController>
{
    [Header("Level Settings")] [SerializeField]
    private float waveSpawnCoolDown = 0.25f;

    [SerializeField] private int wavesToSpawnCount = 2;
    public int WavesRemains => wavesToSpawnCount - spawnedWavesCount;

    [Header("References")] [SerializeField]
    private Transform wavesContainer;

    [SerializeField] private GameObject wavePrefab;
    [SerializeField] private GameObject bigWavePrefab;
    
    [SerializeField] private Transform pointsContainer;

    public bool levelComplete { get; private set; }
    public bool levelFailed { get; private set; }

    private float waveSpawnTimeStamp = -999f;
    private int spawnedWavesCount = 0;

    private readonly Dictionary<int, List<LevelPointBehaviour>> levelPoints = new Dictionary<int, List<LevelPointBehaviour>>();

    private void Start()
    {
        foreach (var point in pointsContainer.GetComponentsInChildren<LevelPointBehaviour>())
        {
            if (!levelPoints.ContainsKey(point.pointGroup))
            {
                levelPoints.Add(point.pointGroup, new List<LevelPointBehaviour>() {point});
            }
            else
            {
                levelPoints[point.pointGroup].Add(point);
            }
        }
    }

    private void Update()
    {
        var touchPos = new Vector2();
        var spawnWave = false;

 /*       if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            spawnWave = true;
            touchPos = Input.GetTouch(0).position;
        }
*/
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            //spawnWave = true;
            //touchPos = Input.mousePosition;
        }
#endif

        if (spawnWave && spawnedWavesCount < wavesToSpawnCount && Time.time - waveSpawnTimeStamp >= waveSpawnCoolDown)
        {
            var ray = Camera.main.ScreenPointToRay(touchPos);
            var hit = Physics2D.GetRayIntersection(ray,Mathf.Infinity);

            var clickBlocked = hit.collider != null && hit.collider.CompareTag("Click Block");


            if (!clickBlocked)
            {
                waveSpawnTimeStamp = Time.time;

                var worldPos = Camera.main.ScreenToWorldPoint(touchPos);
                worldPos.z = 0f;

                CreateWave(worldPos, transform);
                spawnedWavesCount++;
            }
        }

        var pointsWithCondition = pointsContainer.GetComponentsInChildren<LevelPointBehaviour>()
            .Count(levelPoint => levelPoint.IsOn);

        if (pointsWithCondition == pointsContainer.childCount && !levelComplete && !levelFailed)
        {
            //TODO: level complete
            Debug.Log("Win");
            levelComplete = true;
            
            WindowsManager.Instance.CreateWindow<VictoryWindow>("Victory Window");
        }

        if (spawnedWavesCount >= wavesToSpawnCount && wavesContainer.childCount == 0 && !levelComplete && !levelFailed)
        {
            //TODO: check win or game over
            Debug.Log("Game Over");
            levelFailed = true;
            
            WindowsManager.Instance.CreateWindow<GameoverWindow>("Gameover Window");
        }
    }

    public WaveBehaviour CreateWave(Vector3 position, Transform parent)
    {
        var waveBehaviour = Instantiate(wavePrefab, wavesContainer).GetComponent<WaveBehaviour>();
        waveBehaviour.transform.position = position;
        waveBehaviour.waveParent = parent;

        return waveBehaviour;
    }
    
    public WaveBehaviour CreateBoostWave(Vector3 position, Transform parent, int minusModifier)
    {
        var waveBehaviour = Instantiate(bigWavePrefab, wavesContainer).GetComponent<WaveBehaviour>();

        waveBehaviour.transform.position = position;
        waveBehaviour.waveParent = parent;
        waveBehaviour.minusModifier -= minusModifier;

        return waveBehaviour;
    }

    public void GetGroupCondition(int group)
    {
        var points = levelPoints[group];
        var pointsWithGroup = points.Count;
        var pointsWithCondition = points.Count(levelPoint => levelPoint.ConditionMet);
        var isOn = pointsWithCondition == pointsWithGroup;

        if (!isOn) return;

        points.ForEach(point => point.IsOn = true);
    }
}