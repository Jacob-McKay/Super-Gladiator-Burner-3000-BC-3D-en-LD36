using UnityEngine;
using System.Collections;

public class LensFlare : MonoBehaviour {

    public Transform targetToAimAt;
    private Transform _targetToAimAt;

    // Use this for initialization
    void Start()
    {
        _targetToAimAt = targetToAimAt ?? Camera.main.transform;
    }

    // Update is called once per frame
    void Update () {
        transform.LookAt(_targetToAimAt);
	}
}
