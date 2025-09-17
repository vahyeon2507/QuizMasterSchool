using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class ChatGPTRequest
{
    public string model = "gpt-4.1-nano";
    public Message[] messages;
    public float temperature = 1.1f;
    public int max_completion_tokens = 4000;
}

[Serializable]
public class Message
{
    public string role;
    public string content;
}

[Serializable]
public class ChatGPTResponse
{
    public Choice[] choices;
}

[Serializable]
public class Choice
{
    public Message message;
}

[Serializable]
public class QuizData
{
    public QuizQuestion[] questions;
}

[Serializable]
public class QuizQuestion
{
    public string question;
    public string[] answers;
    public int correctAnswerIndex;
}

public class ChatGPTClient : MonoBehaviour
{
    private const string API_URL = "https://api.openai.com/v1/chat/completions";
    private string apiKey = "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";

    public delegate void QuizGenerateHandler(List<QuestionSO> questions);
    public event QuizGenerateHandler quizGenerateHandler;

    public void GenerateQuizQuestions(int count = 3, string topic = "�Ϲݻ��")
    {
        StartCoroutine(RequestQuizQuestions(count, topic));
    }

    private IEnumerator RequestQuizQuestions(int count, string topic)
    {
        string prompt = $"���� ���ǿ� �´� â�����̰� ����ִ� ������ ���� ������ {count}�� �������ּ���:\n" +
                       $"����: {topic}\n" +
                       "����:\n" +
                       "- �� ������ 4���� ��â���̰� ������ �������� ������ �մϴ�\n" +
                       "- ������ �پ��� ���̵��� �������� �������ּ��� (��������, �߷й���, �������, â���� ��� ��)\n" +
                       "- �������� ������ �ְų� ��ġ�ְ� �������ּ���\n" +
                       "- ������ 0~3 ������ �ε����� ǥ�����ּ���\n" +
                       "- ������ �������� ��̷Ӱ� �����ϰ� �Ͱ� ������ּ���\n" +
                       "- �����ϸ� �ǻ�Ȱ�� ������ ���ó� �ó������� Ȱ�����ּ���\n" +
                       "- ������ �ݵ�� ���� JSON �������θ� �������ּ���:\n" +
                       "{\n" +
                       "  \"questions\": [\n" +
                       "    {\n" +
                       "      \"question\": \"���� ����\",\n" +
                       "      \"answers\": [\"������1\", \"������2\", \"������3\", \"������4\"],\n" +
                       "      \"correctAnswerIndex\": 0\n" +
                       "    }\n" +
                       "  ]\n" +
                       "}";

        Debug.Log("Prompt to ChatGPT:\n" + prompt);

        ChatGPTRequest request = new ChatGPTRequest
        {
            messages = new Message[]
            {
                new Message { role = "user", content = prompt }
            }
        };

        string jsonRequest = JsonUtility.ToJson(request);
        Debug.Log("Request JSON:\n" + jsonRequest);

        using (UnityWebRequest webRequest = new UnityWebRequest(API_URL, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequest);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", $"Bearer {apiKey}");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    Debug.Log("Raw response from ChatGPT:\n" + webRequest.downloadHandler.text);
                    ChatGPTResponse response = JsonUtility.FromJson<ChatGPTResponse>(webRequest.downloadHandler.text);

                    if (response == null || response.choices == null || response.choices.Length == 0)
                    {
                        Debug.LogError("Invalid response structure from ChatGPT API");
                        yield break;
                    }

                    if (response.choices[0].message == null)
                    {
                        Debug.LogError("Message content is null in ChatGPT response");
                        yield break;
                    }

                    string jsonContent = response.choices[0].message.content;

                    if (string.IsNullOrEmpty(jsonContent))
                    {
                        Debug.LogError("Content is empty. Finish reason: " + response.choices[0].message);
                        Debug.LogError("Consider increasing max_completion_tokens");
                        yield break;
                    }

                    Debug.Log("Response from ChatGPT:\n" + jsonContent);
                    // JSON ���ڿ����� ���ʿ��� �κ� ����
                    jsonContent = jsonContent.Trim();
                    if (jsonContent.StartsWith("```json"))
                    {
                        jsonContent = jsonContent.Substring(7);
                    }
                    if (jsonContent.EndsWith("```"))
                    {
                        jsonContent = jsonContent.Substring(0, jsonContent.Length - 3);
                    }
                    jsonContent = jsonContent.Trim();

                    QuizData quizData = JsonUtility.FromJson<QuizData>(jsonContent);
                    List<QuestionSO> generatedQuestions = CreateQuestionSOs(quizData.questions);

                    quizGenerateHandler?.Invoke(generatedQuestions);
                }
                catch (Exception e)
                {
                    Debug.LogError($"���� �Ľ� ����: {e.Message}");
                    Debug.LogError($"���� ����: {webRequest.downloadHandler.text}");
                }
            }
            else
            {
                Debug.LogError($"ChatGPT API ��û ����: {webRequest.error}");
                Debug.LogError($"���� �ڵ�: {webRequest.responseCode}");
                Debug.LogError($"���� ����: {webRequest.downloadHandler.text}");
            }
        }
    }

    private List<QuestionSO> CreateQuestionSOs(QuizQuestion[] quizQuestions)
    {
        List<QuestionSO> questionSOs = new List<QuestionSO>();

        foreach (QuizQuestion quizQ in quizQuestions)
        {
            QuestionSO questionSO = ScriptableObject.CreateInstance<QuestionSO>();

            // Reflection�� ����Ͽ� private �ʵ忡 �� ����
            var questionField = typeof(QuestionSO).GetField("question", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var answersField = typeof(QuestionSO).GetField("answers", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var correctAnswerIndexField = typeof(QuestionSO).GetField("correctAnswerIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            questionField?.SetValue(questionSO, quizQ.question);
            answersField?.SetValue(questionSO, quizQ.answers);
            correctAnswerIndexField?.SetValue(questionSO, quizQ.correctAnswerIndex);

            questionSOs.Add(questionSO);
        }

        return questionSOs;
    }

    public void SetApiKey(string key)
    {
        apiKey = key;
        PlayerPrefs.SetString("OpenAI_API_Key", key);
        PlayerPrefs.Save();
    }
}