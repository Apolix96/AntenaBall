using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeaterBehaviour : MonoBehaviour
{
    [SerializeField] private int repeatTime = 5;
    [SerializeField] private float waveSpeedModificator = 1f;
    [SerializeField] private float waveLifeModificator = 1f;

    private int currentRepeats = 0;

    public void SpawnWave()
    {
        var waveBehaviour = LevelController.Instance.CreateWave(transform.position, transform);
        waveBehaviour.speed *= waveSpeedModificator;
        waveBehaviour.lifeTime *= waveLifeModificator;
    }
}
