using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] float problemTime = 10f;
    [SerializeField] float solutionTime = 3f;
    float time = 0f;

    public float ProblemTime => problemTime; // �б� ���� ������Ƽ �߰�

    public bool isProblemTime = true;
    public float fillAmount = 1f;
    public bool loadNextQuestion;

    [SerializeField] Image timerImage; // UI �̹��� ����
    [SerializeField] TextMeshProUGUI timerText; // ���� �ð� ǥ�ÿ� TMP ����

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
                Debug.Log("�������� �ַ�� Ÿ���Դϴ�.");
                isProblemTime = false;
                time = solutionTime;
            }
            else
            {
                Debug.Log("�������� ���� Ÿ���Դϴ�.");
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

        // �̹��� ���� ����
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

        // ���� �ð� TMP�� ������ ���
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
