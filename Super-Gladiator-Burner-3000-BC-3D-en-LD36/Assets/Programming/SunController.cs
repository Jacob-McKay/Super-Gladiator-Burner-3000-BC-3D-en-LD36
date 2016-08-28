using UnityEngine;
using System.Collections;

public class SunController : MonoBehaviour {

    //   public float sunRotateSpeed = 1;
    //   public Transform targetToAimAt;

    //// Use this for initialization
    //void Start () {
    //}

    //// Update is called once per frame
    //void Update () {
    //       transform.RotateAround(targetToAimAt.position, transform.up, Time.deltaTime * sunRotateSpeed);
    //}
    public Transform directionalLightTarget;
    public Transform sunrise;
    public Transform sunset;
    public Transform lensFlareParent;
    public float journeyTime = 1.0F;
    private float startTime;
    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        MoveSunAcrossTheSky();
        AimSunlightAtTarget();
        AimLenseFlareAtCamera();
    }

    private void AimSunlightAtTarget()
    {
        transform.LookAt(directionalLightTarget);
    }

    private void MoveSunAcrossTheSky()
    {
        Vector3 center = (sunrise.position + sunset.position) * 0.5F;
        center -= new Vector3(0, 50, 0);
        Vector3 riseRelCenter = sunrise.position - center;
        Vector3 setRelCenter = sunset.position - center;
        float fracComplete = (Time.time - startTime) / journeyTime;
        transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
        transform.position += center;
    }

    private void AimLenseFlareAtCamera()
    {
        lensFlareParent.LookAt(Camera.main.transform);
    }
}
