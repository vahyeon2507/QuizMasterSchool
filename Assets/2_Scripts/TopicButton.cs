using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TopicButton : MonoBehaviour
{
    [SerializeField] Button topicButton; // 단일 버튼 연결
    [SerializeField] string topic;       // 선택할 토픽명
    [SerializeField] int questionCount = 10;

    void Start()
    {
        if (topicButton != null)
            topicButton.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        PlayerPrefs.SetString("SelectedTopic", topic);
        PlayerPrefs.SetInt("QuestionCount", questionCount);
        PlayerPrefs.Save();
        SceneManager.LoadScene(1); // 빌드 인덱스 1번 씬으로 이동
    }
}
