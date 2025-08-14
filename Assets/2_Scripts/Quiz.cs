using TMPro;
using UnityEngine;

public class Quiz : MonoBehaviour
{
    [SerializeField]  TextMeshProUGUI questionText;
    [SerializeField]  QuestionSO questions;
    void Start()
    {
        questionsText[0].text = questions.GetAnswer(0);
    }

    void Update()
    {
        
    }
}
