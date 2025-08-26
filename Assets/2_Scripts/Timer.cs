using UnityEngine;
//using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] float problemTIme = 10f;
    [SerializeField] float solutionTime = 3f;
    float time = 0f;

    [HideInInspector] public bool isProblemTime = true;
    [HideInInspector] public float fillAmount = 1f;
    //[SerializeField] Image timerImage; // UI Image 연결

    private void Start()
    {
        time = problemTIme;
    }
    private void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0f)
        {
            if (isProblemTime)
            {
                isProblemTime = false;
                time = solutionTime;
            }
            else
            {
                time = problemTIme;
            }
        }
        if (isProblemTime)
        {
            fillAmount = time / problemTIme;
        }
        else
        {
            fillAmount = time / solutionTime;
        }

        // if (timerImage != null)
        //{
        //    timerImage.fillAmount = fillAmount; // UI에 값 반영
        //}
    }
}