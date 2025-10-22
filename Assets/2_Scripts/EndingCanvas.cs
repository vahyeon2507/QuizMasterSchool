using TMPro;
using UnityEngine;

public class EndingCanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI endingText;
    [SerializeField] ScoreKeeper scoreKeeper;
    
    public void ShowEnding()
    {
     endingText.text = $"축하합니다!\n\r당신의 점수는 {scoreKeeper.CorrectRate()}%입니다.";

    }



}
