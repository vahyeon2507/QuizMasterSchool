using UnityEngine;
using System.Collections.Generic;

public class Test : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Hello World");

        Publisher publisher = new Publisher();
        publisher.OnMessage += ResultProcess;  

    publisher.SendMessage("추가문제주세요.");
        Debug.Log("작업완료되었습니다.");
    }

    void ResultProcess (string message)
    {
        Debug.Log($"메시지를 받았습니다: {message}");
    }



    void OtherProcess(string message)
    {
        Debug.Log($"다른 작업을 수행합니다: {message}");
    }
    public class Publisher
    {
        public delegate void onMessage(string message);
        public event onMessage OnMessage;

        public void SendMessage(string text)
        {
                Debug.Log($"ChatGPT API와 통신합니다.(1분 이상 소요됩니다.)+ {text}");
            //return null;

            OnMessage?.Invoke(text);
        }

    }
}
