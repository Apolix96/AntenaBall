using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelPointBehaviour : MonoBehaviour
{
    public int signalsNeeded = 1;

    [Tooltip("Group value, 0 - not in group")]
    public int pointGroup = 0;


    [Header("Refernces")] [SerializeField] private TextMeshPro signalsText;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private LineRenderer groupLine;

    public bool ConditionMet { get; private set; }

    private bool _isOn;
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

    private void Awake()
    {
        signalsText.text = signalsNeeded.ToString();

   
    }

    private void Start()
    {
        if (pointGroup != 0 && nextPoint != null)
        {
            groupLine.SetPosition(0, transform.position);
            groupLine.SetPosition(1, nextPoint.position);
        }
    }

    public void TouchByWave(WaveBehaviour waveBehaviour)
    {
        if (currentWaves.Contains(waveBehaviour)) return;
        currentWaves.Add(waveBehaviour);

        ConditionMet = currentWaves.Count >= signalsNeeded;

        if (pointGroup == 0)
        {
            IsOn = ConditionMet;
        }
        else
        {
            LevelController.Instance.GetGroupCondition(pointGroup);
        }
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