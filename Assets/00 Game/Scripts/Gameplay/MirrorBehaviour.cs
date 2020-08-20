using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBehaviour : MonoBehaviour
{
    [SerializeField] private int repeatTime = 5;
    [SerializeField] private float waveSpeedModificator = 1f;
    [SerializeField] private float waveLifeModificator = 1f;


    public void SpawnWave(Vector3 point)
    {
        var waveBehaviour = LevelController.Instance.CreateWave(point, transform);
        waveBehaviour.speed *= waveSpeedModificator;
        waveBehaviour.lifeTime *= waveLifeModificator;
    }
}
