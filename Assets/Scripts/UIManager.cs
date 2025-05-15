using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button[] _allReasonButton;
    [SerializeField, TextArea(2, 2)] private string[] _reasons;
    [SerializeField] private TMP_InputField _otherReasons;
    [SerializeField] private TMP_Text[] _timesText;
    [SerializeField] private Image _background;

    private const string _prefsType = "Type";

    private void Awake()
    {
        CheckElapsedTime();
        CheckLastTime();
        InstantiateTypesToButtons();
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
        _otherReasons.onSubmit.AddListener(SaveLastTag);
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
            _timesText[1].text = "Не было сохранений";
            return;
        }

        var elapsed = DateTime.Now - savedDt;
        int days = elapsed.Days;
        int hours = elapsed.Hours;
        int minutes = elapsed.Minutes;

        string tag = PlayerPrefsManager.GetStringSave(_prefsType);

        _timesText[1].text = string.Format(
            "С последнего тега прошло {0} ч. {1} мин. с пометкой «{2}»",
             hours, minutes, tag
        );
    }

}
