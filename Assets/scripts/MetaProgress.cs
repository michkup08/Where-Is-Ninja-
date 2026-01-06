using UnityEngine;

public static class MetaProgress
{
    private const string LaneBestKey = "LaneDodge_Best";

    public static int LaneBest => PlayerPrefs.GetInt(LaneBestKey, 0);

    public static bool TrySetLaneBest(int value)
    {
        value = Mathf.Max(0, value);
        int cur = LaneBest;
        if (value <= cur) return false;

        PlayerPrefs.SetInt(LaneBestKey, value);
        PlayerPrefs.Save();
        return true;
    }

    // Bonus dmg: np. +5% za poziom, max x2
    public static float GetLaneDamageMultiplier(float perLevel = 0.05f, float max = 2.0f)
    {
        float mult = 1f + LaneBest * Mathf.Max(0f, perLevel);
        return Mathf.Clamp(mult, 1f, Mathf.Max(1f, max));
    }
}
