using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] QuestionSO questions;
    [SerializeField] TextMeshProUGUI[] answersText;
    [SerializeField] GameObject[] answerButtons;
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    void Start()
    {
        GetNextQuestion();
    }

    private void GetNextQuestion()
    {
        OnDisplayQuestion();
        SetDefaultButtonSprites();
        SetButtonState(true);
    }

    private void OnDisplayQuestion()
    {
        questionText.text = questions.GetQuestion();

        for (int i = 0; i < answersText.Length; i++)
        {
            answersText[i].text = questions.GetAnswer(i);
        }
    }

    public void OnAnswerButtonClicked(int index)
    {
        if ((index + 1) == (questions.GetCorrectAnswerIndex() + 1))
        {
            questionText.text = "정답입니다.";
            answerButtons[index].GetComponent<Image>().sprite = correctAnswerSprite;
        }
        else
        {
            questionText.text = "틀렸습니다. 정답은 " + (questions.GetCorrectAnswerIndex() + 1) + "번입니다.";
        }

        SetButtonState(false);
    }

    private void SetButtonState(bool state)
    {
        foreach (GameObject obj in answerButtons)
        {
            obj.GetComponent<Button>().interactable = state;
        }
    }

    private void SetDefaultButtonSprites()
    {
        foreach (GameObject obj in answerButtons)
        {
            obj.GetComponent<Image>().sprite = defaultAnswerSprite;
        }
    }
}
