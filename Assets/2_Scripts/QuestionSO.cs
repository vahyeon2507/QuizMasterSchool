using UnityEngine;
[CreateAssetMenu(menuName = "QuizQuestion", fileName = "NewQuestion")]
public class QuestionSO : ScriptableObject
{
    [TextArea(2, 6)]
    [SerializeField] string question = "여기에 질문을 입력하세요.";
    [SerializeField] string[] answers = new string[4];
    [SerializeField] int correctAnswerIndex = 0;

    public string GetQuestion()
    {
      return question;
    }

    public string GetAnswer(int index)
    {
        return answers[index];
    }

    public string tCorrectAnswer()
    {
        return answers[correctAnswerIndex];
    }
    
    public int GetCorrectAnswerIndex()
    {
        return correctAnswerIndex;
    }

    public void SetData(string q, string[] a, int correctIndex)
    {
        question = q;
        answers = a;
        correctAnswerIndex = correctIndex;
    }
}
