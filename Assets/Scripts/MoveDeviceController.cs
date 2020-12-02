using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDeviceController : MonoBehaviour
{

    public GameObject scan_marker_button;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable() {
        Debug.Log("ENABLED!!!");
        if(!scan_marker_button.activeSelf) scan_marker_button.SetActive(true);
    }

    void OnDisable() {
        Debug.Log("DISABLED!!!");
        if(scan_marker_button.activeSelf) scan_marker_button.SetActive(false);
    }
}
