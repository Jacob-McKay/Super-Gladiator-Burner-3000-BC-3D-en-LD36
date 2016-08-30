using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

using System.Collections;

public class FancyMirrorController : MonoBehaviour
{
    public float rotationMultiplier = 1;
    public float deathRayRange = 9001;

    public GameObject smoke;
    GameObject smokeClone;
    public float smokeTimer;

    public Transform mirror;
    public Transform mirrorFrame;

    private Transform _sun;

    // Use this for initialization
    void Start()
    {
        if (deathRayRange > 9000)
        {
            Debug.LogWarning("Death Ray range is over NINE-THOUSAAAAAAND!!!!1");
        }

        _sun = FindObjectOfType<SunController>().gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        RotateMirrorBasedOnMouseMovement();
        ShootDeathRayOfHeavenlyLight();
        smokeTimer += Time.deltaTime;
    }

    private void RotateMirrorBasedOnMouseMovement()
    {
        var mouseXYDelta = GetMouseDeltas();
        var rotationX = Mathf.Clamp((mouseXYDelta.x * rotationMultiplier), -1, 1);
        var rotationY = Mathf.Clamp((mouseXYDelta.y * rotationMultiplier * -1), -1, 1);
        mirror.transform.Rotate(rotationY, 0, 0);
        mirrorFrame.transform.Rotate(0, rotationX, 0);
    }

    private Vector2 GetMouseDeltas()
    {
        float xDelta = CrossPlatformInputManager.GetAxis("Mouse X");
        float yDelta = CrossPlatformInputManager.GetAxis("Mouse Y");

        //Debug.Log("Movement detected: " + new Vector2(xDelta, yDelta));

        return new Vector2(xDelta, yDelta);
    }

    private void ShootDeathRayOfHeavenlyLight()
    {
        var vectorFromSunToMirror = mirror.transform.position - _sun.position;
        var mirrorNormal = mirror.transform.forward;
        var reflectedVectorOffOfMirror = Vector3.Reflect(vectorFromSunToMirror, mirrorNormal);

        PaintDeathRayForVisualEffect();
        RaycastHit hit;
        if (Physics.Raycast(mirror.transform.position, reflectedVectorOffOfMirror, out hit, deathRayRange))
        {
            GameObject objectHit = hit.transform.gameObject;
            //Debug.Log("I hit something! " + objectHit.name);

            if (objectHit.tag == "Burnable")
            {
                //Debug.Log("BURNABLE HIT!");
                var burnable = objectHit.GetComponent<IBurnable>();
                burnable.Burn();
            }
            if (hit.collider.tag == "Floor")
            {
                Debug.Log("Floor!");
                smokeClone = Instantiate(smoke, hit.point, Quaternion.identity) as GameObject;
                Destroy(smokeClone, 3f);
                smokeTimer = 0f;
            }
        }
    }

    private void PaintDeathRayForVisualEffect()
    {
        var vectorFromSunToMirror = mirror.transform.position - _sun.position;
        var mirrorNormal = mirror.transform.forward;
        var reflectedVectorOffOfMirror = Vector3.Reflect(vectorFromSunToMirror, mirrorNormal);
        Debug.DrawRay(_sun.position, mirror.transform.position - _sun.position, Color.black);
        Debug.DrawRay(mirror.transform.position, reflectedVectorOffOfMirror, Color.red, 0.2f);
    }
}