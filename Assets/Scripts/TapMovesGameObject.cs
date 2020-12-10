using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TapMovesGameObject : MonoBehaviour
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
        // if tap is happening
        if (Input.touchCount > 0)
        {
            //var topScreen = new Rect(0, Screen.height/2, Screen.width, Screen.height / 2);

            //if(topScreen.Contains(Input.GetTouch(0).position))
            //{
            // instantiate a copy of the prefab
            //GameObject virtualObject = Instantiate<GameObject>(virtualObjectPrefab, gameObject.transform);
            virtualObjectPrefab.transform.rotation.Set(90f, 180f, 0f, 0f);


                // get the AR Camera and set the object to a distance of 40 cm in front of the camera
                Camera trackedCamera = aRSessionOrigin.camera;
            virtualObjectPrefab.transform.position = trackedCamera.transform.position + trackedCamera.transform.forward * 0.4f;

            // set rotation of virtual object to be the same as AR Camera
            virtualObjectPrefab.transform.rotation = trackedCamera.transform.rotation;
            //}


        }
    }
}

