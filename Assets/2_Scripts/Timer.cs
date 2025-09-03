using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float problemTime = 10f;
    [SerializeField] float solutionTime = 3f;
    float time = 0f;

    [HideInInspector] public bool isProblemTime = true;
    [HideInInspector] public float fillAmount = 1f;
    [HideInInspector] public bool loadNextQuestion;

    private void Start()
    {
        time = problemTime;
    }
    private void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0f)
        {
            if (isProblemTime)
            {
                Debug.Log("이제부터 솔루션 타입입니다.");
                isProblemTime = false;
                time = solutionTime;
            }
            else
            {
                Debug.Log("이제부터 문제 타입입니다.");
                isProblemTime = true;
                time = problemTime;
                loadNextQuestion = true; 
            }
        }
        if (isProblemTime)
        {
            fillAmount = time / problemTime;
        }
        else
        {
            fillAmount = time / solutionTime;
        }

    }
    public void CancleTimer()
    {
        time = 0f;
    }
}
