using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {

    public ParticleSystem mParticleSystem;
    public bool isActive; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown() {

        var emission = mParticleSystem.emission;

        if (!isActive)
        {
            emission.rateOverTime = 2f;
            isActive = true;
        }
        else if (isActive)
        {
            emission.rateOverTime = 1f;
            isActive = false;
        }
    }

    private void OnMouseEnter()
    {
        var main = mParticleSystem.main;
        main.startColor = Color.magenta;
        Debug.Log("Mouse Entered");
    }

    private void OnMouseExit()
    {
        var main = mParticleSystem.main;
        main.startColor = Color.white;
        Debug.Log("Mouse Left");
    }
}
