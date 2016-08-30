using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {

    public Text levelText;
    public Text scoreText;
    public Text gameOverText;
    public Image screenFader;

    public float secondsTilFullFade = 0.5f;

    private string[] _romanNumerals = { "0", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GameOver(string gameOverMessage)
    {
        gameOverText.text = gameOverMessage;
        StartCoroutine(CoGameover(Time.time, Time.time + secondsTilFullFade));
    }

    private IEnumerator CoGameover(float fadeStartTime, float fadeEndTime)
    {
        while (screenFader.color.a != 1)
        {
            float fractionFaded = (Time.time - fadeStartTime) / (secondsTilFullFade);
            var newAlpha = Mathf.Lerp(screenFader.color.a, 1, Mathf.Clamp01(fractionFaded));
            var newColor = new Color(screenFader.color.r, screenFader.color.g, screenFader.color.b, newAlpha);
            screenFader.color = newColor;
            yield return null;
        }
        gameOverText.enabled = true;
    }
    public void UpdateLevel(int level)
    {
        levelText.text = "Day " + _romanNumerals[level];
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
