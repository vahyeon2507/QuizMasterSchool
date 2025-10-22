using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Quiz Quiz;
    [SerializeField] private EndingCanvas EndingCanvas;
    [SerializeField] private GameObject LoadingCanvas;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
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
        LoadingCanvas.SetActive(false);
        BGMManager.Instance?.PlayQuizBGM();
    }

    public void ShowEndingScene()
    {
        Quiz.gameObject.SetActive(false);
        EndingCanvas.gameObject.SetActive(true);
        EndingCanvas.ShowEnding();
        LoadingCanvas.SetActive(false);
        BGMManager.Instance?.PlayEndingBGM();
    }

    void Update()
    {

    }


    public void OnReplayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowLoadingScene()
    {
        Quiz.gameObject.SetActive(false);
        EndingCanvas.gameObject.SetActive(false);
        LoadingCanvas.SetActive(true);
    }
}


