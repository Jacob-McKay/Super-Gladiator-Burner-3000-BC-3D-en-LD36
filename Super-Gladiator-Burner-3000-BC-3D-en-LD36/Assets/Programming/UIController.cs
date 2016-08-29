using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {

    public Text levelText;
    public Text scoreText;

    private string[] _romanNumerals = { "0", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
