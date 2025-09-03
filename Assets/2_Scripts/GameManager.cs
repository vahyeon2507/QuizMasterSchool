using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Quiz Quiz;
    [SerializeField] private EndingCanvas EndingCanvas;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ShowQuizScene();

    }

    public void ShowQuizScene()
    {
        Quiz.gameObject.SetActive(true);
        EndingCanvas.gameObject.SetActive(false);
    }

    public void ShowEndingScene()
    {
        Quiz.gameObject.SetActive(false);
        EndingCanvas.gameObject.SetActive(true);
        EndingCanvas.ShowEnding();
    }
    void Update()
    {

    }


    public void OnReplayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

//
