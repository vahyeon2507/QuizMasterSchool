using TMPro;
using UnityEngine;

public class Quiz : MonoBehaviour
{
    [SerializeField]  TextMeshProUGUI questionText;
    [SerializeField]  QuestionSO questions;
    [SerializeField]  TextMeshProUGUI[] answersText;   
    void Start()
    {
        questionText.text = questions.GetAnswer(0);

        for (int i = 0; i < answersText.Length; i++)
        {
            answersText[i].text = questions.GetAnswer(i);
        }
    }

    void Update()
    {
        
    }
}
