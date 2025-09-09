using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Quiz : MonoBehaviour
{
    [Header("질문")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("답변")]
    [SerializeField] TextMeshProUGUI[] answersText;
    [SerializeField] GameObject[] answerButtons;

    [Header("버튼 스프라이트")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("타이머")]
    [SerializeField] Image timerImage;
    [SerializeField] Sprite ProblemTimerSprite;
    [SerializeField] Sprite SolutionTimerSprite;
    Timer timer;

    [Header("점수")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] ScoreKeeper scoreKeeper;

    [Header("진행 바")]
    [SerializeField] Slider progressBar;

    bool chooseAnswer = false;
    bool isGenerateQuestions = false; 
    

    void Start()
    {
        timer = FindFirstObjectByType<Timer>();
        scoreKeeper = FindFirstObjectByType<ScoreKeeper>();
        InitializeProgressbar();


        if (questions.Count == 0)
        {
            GenerateQuestionsIfNeeded();
                
        }
        else
        {
            InitializeProgressbar();
        }

        GetNextQuestion();
    }

    private void GenerateQuestionsIfNeeded()
    {
        if (isGenerateQuestions) return;



        isGenerateQuestions = true;
        GameManager.Instance.ShowLoadingScene();
    }

    private void InitializeProgressbar()
    {
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    private void Update()
    {
        if (timer.isProblemTime)
        {
            timerImage.sprite = ProblemTimerSprite;
        }
        else
        {
            timerImage.sprite = SolutionTimerSprite;
        }
        timerImage.fillAmount = timer.fillAmount;
        if (timer.loadNextQuestion)
        {
            if (questions.Count == 0)
            {
                GenerateQuestionsIfNeeded();
                // GameManager.Instance.ShowEndingScene();
            }
            else
            {
                timer.loadNextQuestion = false;
                chooseAnswer = false;
                GetNextQuestion();
            }

        }
        if (timer.isProblemTime == false && chooseAnswer == false)
        {
            DisplaySolution(-1);
        }
    }

    private void GetNextQuestion()
    {
        if (questions.Count == 0)
        {
            Debug.Log("더 이상 질문이 없습니다.");
            return;
        }
        chooseAnswer = false;
        GetRandomQuestion();
        OnDisplayQuestion();
        SetDefaultButtonSprites();
        SetButtonState(true);
        scoreKeeper.IncrementSeenQuestion();
        progressBar.value++;
    }

    private void GetRandomQuestion()
    {
        int randomIndex = UnityEngine.Random.Range(0, questions.Count);
        currentQuestion = questions[randomIndex];
        questions.RemoveAt(randomIndex);
    }

    private void OnDisplayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();
        for (int i = 0; i < answersText.Length; i++)
        {
            answersText[i].text = currentQuestion.GetAnswer(i);
        }
    }
    public void OnAnswerButtonClicked(int index)
    {
        chooseAnswer = true;
        DisplaySolution(index);
        timer.CancleTimer();
        scoreText.text = $"Score: {scoreKeeper.CalculateScorePercentage()}%";

    }

    private void DisplaySolution(int index)
    {
        if ((index + 1) == (currentQuestion.GetCorrectAnswerIndex() + 1))
        {
            questionText.text = "정답입니다.";
            answerButtons[index].GetComponent<Image>().sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswers();
        }
        else
        {
            questionText.text = "틀렸습니다. 정답은 " + (currentQuestion.GetCorrectAnswerIndex() + 1) + "번입니다.";
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
