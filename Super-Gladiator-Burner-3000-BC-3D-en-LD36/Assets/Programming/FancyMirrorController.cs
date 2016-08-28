using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

using System.Collections;

public class FancyMirrorController : MonoBehaviour
{
    public float rotationMultiplier = 1;
    public float deathRayRange = 9001;

    public Transform mirror;
    public Transform mirrorFrame;

    // Use this for initialization
    void Start()
    {
        if (deathRayRange > 9000)
        {
            //Debug.LogWarning("Death Ray range is over NINE-THOUSAAAAAAND!!!!1");
        }
    }

    // Update is called once per frame
    void Update()
    {
        RotateMirrorBasedOnMouseMovement();
        ShootDeathRayOfHeavenlyLight();
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

        Debug.Log("Movement detected: " + new Vector2(xDelta, yDelta));

        return new Vector2(xDelta, yDelta);
    }

    private void ShootDeathRayOfHeavenlyLight()
    {
        PaintDeathRayForVisualEffect();
        RaycastHit hit;
        if (Physics.Raycast(mirror.transform.position, mirror.transform.forward, out hit, deathRayRange))
        {

            GameObject objectHit = hit.transform.gameObject;
            if (objectHit.tag == "Burnable")
            {
                Debug.Log("BURNABLE HIT!");
                var burnable = objectHit.GetComponent<Burnable>();
                burnable.Burn();
            }
        }
    }

    private void PaintDeathRayForVisualEffect()
    {
        Vector3 forward = mirror.transform.TransformDirection(Vector3.forward) * deathRayRange;
        Debug.DrawRay(mirror.transform.position, forward, Color.yellow);
    }
}
