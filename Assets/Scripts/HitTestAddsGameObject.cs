using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

using LostInTheGarden.KaffeeKosmos;


[RequireComponent(typeof(ARRaycastManager))]


public class HitTestAddsGameObject : MonoBehaviour
{



    public ARSessionOrigin aRSessionOrigin;
    public GameObject aRSessionOrigin_gameobject;

    ARRaycastManager m_RaycastManager;

    public GameObject virtualGameObject;
    public GameObject rain;
    public GameObject cloud;
    public GameObject ground; 
    public GameObject particleForceField;
    public GameObject Timeline; 

    private bool isTimelineEnabled = false;
    public GameObject GrowButton; 

    public bool movingEnabled = true;

    public static event Action onPlacedObject;


    public GameObject tutorialStart;

    public GameObject stickyItem;
    private bool objectInstantiated = false;
    GameObject virtualObject;

    //Rect topLeft = new Rect(0, 0, Screen.width / 2, Screen.height / 2);
    Rect touchRect = new Rect(0, Screen.height, Screen.width, Screen.height / 1.5f);

    public float uniformScale;

    // Start is called before the first frame update
    void Start()
    {


        if (virtualGameObject == null || aRSessionOrigin == null)
        {
           Debug.LogError("GameObject or arSessionOriginObject is missing!");
        }
        else
        {
            // 
            virtualObject = virtualGameObject;
            virtualObject.transform.localScale = new Vector3(uniformScale, uniformScale, uniformScale);
            stickyItem.transform.position = new Vector3(0f, CoffeePlant.Instance.Trunks[0].Growth * uniformScale, 0f);



            // Debug.Log("ARKit or ARCore Status:" + ARSubsystemManager.ARSystemState);
            Debug.Log("ARKit or ARCore Status:" + ARSession.state);
            if (ARSession.state <= ARSessionState.Unsupported)
            {
                // when playing in Unity Editor position the tree(virtual object) in front of the Camera
                virtualObject.transform.position = new Vector3(0f, -.5f, 1.5f);
                virtualObject.transform.localScale = new Vector3(uniformScale, uniformScale, uniformScale);
                stickyItem.transform.position = virtualObject.transform.position;

                // move the ground as well
                ground.transform.position = new Vector3(0f-0.15f, -.5f, 1.5f-0.15f);
                particleForceField.transform.position = new Vector3(0f, -.5f, 1.5f);

            }
        }


        m_RaycastManager = GetComponent<ARRaycastManager>();



    }

    // Update is called once per frame
    void Update()
    {

        // Debug.Log(ground.GetComponent<Terrain>().terrainData.size);

        stickyItem.transform.position = new Vector3(virtualObject.transform.position.x -.1f, virtualObject.transform.position.y + CoffeePlant.Instance.Trunks[0].Growth + .1f, virtualObject.transform.position.z);
        rain.transform.position = new Vector3(virtualObject.transform.position.x, CoffeePlant.Instance.Trunks[0].Growth + 0.01f, virtualObject.transform.position.z);
        cloud.transform.position = new Vector3(virtualObject.transform.position.x, CoffeePlant.Instance.Trunks[0].Growth + 0.01f, virtualObject.transform.position.z);


        if(movingEnabled) {
            if (Input.touchCount > 0)
                {
                    
                    if(!tutorialStart.GetComponent<TutorialController>().wasShown) {
                        tutorialStart.SetActive(true);
                    } 

                    // get touch input
                    Touch mTouch = Input.GetTouch(0);

                    if (mTouch.position.y > (Screen.height * 0.3))
                    {
                        aRSessionOrigin_gameobject.GetComponent<PlaneDetectionController>().TogglePlaneDetection();

                        // array of trackabletypes in where to check for collisions
                        TrackableType[] trackableTypes =
                        {
                            TrackableType.PlaneWithinPolygon,
                            TrackableType.PlaneWithinBounds,
                            //TrackableType.PlaneEstimated,
                            //TrackableType.FeaturePoint
                        };

                        // loop through trackabletypes 
                        foreach (TrackableType trackableType in trackableTypes)
                        {
                            if (HitTestWithTrackableType(mTouch.position, trackableType))
                            {
                                // if it returns true leave the loop
                                return;
                            }
                        }
                    }
                    else {
                        aRSessionOrigin_gameobject.GetComponent<PlaneDetectionController>().TogglePlaneDetection();
                    }
                }
        }

        
    }

    void CreateVirtualObjectWithTransform(Pose hitPose)
    {

        onPlacedObject();

        Timeline.SetActive(true);
        GrowButton.SetActive(true);


        if(!objectInstantiated) objectInstantiated = true;

            // set position and rotation to hitPose
            virtualObject.transform.position = hitPose.position;
            virtualObject.transform.rotation = hitPose.rotation;

            
            // calculate new ground position to get the middle of the terrain
            var groundPosition = new Vector3(hitPose.position.x-ground.GetComponent<Terrain>().terrainData.size.x/2, hitPose.position.y, hitPose.position.z-ground.GetComponent<Terrain>().terrainData.size.z/2);
            ground.transform.position = groundPosition;
            ground.transform.rotation = hitPose.rotation;


            particleForceField.transform.position = hitPose.position;

            rain.transform.position = new Vector3(hitPose.position.x, CoffeePlant.Instance.Trunks[0].Growth + 0.2f, hitPose.position.z);
            cloud.transform.position = new Vector3(hitPose.position.x, CoffeePlant.Instance.Trunks[0].Growth + 0.2f, hitPose.position.z);
    }

    bool HitTestWithTrackableType(Vector3 screenPosition, TrackableType trackableType)
    {
        // create empty list of raycast hitresults
        List<ARRaycastHit> hitResults = new List<ARRaycastHit>();

        // use raycast API to detect collisions
        // screenPosition = position of the tap
        // hitresults = empty list
        // trackableTypeMask = the type of trackable to hit test against
        if(m_RaycastManager.Raycast(screenPosition, hitResults, trackableType))
        {
            // for each hitResult -> create object
            foreach(var hitResult in hitResults)
            {
                CreateVirtualObjectWithTransform(hitResult.sessionRelativePose);
                return true;
            }
        }
        // no collision detected
        return false;
    }
}
