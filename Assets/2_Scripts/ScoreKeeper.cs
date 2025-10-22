using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    int correctAnswers = 0; 
    int SeenQuestion = 0;
    int scorePoint = 0; // ���� ������ �ʵ� �߰�

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

    // ����� ��ȯ �޼���
    public int CorrectRate()
    {
        if (SeenQuestion == 0) return 0;
        return Mathf.RoundToInt((float)correctAnswers / (SeenQuestion-1) * 100);
    }

    // ���� ���� �޼���
    public void AddScorePoint(int point)
    {
        scorePoint += point;
    }

    // ���� ���� ��ȯ �޼���
    public int GetScorePoint()
    {
        return scorePoint;
    }

    public int ScorePoint()
    {
        return correctAnswers * 10;
    }
}
