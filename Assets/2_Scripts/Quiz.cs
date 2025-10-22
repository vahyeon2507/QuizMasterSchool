using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Quiz : MonoBehaviour
{
    [Header("����")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("�亯")]
    [SerializeField] TextMeshProUGUI[] answersText;
    [SerializeField] GameObject[] answerButtons;

    [Header("��ư ��������Ʈ")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Ÿ�̸�")]
    [SerializeField] Image timerImage;
    [SerializeField] Sprite ProblemTimerSprite;
    [SerializeField] Sprite SolutionTimerSprite;
    Timer timer;

    [Header("����")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] ScoreKeeper scoreKeeper;
    [SerializeField] TextMeshProUGUI CorrectRatetext;

    [Header("���� ��")]
    [SerializeField] Slider progressBar;

    [Header("ChatGPT Client")]
    [SerializeField] ChatGPTClient chatGPTClient;
    [SerializeField] int QuestionCount = 10;
    [SerializeField] TextMeshProUGUI LoadingText;

    [Header("��Ʈ")]
    [SerializeField] TextMeshProUGUI Hint;

    [Header("�����")]
    [SerializeField] AudioSource audioSource; // AudioSource ����
    [SerializeField] AudioClip correctSound;  // ���� �Ҹ�
    [SerializeField] AudioClip wrongSound;    // ���� �Ҹ�

    bool chooseAnswer = false;
    bool isGenerateQuestions = false;


    void Start()
    {
        timer = FindFirstObjectByType<Timer>();
        scoreKeeper = FindFirstObjectByType<ScoreKeeper>();
        InitializeProgressbar();
        chatGPTClient.quizGenerateHandler += QuizGeneratedHandler;

        string selectedTopic = PlayerPrefs.GetString("SelectedTopic", "�Ϲݻ��");
        int questionCount = PlayerPrefs.GetInt("QuestionCount", 10);

        // chatGPTClient�� �ν����Ϳ� ����Ǿ� �ִٰ� ����
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
        string[] topics = new string[] { "K-Pop", "���� ����" };
        int randomindex = UnityEngine.Random.Range(0, topics.Length);
        return topics[randomindex];
    }


    void QuizGeneratedHandler(List<QuestionSO> GeneratedQuestions)
    {
        Debug.Log("3. QuizGeneratedHandler called with " + GeneratedQuestions.Count + " questions.");
        isGenerateQuestions = false;

        if (GeneratedQuestions == null || GeneratedQuestions.Count == 0)
        {
            Debug.LogError("���� ������ �����߽��ϴ�..");
            LoadingText.text = ("���� ������ �����߽��ϴ�.");
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

        // 5�� ������ �� ��Ʈ ǥ��
        if (timer.isProblemTime && timer.fillAmount * timer.ProblemTime <= 5f && Hint != null)
        {
            if (currentQuestion != null)
                Hint.text = currentQuestion.GetHint();
        }
        else if (Hint != null)
        {
            Hint.text = ""; // �� �ܿ��� ��Ʈ ����
        }

        if (timer.loadNextQuestion)
        {
            if (questions.Count == 0)
            {
                // ������ ���� �� EndingCanvas ����
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
            Debug.Log("�� �̻� ������ �����ϴ�.");
            // ������ ���� �� EndingCanvas ����
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

        // ����/���� �Ҹ� ���
        if ((index + 1) == (currentQuestion.GetCorrectAnswerIndex() + 1))
        {
            PlaySound(correctSound); // ���� �Ҹ�
        }
        else
        {
            PlaySound(wrongSound); // ���� �Ҹ�
        }

        // ���� �ð��� ������ ������ �� ���� �ð� ��� ����
        if ((index + 1) == (currentQuestion.GetCorrectAnswerIndex() + 1) && timer.isProblemTime)
        {
            int maxScore = 100; // ������ �ִ� ����
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
            questionText.text = "�����Դϴ�.";
            answerButtons[index].GetComponent<Image>().sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswers();
        }
        else
        {
            questionText.text = "Ʋ�Ƚ��ϴ�. ������ " + (currentQuestion.GetCorrectAnswerIndex() + 1) + "���Դϴ�.";
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
