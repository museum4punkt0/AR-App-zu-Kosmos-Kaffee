using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticlePositionController : MonoBehaviour
{
    // Start is called before the first frame update

    public Button mButton;
    public Camera mCamera;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 worldPosition = mCamera.ScreenToWorldPoint(new Vector3(mButton.transform.position.x, mButton.transform.position.y, mCamera.nearClipPlane));
        gameObject.transform.position = worldPosition;
    }
}
