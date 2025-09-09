using UnityEngine;
using System.Collections.Generic;

public class Test : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Hello World");

        Publisher publisher = new Publisher();
        publisher.OnMessage += ResultProcess;  

    publisher.SendMessage("�߰������ּ���.");
        Debug.Log("�۾��Ϸ�Ǿ����ϴ�.");
    }

    void ResultProcess (string message)
    {
        Debug.Log($"�޽����� �޾ҽ��ϴ�: {message}");
    }



    void OtherProcess(string message)
    {
        Debug.Log($"�ٸ� �۾��� �����մϴ�: {message}");
    }
    public class Publisher
    {
        public delegate void onMessage(string message);
        public event onMessage OnMessage;

        public void SendMessage(string text)
        {
                Debug.Log($"ChatGPT API�� ����մϴ�.(1�� �̻� �ҿ�˴ϴ�.)+ {text}");
            //return null;

            OnMessage?.Invoke(text);
        }

    }
}
