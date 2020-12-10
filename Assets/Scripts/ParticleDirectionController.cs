using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDirectionController : MonoBehaviour
{

    
    ParticleSystem mParticleSystem;
    
    ParticleSystem.ForceOverLifetimeModule mForceOverLifetime; 


    public GameObject target; 


    // Start is called before the first frame update
    void Start()
    {
        mParticleSystem = gameObject.GetComponent<ParticleSystem>();
        mForceOverLifetime = mParticleSystem.forceOverLifetime;
        mForceOverLifetime.enabled = true; 

    }

    // Update is called once per frame
    void Update()
    {
        mForceOverLifetime.x = target.transform.position.x;
        mForceOverLifetime.y = target.transform.position.y;
        mForceOverLifetime.z = target.transform.position.z;
    }
}
    