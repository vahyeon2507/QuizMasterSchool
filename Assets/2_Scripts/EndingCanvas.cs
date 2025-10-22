using TMPro;
using UnityEngine;

public class EndingCanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI endingText;
    [SerializeField] ScoreKeeper scoreKeeper;
    
    public void ShowEnding()
    {
     endingText.text = $"�����մϴ�!\n\r����� ������ {scoreKeeper.CorrectRate()}%�Դϴ�.";

    }



}
