using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

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
    [SerializeField] TextMeshProUGUI CorrectRatetext;

    [Header("진행 바")]
    [SerializeField] Slider progressBar;

    [Header("ChatGPT Client")]
    [SerializeField] ChatGPTClient chatGPTClient;
    [SerializeField] int QuestionCount = 10;
    [SerializeField] TextMeshProUGUI LoadingText;

    [Header("힌트")]
    [SerializeField] TextMeshProUGUI Hint;

    [Header("오디오")]
    [SerializeField] AudioSource audioSource; // AudioSource 연결
    [SerializeField] AudioClip correctSound;  // 정답 소리
    [SerializeField] AudioClip wrongSound;    // 오답 소리

    bool chooseAnswer = false;
    bool isGenerateQuestions = false;


    void Start()
    {
        timer = FindFirstObjectByType<Timer>();
        scoreKeeper = FindFirstObjectByType<ScoreKeeper>();
        InitializeProgressbar();
        chatGPTClient.quizGenerateHandler += QuizGeneratedHandler;

        string selectedTopic = PlayerPrefs.GetString("SelectedTopic", "일반상식");
        int questionCount = PlayerPrefs.GetInt("QuestionCount", 10);

        // chatGPTClient가 인스펙터에 연결되어 있다고 가정
        chatGPTClient.GenerateQuizQuestions(questionCount, selectedTopic);

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

        string topicToUse = GetTrendingTopic();
        chatGPTClient.GenerateQuizQuestions(QuestionCount, topicToUse);
        Debug.Log($"2. Generating {QuestionCount} questions about {topicToUse}");
    }

    private string GetTrendingTopic()
    {
        string[] topics = new string[] { "K-Pop", "비디오 게임" };
        int randomindex = UnityEngine.Random.Range(0, topics.Length);
        return topics[randomindex];
    }


    void QuizGeneratedHandler(List<QuestionSO> GeneratedQuestions)
    {
        Debug.Log("3. QuizGeneratedHandler called with " + GeneratedQuestions.Count + " questions.");
        isGenerateQuestions = false;

        if (GeneratedQuestions == null || GeneratedQuestions.Count == 0)
        {
            Debug.LogError("문제 생성에 실패했습니다..");
            LoadingText.text = ("문제 생성에 실패했습니다.");
            return;
        }


        questions.AddRange(GeneratedQuestions);
        progressBar.maxValue += GeneratedQuestions.Count;

        GameManager.Instance.ShowQuizScene();
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

        // 5초 이하일 때 힌트 표시
        if (timer.isProblemTime && timer.fillAmount * timer.ProblemTime <= 5f && Hint != null)
        {
            if (currentQuestion != null)
                Hint.text = currentQuestion.GetHint();
        }
        else if (Hint != null)
        {
            Hint.text = ""; // 그 외에는 힌트 숨김
        }

        if (timer.loadNextQuestion)
        {
            if (questions.Count == 0)
            {
                // 문제가 없을 때 EndingCanvas 열기
                GameManager.Instance.ShowEndingScene();
                return;
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
            // 문제가 없을 때 EndingCanvas 열기
            GameManager.Instance.ShowEndingScene();
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

        // 정답/오답 소리 재생
        if ((index + 1) == (currentQuestion.GetCorrectAnswerIndex() + 1))
        {
            PlaySound(correctSound); // 정답 소리
        }
        else
        {
            PlaySound(wrongSound); // 오답 소리
        }

        // 문제 시간에 정답을 맞췄을 때 남은 시간 비례 점수
        if ((index + 1) == (currentQuestion.GetCorrectAnswerIndex() + 1) && timer.isProblemTime)
        {
            int maxScore = 100; // 문제당 최대 점수
            int point = Mathf.RoundToInt(maxScore * timer.fillAmount);
            scoreKeeper.AddScorePoint(point);
        }

        scoreText.text = $"Score: {scoreKeeper.GetScorePoint()}";
        CorrectRatetext.text = $"CorrectRate: {scoreKeeper.CorrectRate()}%";
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

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
