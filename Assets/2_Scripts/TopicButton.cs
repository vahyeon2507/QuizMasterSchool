using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TopicButton : MonoBehaviour
{
    [SerializeField] Button topicButton; // ���� ��ư ����
    [SerializeField] string topic;       // ������ ���ȸ�
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
        SceneManager.LoadScene(1); // ���� �ε��� 1�� ������ �̵�
    }
}
