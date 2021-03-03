using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;
using UnityEngine.UI;

public class LaserBangLevel : LevelManager
{

    [SerializeField] private Text scoreText = null;
    private int levelScore = 0;

    private new void Awake()
    {
        base.Awake();
        SetScoreText();
    }

    public override void FinishLevel(bool success)
    {
        GameManager.Instance.FinishLevel(success);
        if(success)
        {
            GameManager.Instance.Gold += levelScore;
            scoreText.text = "Gold: " + GameManager.Instance.Gold.ToString();
        }
    }

    public override void StartLevel()
    {
        print("start lvl");
    }

    public void AddScore(int amount)
    {
        levelScore += 1;
        SetScoreText();
    }

    private void SetScoreText()
    {
        scoreText.text = "Gold: " + GameManager.Instance.Gold.ToString() + " + " + levelScore.ToString();
    }

}
