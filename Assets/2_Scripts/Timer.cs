using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] float problemTime = 10f;
    [SerializeField] float solutionTime = 3f;
    float time = 0f;

    public float ProblemTime => problemTime; // 읽기 전용 프로퍼티 추가

    public bool isProblemTime = true;
    public float fillAmount = 1f;
    public bool loadNextQuestion;

    [SerializeField] Image timerImage; // UI 이미지 연결
    [SerializeField] TextMeshProUGUI timerText; // 남은 시간 표시용 TMP 연결

    public void Start()
    {
        time = problemTime;
        loadNextQuestion = true;
    }
    public void Update()
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

        // 이미지 색상 변경
        if (isProblemTime)
        {
            if (fillAmount > 0.6f)
                timerImage.color = Color.green;
            else if (fillAmount > 0.2f)
                timerImage.color = Color.yellow;
            else
                timerImage.color = Color.red;
        }
        else
        {
            timerImage.color = Color.white;
        }

        // 남은 시간 TMP에 정수로 출력
        if (timerText != null)
        {
            int displayTime = Mathf.CeilToInt(time);
            timerText.text = displayTime.ToString();
        }
    }
    public void CancleTimer()
    {
        time = 0f;
    }
}
