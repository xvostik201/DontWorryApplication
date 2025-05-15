using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class PlayerPrefsManager
{
    private const string KeyLastDateTime = "LastDateTime";

    public static void SaveLastDateTime(DateTime dt)
    {
        PlayerPrefs.SetString(KeyLastDateTime, dt.ToString("O", CultureInfo.InvariantCulture));
        PlayerPrefs.Save();
    }

    public static bool TryGetLastDateTime(out DateTime result)
    {
        result = default;
        if (!PlayerPrefs.HasKey(KeyLastDateTime))
            return false;

        var str = PlayerPrefs.GetString(KeyLastDateTime);
        if (DateTime.TryParseExact(str, "O", CultureInfo.InvariantCulture,
            DateTimeStyles.RoundtripKind, out result))
            return true;

        Debug.LogWarning($"Не удалось распознать дату «{str}»");
        return false;
    }
    public static string GetStringSave(string name)
    {
        return PlayerPrefs.GetString(name);
    }

    public static void SaveStringPrefs(string name, string value)
    {
        PlayerPrefs.SetString(name, value);
        PlayerPrefs.Save();
    }

    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }
}
