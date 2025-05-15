using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button[] _allReasonButton;

    [Header("Reasons")]
    [SerializeField, TextArea(2, 2)] private string[] _reasons;

    [Header("InputField")]
    [SerializeField] private TMP_InputField _otherReasonsInputField;

    [Header("Texts")]
    [SerializeField] private TMP_Text _currentTimeText;
    [SerializeField] private TMP_Text _lastTimeText;

    [Header("Background")]
    [SerializeField] private Image _background;

    private const string _prefsType = "Type";

    private void Awake()
    {
        CheckElapsedTime();
        CheckLastTime();
        InstantiateTypesToButtons();
    }
    private void OnEnable()
    {
        _currentTimeText.text = "Текущее время: " + TimeManager.Instance.CurrentTime;
        TimeManager.Instance.OnTimeChanged += TimeChanged;
    }
    private void OnDisable()
    {
        TimeManager.Instance.OnTimeChanged -= TimeChanged;
    }

    private void TimeChanged(string newTime)
    {
        _currentTimeText.text = "Текущее время: " + newTime;
    }
    private void SaveLastTag(int typeIndex)
    {
        SaveLastTagInternal(_reasons[typeIndex]);
    }

    private void SaveLastTag(string otherType)
    {
        SaveLastTagInternal(otherType);
    }

    private void SaveLastTagInternal(string tag)
    {
        PlayerPrefsManager.SaveStringPrefs(_prefsType, tag);
        PlayerPrefsManager.SaveLastDateTime(DateTime.Now);

        CheckElapsedTime();
        CheckLastTime();
    }

    private void InstantiateTypesToButtons()
    {
        for (int i = 0; i < _allReasonButton.Length; i++)
        {
            int index = i;
            _allReasonButton[i].onClick.AddListener(() => SaveLastTag(index));
        }
        _otherReasonsInputField.onSubmit.AddListener(SaveLastTag);
    }

    private void CheckElapsedTime()
    {
        if (!PlayerPrefsManager.TryGetLastDateTime(out var savedDt))
            return;

        var now = DateTime.Now;
        var elapsed = now - savedDt;

        int days = elapsed.Days;               
        int hours = elapsed.Hours;              
        int minutes = elapsed.Minutes;           

        bool isAway = days > 0 || hours >= 2;
        _background.color = isAway
            ? ColorManager.Instance.RedColor
            : ColorManager.Instance.GreenColor;
    }

    private void CheckLastTime()
    {
        if (!PlayerPrefsManager.TryGetLastDateTime(out var savedDt))
        {
            _lastTimeText.text = "Не было сохранений";
            return;
        }

        var elapsed = DateTime.Now - savedDt;
        int days = elapsed.Days;
        int hours = elapsed.Hours;
        int minutes = elapsed.Minutes;

        string tag = PlayerPrefsManager.GetStringSave(_prefsType);

        _lastTimeText.text = string.Format(
            "С последнего тега прошло {0} ч. {1} мин. с пометкой «{2}»",
             hours, minutes, tag
        );
    }

}
