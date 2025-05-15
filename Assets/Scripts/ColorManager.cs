using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    [SerializeField] private Color _redColor = Color.red;
    [SerializeField] private Color _greenColor = Color.green;

    public Color RedColor => _redColor;
    public Color GreenColor => _greenColor;

    public static ColorManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }
}
