using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LevelType
{
    QuestionTest,
    Write
}

[System.Serializable]
public class LevelData
{
    public string canvasName;
    public LevelType type;
    public int difficulty;
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public int currentLevelIndex = 0;
    public int currentDifficulty = 1;

    public List<LevelData> levelPool;

    private void Awake()
    {
        Instance = this;
    }

    public List<LevelData> GetNextLevelOptions()
    {
        List<LevelData> options = new List<LevelData>();

        for (int i = 0; i < 3; i++)
        {
            LevelData randomLevel = levelPool[Random.Range(0, levelPool.Count)];
            options.Add(randomLevel);
        }

        return options;
    }

    public void LoadLevel(LevelData level, GameObject canvas)
    {
        currentLevelIndex++;

        if (currentLevelIndex % 5 == 0)
        {
            // Boss cada 5 niveles
            LoadBoss();
            return;
        }

        canvas.SetActive(true);
    }

    void LoadBoss()
    {
        SceneManager.LoadScene("BossScene");
    }
}
