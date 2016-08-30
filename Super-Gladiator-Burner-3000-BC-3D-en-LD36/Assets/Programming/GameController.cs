using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

	// Use this for initialization
	void Start () {
        _ui = FindObjectOfType<UIController>();
        _ui.UpdateLevel(level);
        _ui.UpdateScore(_score);

        _sun = FindObjectOfType<SunController>();

        gladiatorSpawns = gladiatorSpawnsParent.GetComponentsInChildren<Transform>();

        StartCoroutine(BeginTimedReleaseWaves());
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
