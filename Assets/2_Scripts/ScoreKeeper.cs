using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    int correctAnswers = 0; 
    int SeenQuestion = 0;
    public int GetCorrectAnswers()
    {
        return correctAnswers;
    }
    public void IncrementCorrectAnswers()
    {
        correctAnswers++;
    }
    public int GetSeenQuestion()
    {
        return SeenQuestion;
    }
    public void IncrementSeenQuestion()
    {
        SeenQuestion++;
    }
    public int CalculateScorePercentage()
    {
        return Mathf.RoundToInt((float)correctAnswers / SeenQuestion * 100);
    }
}
