using UnityEngine;
using UnityEngine.UI;

public class TextTrigger : MonoBehaviour
{
    public Text successesText;
    private string successesString;

    public Text attemptsText;
    private string attemptsString;
    
    public int attemptsCounter;
    public int succeedingCounter;
    public int succeededCounter;

    private void Update()
    {
        SetSuccessesText();
        SetAttemptsText();
    }

    private void SetSuccessesText()
    {
        successesString = "Successes Needed: " + succeededCounter.ToString() + "/" + succeedingCounter.ToString();
        successesText.text = successesString;
    }

    private void SetAttemptsText()
    {
        attemptsString = "Attempts: " + attemptsCounter.ToString();
        attemptsText.text = attemptsString;
    }
}
