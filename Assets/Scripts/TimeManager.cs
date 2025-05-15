using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float _timeToUpdate = 0.6f;

    public string CurrentTime { get; private set; }

    public static TimeManager Instance;
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);
    }

    private void Start()
    {
        InvokeRepeating("UpdateCurrentTime", 0f, _timeToUpdate);
    }

    private void UpdateCurrentTime()
    {
        CurrentTime = DateTime.Now.ToString("HH:mm:ss");
    }
    private void OnDisable()
    {
        CancelInvoke(nameof(UpdateCurrentTime));
    }
}
