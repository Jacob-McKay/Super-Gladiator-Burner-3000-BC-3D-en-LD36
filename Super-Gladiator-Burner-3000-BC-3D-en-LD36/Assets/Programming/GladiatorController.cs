using UnityEngine;
using System.Collections;

public class GladiatorController : MonoBehaviour {
    private Animator _animator;

	// Use this for initialization
	void Start () {
	    _animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.W)){
            _animator.SetTrigger("EnteredArena");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _animator.SetTrigger("SetOnFire");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _animator.SetTrigger("Die");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            _animator.SetTrigger("Extinguished");
        }
    }
}
