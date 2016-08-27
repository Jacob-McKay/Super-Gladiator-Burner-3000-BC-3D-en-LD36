using UnityEngine;
using System.Collections;

public class Burnable : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void Burn()
    {
        Debug.Log("WE BE BURNIN! EEEEEEEEEEEEK!");
        Destroy(transform.gameObject);
    }
}
