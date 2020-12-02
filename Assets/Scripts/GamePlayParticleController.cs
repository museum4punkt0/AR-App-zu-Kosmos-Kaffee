using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayParticleController : MonoBehaviour
{

    public GameObject growButton;    
    private Vector3 worldPosition;
    private Camera cam;

    public bool disableOnStart;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        if(disableOnStart) gameObject.GetComponent<ParticleSystem>().Stop();
        
    }

    // Update is called once per frame
    void Update()
    {
        worldPosition = cam.ScreenToWorldPoint(new Vector3(growButton.transform.position.x, growButton.transform.position.y, cam.nearClipPlane));
        gameObject.transform.position = worldPosition;
    }
}
