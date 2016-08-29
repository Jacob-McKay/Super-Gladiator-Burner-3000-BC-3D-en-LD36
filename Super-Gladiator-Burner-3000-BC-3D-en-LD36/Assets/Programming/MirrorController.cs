using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

using System.Collections;

public class MirrorController : MonoBehaviour {

    public float rotationMultiplier = 1;
    public float deathRayRange = 9001;

	// Use this for initialization
	void Start () {
	    if(deathRayRange > 9000)
        {
            //Debug.LogWarning("Death Ray range is over NINE-THOUSAAAAAAND!!!!1");
        }
	}
	
	// Update is called once per frame
	void Update () {
        RotateMirrorBasedOnMouseMovement();
        ShootDeathRayOfHeavenlyLight();
    }

    private void RotateMirrorBasedOnMouseMovement()
    {
        var mouseXYDelta = GetMouseDeltas();
        var rotationXYDelta = new Vector2(Mathf.Clamp((mouseXYDelta.x * rotationMultiplier), -1, 1), Mathf.Clamp((mouseXYDelta.y * rotationMultiplier * -1), -1, 1));
        transform.Rotate(rotationXYDelta.y, rotationXYDelta.x, 0);
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
        if (Physics.Raycast(transform.position, transform.forward, out hit, deathRayRange))
        {

            GameObject objectHit = hit.transform.gameObject;
            if (objectHit.tag == "Burnable")
            {
                Debug.Log("BURNABLE HIT!");
                var burnable = objectHit.GetComponent<IBurnable>();
                burnable.Burn();
            }
        }
    }

    private void PaintDeathRayForVisualEffect()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * deathRayRange;
        Debug.DrawRay(transform.position, forward, Color.yellow);
    }
}
