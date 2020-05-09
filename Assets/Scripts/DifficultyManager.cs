using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    #region Singleton
    public static DifficultyManager instance { get; private set; }

    void Awake()
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
    }
    #endregion

    void Start()
    {
        SelectDifficulty("easy");
    }

    private string difficulty;

    public void SelectDifficulty(string _difficulty)
    {
        difficulty = _difficulty;
    }


    private static float ENEMIES_COOLDOWN_SECONDS_EASY = 14;
    private static int ENEMIES_BATCH_AMOUNT_EASY = 7;
    private static int ENEMIES_AMOUNT_EASY = 90;

    private static float ENEMIES_COOLDOWN_SECONDS_MEDIUM = 12;
    private static int ENEMIES_BATCH_AMOUNT_MEDIUM = 8;
    private static int ENEMIES_AMOUNT_MEDIUM = 140;

    private static float ENEMIES_COOLDOWN_SECONDS_HARD = 10;
    private static int ENEMIES_BATCH_AMOUNT_HARD = 9;
    private static int ENEMIES_AMOUNT_HARD = 170;

    public float GetEnemiesCooldownSeconds()
    {
        if (difficulty == "easy")
        {
            return ENEMIES_COOLDOWN_SECONDS_EASY;
        } else if (difficulty == "medium")
        {
            return ENEMIES_COOLDOWN_SECONDS_MEDIUM;
        } else
        {
            return ENEMIES_COOLDOWN_SECONDS_HARD;
        }
    }

    public int GetEnemiesBatchAmount()
    {
        if (difficulty == "easy")
        {
            return ENEMIES_BATCH_AMOUNT_EASY;
        }
        else if (difficulty == "medium")
        {
            return ENEMIES_BATCH_AMOUNT_MEDIUM;
        }
        else
        {
            return ENEMIES_BATCH_AMOUNT_HARD;
        }
    }

    public int GetEnemiesAmount()
    {
        if (difficulty == "easy")
        {
            return ENEMIES_AMOUNT_EASY;
        }
        else if (difficulty == "medium")
        {
            return ENEMIES_AMOUNT_MEDIUM;
        }
        else
        {
            return ENEMIES_AMOUNT_HARD;
        }
    }
}
