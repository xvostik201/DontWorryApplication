using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float _timeToUpdate = 0.6f;
    public string CurrentTime { get; private set; }

    public event Action<string> OnTimeChanged;

    public static TimeManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        InvokeRepeating(nameof(UpdateCurrentTime), 0f, _timeToUpdate);
    }

    private void UpdateCurrentTime()
    {
        CurrentTime = DateTime.Now.ToString("HH:mm:ss");
        OnTimeChanged?.Invoke(CurrentTime);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(UpdateCurrentTime));
    }
}
