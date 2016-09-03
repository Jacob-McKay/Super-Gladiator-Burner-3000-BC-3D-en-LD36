using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using SocketIO;
using Assets.Programming;

public class GameController : MonoBehaviour {

    public GameObject gladiatorPrefab;
    public GameObject gladiatorSpawnsParent;
    public Transform[] gladiatorSpawns;
    public GameObject gladiatorTargetsParent;

    public float secondsBetweenWaves = 1;
    public int level = 1;

    public int[][] waves = {
        new int[] { 0 }, // we don't do level zero!
        new int[] { 2 },
        new int[] { 3 },
        new int[] { 4, 2 },
        new int[] { 5, 3 },
        new int[] { 5, 4, 2 },
        new int[] { 5, 5, 3 },
        new int[] { 5, 5, 4, 2 },
        new int[] { 5, 5, 5, 3 },
        new int[] { 5, 5, 5, 4 },
        new int[] { 5, 5, 5, 5 }
        };

    private int _score = 0;
    private int _currentWave = 0;
    private List<Transform> _temporarySpawnChoices;

    private UIController _ui;
    private SunController _sun;
    private SocketIOComponent _socketIO;
    private FancyMirrorController _mirror;

    private RedneckGyroData _lastGryoRead;

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        _ui = FindObjectOfType<UIController>();
        _ui.UpdateLevel(level);
        _ui.UpdateScore(_score);

        _sun = FindObjectOfType<SunController>();

        _mirror = FindObjectOfType<FancyMirrorController>();

        _socketIO = FindObjectOfType<SocketIOComponent>();
        _socketIO.On("chat message", OnMessageReceived);

        gladiatorSpawns = gladiatorSpawnsParent.GetComponentsInChildren<Transform>();

        StartCoroutine(BeginTimedReleaseWaves());
	}

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
    }

    public void SunHasSet()
    {
        // var numGladiatorsInLevel = waves[level].ToList().Aggregate((runningTotal, numInWave) => { return runningTotal + numInWave; });
        // Debug.Log("There are: " + numGladiatorsInLevel + " Gladiators in the level");
        var gladiatorsInScene = FindObjectsOfType<GladiatorController>();
        var numGladiatorsInScene = gladiatorsInScene.Length;
        Debug.Log("There are: " + numGladiatorsInScene + " Gladiators in the scene");

        var numDeadGladiatorsInScene = gladiatorsInScene.Where((gladiator) => gladiator.IsDead()).Count();
        Debug.Log("There are: " + numDeadGladiatorsInScene + " DEAD Gladiators in the scene");

        var numGladiatorsLeftLiving = numGladiatorsInScene - numDeadGladiatorsInScene;
        if(numGladiatorsLeftLiving > 0)
        {
            _ui.GameOver("You failed, you allowed: " + numGladiatorsLeftLiving + " gladiators to live \nTry harder next time!");
        } else
        {
            gladiatorsInScene.ToList().ForEach((gladiator) => Destroy(gladiator.gameObject));
            _sun.ResetSun();
            _currentWave = 0;
            level++;
            _ui.UpdateLevel(level);
            StartCoroutine(BeginTimedReleaseWaves());
        }
    }

    public void PlusOneGladiatorDown()
    {
        _score++;
        _ui.UpdateScore(_score);
    }

    void OnMessageReceived(SocketIOEvent obj)
    {

        var gyroRead = new RedneckGyroData
        {
            x = float.Parse(obj.data["x"].ToString()),
            y = float.Parse(obj.data["y"].ToString()),
            z = float.Parse(obj.data["z"].ToString()),
            timestamp = float.Parse(obj.data["timestamp"].ToString()),
        };
        _mirror.ApplyGryoRotation((gyroRead));

        if(_lastGryoRead.timestamp > gyroRead.timestamp)
        {
            Debug.LogError("HOLY BUTTS THE STUFF IS OUT OF ORDER, WTF ARE WE GONNA DO!???");
        } else
        {
            _lastGryoRead = gyroRead;
        }
    }

    /*

        	void onUserMove (SocketIOEvent obj)
	{
		GameObject player = GameObject.Find(  JsonToString( obj.data.GetField("name").ToString(), "\"") ) as GameObject;
		player.transform.position =  JsonToVecter3( JsonToString(obj.data.GetField("position").ToString(), "\"") );

	}

	string  JsonToString( string target, string s){

		string[] newString = Regex.Split(target,s);

		return newString[1];

	}

	Vector3 JsonToVecter3(string target ){

		Vector3 newVector;
		string[] newString = Regex.Split(target,",");
		newVector = new Vector3( float.Parse(newString[0]), float.Parse(newString[1]), float.Parse(newString[2]));

		return newVector;

	}
    */

    private IEnumerator BeginTimedReleaseWaves()
    {
        var numWaves = waves[level].Length;
        while(_currentWave != numWaves)
        {
            yield return new WaitForSeconds(secondsBetweenWaves);
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        _temporarySpawnChoices = new List<Transform>(gladiatorSpawns);
        var numGladiatorsInWave = 0;
        if (_currentWave < waves[level].Length)
        {
            numGladiatorsInWave = waves[level][_currentWave];
        }
        for (int i = 0; i < numGladiatorsInWave; i++)
        {
            var randomSpawn = _temporarySpawnChoices[Random.Range(0, _temporarySpawnChoices.Count)];
            _temporarySpawnChoices.Remove(randomSpawn);
            var gladiatorClone = Instantiate(gladiatorPrefab, randomSpawn.position, randomSpawn.rotation) as GameObject;
            var cloneGladiatorScript = gladiatorClone.GetComponent<GladiatorController>();
            cloneGladiatorScript.targetsParent = gladiatorTargetsParent;
        }
       
        _currentWave++;
    }
}
