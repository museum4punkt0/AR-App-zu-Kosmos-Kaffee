using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSound : MonoBehaviour
{

    public AudioClip sound; 


    private AudioSource source {get {return GetComponent<AudioSource>(); } }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<AudioSource>();        
        source.clip = sound;
        source.playOnAwake = false;
        source.loop = true;
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlaySound() {
        
        source.Play();
    }

    public void StopSound() {
        source.Stop();
    }
}
