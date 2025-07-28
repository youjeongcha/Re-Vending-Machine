using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance;

    public string currentThemeID;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void ApplyTheme(string themeId)
    {
        currentThemeID = themeId;

        // Load
        var unlocked = SaveManager.Instance.LoadUnlockedThemes();

        // Add new theme
        if (!unlocked.Contains(themeId))
        {
            unlocked.Add(themeId);
            SaveManager.Instance.SaveUnlockedThemes(unlocked);
        }

        // TODO: 실제 테마 적용 로직
    }

    public bool IsThemeUnlocked(string themeId)
    {
        return SaveManager.Instance.unlockedThemes.Contains(themeId);
    }
}
