using UnityEngine;
[CreateAssetMenu(menuName = "QuizQuestion",fileName = "NewQuestion")]
public class QuestionSO : ScriptableObject
{

    [TextArea(2, 6)]

    [SerializeField ]string question  = "���⿡ ������ �Է��ϼ���.";
}
