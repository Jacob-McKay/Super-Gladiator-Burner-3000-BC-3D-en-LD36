using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    private int _score = 0;
    private int _level = 1;

    private UIController _ui;
	// Use this for initialization
	void Start () {
        _score = 0;
        _level = 1;
        _ui = FindObjectOfType<UIController>();
        _ui.UpdateLevel(_level);
        _ui.UpdateScore(_score);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlusOneGladiatorDown()
    {
        _score++;
        _ui.UpdateScore(_score);
    }
}
