using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button[] _allReasonButton;
    [SerializeField, TextArea(2, 2)] private string[] _reasons;
    [SerializeField] private TMP_Text[] _timesText; // 0 — текущее, 1 — последнее
    [SerializeField] private Image _background;
    [SerializeField] private Color[] _colors; // 0 - green, 1 -red

    private void Awake()
    {
        CheckElapsedTime();
        CheckLastTime();
        InstantiateTypesToButtons();
    }

    private void Update()
    {
        _timesText[0].text = "Текущее время: " + DateTime.Now.ToString("HH:mm:ss");
    }

    private void SaveLastTag(int type)
    {
        string time = DateTime.Now.ToString("HH:mm:ss");
        PlayerPrefs.SetString("Type", _reasons[type]);
        PlayerPrefs.SetString("LastTime", time);
        PlayerPrefs.Save();

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
    }

    private void CheckElapsedTime()
    {
        if (PlayerPrefs.HasKey("LastTime"))
        {
            string saved = PlayerPrefs.GetString("LastTime");
            if (TimeSpan.TryParseExact(saved, @"hh\:mm\:ss", CultureInfo.InvariantCulture, out TimeSpan savedTime))
            {
                TimeSpan nowTime = DateTime.Now.TimeOfDay;
                TimeSpan elapsed = nowTime - savedTime;
                if (elapsed < TimeSpan.Zero)
                    elapsed += TimeSpan.FromDays(1);

                _background.color = elapsed.TotalHours > 2
                    ? _colors[1]
                    : _colors[0];
            }
            else
            {
                Debug.LogWarning("Неправильный формат сохранённого времени: " + saved);
            }
        }
    }

    private void CheckLastTime()
    {
        if (PlayerPrefs.HasKey("LastTime"))
        {
            _timesText[1].text = $"Последний тег был в {PlayerPrefs.GetString("LastTime")} с пометкой «{PlayerPrefs.GetString("Type")}»";
        }
        else
        {
            _timesText[1].text = "Не было сохранений";
        }
    }

}
