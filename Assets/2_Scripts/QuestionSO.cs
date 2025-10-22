using UnityEngine;
[CreateAssetMenu(menuName = "QuizQuestion", fileName = "NewQuestion")]
public class QuestionSO : ScriptableObject
{
    [TextArea(2, 6)]
    [SerializeField] string question = "여기에 질문을 입력하세요.";
    [SerializeField] string[] answers = new string[4];
    [SerializeField] int correctAnswerIndex = 0;
    [SerializeField] string hint = "";

    public string GetQuestion() => question;
    public string GetAnswer(int index) => answers[index];
    public string tCorrectAnswer() => answers[correctAnswerIndex];
    public int GetCorrectAnswerIndex() => correctAnswerIndex;
    public string GetHint() => hint;
    public void SetData(string q, string[] a, int correctIndex, string h = "")
    {
        question = q;
        answers = a;
        correctAnswerIndex = correctIndex;
        hint = h;
    }
}
