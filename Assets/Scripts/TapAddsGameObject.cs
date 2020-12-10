using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TapAddsGameObject : MonoBehaviour
{

    public ARSessionOrigin aRSessionOrigin;
    public GameObject virtualObjectPrefab;

    // Start is called before the first frame update
    void Start()
    {

        if (virtualObjectPrefab == null || aRSessionOrigin == null)
        {
            Debug.LogError("GameObject or arSessionOriginObject is missing!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if tap is happening | Input.touchCount = number of touches in one frame 
        if (Input.touchCount > 0) 
        {
                // instantiate a copy of the prefab
                GameObject virtualObject = Instantiate<GameObject>(virtualObjectPrefab, gameObject.transform);
                virtualObject.transform.rotation.Set(90f, 180f, 0f, 0f);

                // get the AR Camera and set the object to a distance of 40 cm in front of the camera
                Camera trackedCamera = aRSessionOrigin.camera;
                virtualObject.transform.position = trackedCamera.transform.position + trackedCamera.transform.forward * 0.4f;

                // set rotation of virtual object to be the same as AR Camera
                virtualObject.transform.rotation = trackedCamera.transform.rotation;
            //}
        }
    }
}

