using UnityEngine;

public class ScoreBoard
{
    private const string RedScoreKey = "RedScore";
    private const string BlueScoreKey = "BlueScore";

    private const int ScoreDefault = 0;

    public void SetRedScore(int score)
    {
        PlayerPrefs.SetInt(RedScoreKey, score);
    }
    
    public int GetRedScore()
    {
        if (!PlayerPrefs.HasKey(RedScoreKey))
            PlayerPrefs.SetInt(RedScoreKey, ScoreDefault);
        return PlayerPrefs.GetInt(RedScoreKey);
    }
    
    public void SetBlueScore(int score)
    {
        PlayerPrefs.SetInt(BlueScoreKey, score);
    }
    
    public int GetBlueScore()
    {
        if (!PlayerPrefs.HasKey(BlueScoreKey))
            PlayerPrefs.SetInt(BlueScoreKey, ScoreDefault);
        return PlayerPrefs.GetInt(BlueScoreKey);
    }

    public void ResetScores()
    {
        SetRedScore(ScoreDefault);
        SetBlueScore(ScoreDefault);
    }

}
