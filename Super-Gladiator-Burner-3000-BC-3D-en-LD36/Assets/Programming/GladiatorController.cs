using UnityEngine;
using System.Collections;

public class GladiatorController : MonoBehaviour, IBurnable {
    public int health = 10;
    public float standingAroundSeconds = 1;
    public GameObject targetsParent;
    public Transform currentTarget;

    private GameController _gameController;

    private ParticleSystem _flameSource;
    private Animator _animator;
    private NavMeshAgent _agent;
    private Transform[] _possibleTargets;

    private float _timeInstantiated;
    private float _timeToStartRunningAround;
    private bool _runningAround;
    private int _timesBurned;
    private bool _alive = true;
    private bool _onFire = false;

	// Use this for initialization
	void Start () {
        _gameController = FindObjectOfType<GameController>();
        _flameSource = GetComponentInChildren<ParticleSystem>();
        _flameSource.Stop();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _timeInstantiated = Time.time;
        _timeToStartRunningAround = _timeInstantiated + standingAroundSeconds;

        _possibleTargets = targetsParent.GetComponentsInChildren<Transform>();
        currentTarget = _possibleTargets[Random.Range(0, _possibleTargets.Length)];
        //Debug.Log("possible targets: " + _possibleTargets + " current target: " + currentTarget);
    }
	
	// Update is called once per frame
	void Update () {

        if(_alive && !_runningAround && Time.time >= _timeToStartRunningAround)
        {
            _agent.destination = currentTarget.position;
            _animator.SetTrigger("EnteredArena");
            _runningAround = true;
        }

        if (_alive && _runningAround && _agent.remainingDistance < 0.5f)
        {
            SelectNextTargetAtRandom();
        }
    }

    private void SelectNextTargetAtRandom()
    {
        currentTarget = _possibleTargets[Random.Range(0, _possibleTargets.Length)];
        _agent.destination = currentTarget.position;
    }

    public void Burn()
    {
        if (_alive)
        {
            _onFire = true;
            _flameSource.Play();
            _animator.SetTrigger("SetOnFire");
            Debug.Log("EEEEEEEKK!!!1111  I'm Burnin'!!!!");
            _timesBurned++;

            if (_timesBurned > health)
            {
                _animator.SetTrigger("Die");
                _agent.Stop();
                _alive = false; // >:)
                _gameController.PlusOneGladiatorDown();
            }
        }
    }
}
