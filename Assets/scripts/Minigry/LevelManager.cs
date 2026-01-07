using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Level Data")]
    public int memoryLevel = 1;
    public int spamLevel = 1;
    public int laneDodgeLevel = 1;

    [Header("Highest Level Data")]
    public int highestLevelMemory = 1;
    public int highestLevelSpam = 1;
    public int highestLevelLaneDodge = 1;

    [Header("Other Data")]
    public int totalPoints = 0;

    public Text memoryLevelText;
    public Text spamLevelText;
    public Text laneDodgeLevelText;

    public PlayerLife playerLife;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadHighestLevels();
    }

    private void LoadHighestLevels()
    {
        highestLevelMemory = PlayerPrefs.GetInt("HighestMemoryLevel", highestLevelMemory);
        highestLevelSpam = PlayerPrefs.GetInt("HighestSpamLevel", highestLevelSpam);
        highestLevelLaneDodge = PlayerPrefs.GetInt("HighestLaneDodgeLevel", highestLevelLaneDodge);

        playerLife.hp = 150 + (highestLevelMemory * 1.5f);
    }

    private void SaveHighestLevels()
    {
        playerLife.hp = 150 + (highestLevelMemory * 1.5f);
        PlayerPrefs.SetInt("HighestMemoryLevel", highestLevelMemory);
        PlayerPrefs.SetInt("HighestSpamLevel", highestLevelSpam);
        PlayerPrefs.SetInt("HighestLaneDodgeLevel", highestLevelLaneDodge);
        PlayerPrefs.Save();
    }

    public void LevelUpMemory()
    {
        memoryLevel++;
        if (memoryLevel > highestLevelMemory)
        {
            highestLevelMemory = memoryLevel;
            SaveHighestLevels();
        }
        DisplayHighestLevels();
    }

    public void LevelUpSpam()
    {
        spamLevel++;
        if (spamLevel > highestLevelSpam)
        {
            highestLevelSpam = spamLevel;
            SaveHighestLevels();
        }
        DisplayHighestLevels();
    }

    public void LevelUpLaneDodge()
    {
        laneDodgeLevel++;
        if (laneDodgeLevel > highestLevelLaneDodge)
        {
            highestLevelLaneDodge = laneDodgeLevel;
            SaveHighestLevels();
        }
        DisplayHighestLevels();
    }

    public void UpdateLaneDodgeScore(int finalScore)
    {
        if (finalScore > highestLevelLaneDodge)
        {
            highestLevelLaneDodge = finalScore;
            SaveHighestLevels();
        }
        DisplayHighestLevels(); // To odœwie¿y teksty natychmiast
    }

    public void ResetLevels()
    {
        memoryLevel = 1;
        spamLevel = 1;
        laneDodgeLevel = 1;
        totalPoints = 0;

        highestLevelMemory = 1;
        highestLevelSpam = 1;
        highestLevelLaneDodge = 1;

        SaveHighestLevels();
    }

    public void DisplayHighestLevels()
    {
        if (memoryLevelText != null)
            memoryLevelText.text = "Zwiêkszone obra¿enia: +" + highestLevelMemory + "%";

        if (spamLevelText != null)
            spamLevelText.text = "Szybsze ataki: +" + highestLevelSpam + "%";

        if (laneDodgeLevelText != null)
            laneDodgeLevelText.text = "Zwiêkszone zdrowie: +" + highestLevelLaneDodge + "%";
    }

    private void Start()
    {
        DisplayHighestLevels();
    }
}
