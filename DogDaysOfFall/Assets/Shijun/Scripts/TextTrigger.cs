using UnityEngine;
using UnityEngine.UI;

public class TextTrigger : MonoBehaviour
{
    public Text successesText;
    private string successesString;

    public Text attemptsText;
    private string attemptsString;
    
    public int attemptsCounter;
    public int toSucceedTimes;
    public int succeededCounter;

    private void Update()
    {
        SetSuccessesText();
        SetAttemptsText();
    }

    private void SetSuccessesText()
    {
        successesString = "Successes Needed: " + succeededCounter.ToString() + "/" + toSucceedTimes.ToString();
        successesText.text = successesString;
    }

    private void SetAttemptsText()
    {
        attemptsString = "Rounds: " + attemptsCounter.ToString();
        attemptsText.text = attemptsString;
    }
}
