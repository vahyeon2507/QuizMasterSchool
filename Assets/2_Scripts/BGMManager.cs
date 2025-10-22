using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; }

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip bgmQuiz;
    [SerializeField] AudioClip bgmEnding;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void PlayQuizBGM()
    {
        if (audioSource == null || bgmQuiz == null) return;
        audioSource.clip = bgmQuiz;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayEndingBGM()
    {
        if (audioSource == null || bgmEnding == null) return;
        audioSource.clip = bgmEnding;
        audioSource.loop = true;
        audioSource.Play();
    }
}
