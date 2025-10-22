using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    int correctAnswers = 0; 
    int SeenQuestion = 0;
    int scorePoint = 0; // 점수 누적용 필드 추가

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

    // 정답률 반환 메서드
    public int CorrectRate()
    {
        if (SeenQuestion == 0) return 0;
        return Mathf.RoundToInt((float)correctAnswers / (SeenQuestion-1) * 100);
    }

    // 점수 누적 메서드
    public void AddScorePoint(int point)
    {
        scorePoint += point;
    }

    // 현재 점수 반환 메서드
    public int GetScorePoint()
    {
        return scorePoint;
    }

    public int ScorePoint()
    {
        return correctAnswers * 10;
    }
}
