using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using LostInTheGarden.KaffeeKosmos;

/// <summary>
/// Listens for touch events and performs an AR raycast from the screen touch point.
/// AR raycasts will only hit detected trackables like feature points and planes.
///
/// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
/// and moved to the hit position.
/// </summary>
[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }


    public GameObject arPlanePointer;

    public GameObject Rain; 
    public GameObject Clouds; 
    public GameObject Ground; 
    public GameObject ParticleForceField; 
    public bool movingEnabled; 

    public float UniformScale;

    public static event Action onPlacedObject;

    public float distance;
    private bool isPlaced = false;



    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        
        spawnedObject = placedPrefab;

        // scale spawned Object on start
        spawnedObject.transform.localScale = new Vector3(UniformScale, UniformScale, UniformScale);


        arPlanePointer.SetActive(false);
        spawnedObject.SetActive(false);
        if(Ground != null) Ground.SetActive(false);
        Rain.SetActive(false);
        Clouds.SetActive(false);

    }

    public void setPlaced(bool placed) {
        isPlaced = placed;
    } 


    void Update()
    {

        if(Application.isEditor) {
            Debug.Log("App is running in Editor");
            // place object ... 
            isPlaced = true;

            // set everything active
            if(!spawnedObject.activeSelf) spawnedObject.SetActive(true);
            // if(!Ground.activeSelf) Ground.SetActive(true);
            if(!Rain.activeSelf) Rain.SetActive(true);
            // if(!Clouds.activeSelf) Clouds.SetActive(true);

        }

        // case object is already placed
        if(isPlaced) { 

            onPlacedObject();

            // disable plane detection if enabled
            if(gameObject.GetComponent<PlaneDetectionController>().enabled) {
                gameObject.GetComponent<ARPlaneManager>().enabled = false;
                gameObject.GetComponent<PlaneDetectionController>().SetAllPlanesActive(false);
                gameObject.GetComponent<PlaneDetectionController>().enabled = false;
            } 
            
            // move rain and clouds to the position of the object
            if(CoffeePlant.Instance != null) {
                Rain.transform.position = new Vector3(spawnedObject.transform.position.x, spawnedObject.transform.position.y + CoffeePlant.Instance.Trunks[0].Growth + 0.7f, spawnedObject.transform.position.z);
                Clouds.transform.position = new Vector3(spawnedObject.transform.position.x, spawnedObject.transform.position.y + CoffeePlant.Instance.Trunks[0].Growth + 0.7f, spawnedObject.transform.position.z);
            }

        }
        else {
            if(spawnedObject != null) {
                // if(!Ground.activeSelf) Ground.SetActive(true);

            }


            // get "center" of screen
            var screenCenter = new Vector2(Screen.width/2, Screen.height/3);

            if (m_RaycastManager.Raycast(screenCenter, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                if(Input.touchCount == 1) {
                    
                    var touch = Input.GetTouch(0);

                    // ignore top and bottom screen corners
                    if(touch.phase == TouchPhase.Began && touch.position.y < (Screen.height-(Screen.height*0.2)))
                    {
                        isPlaced = true;
                    }

                }
                
                if(!spawnedObject.activeSelf) spawnedObject.SetActive(true);
                if(!arPlanePointer.activeSelf) arPlanePointer.SetActive(true);
                if(!Rain.activeSelf) Rain.SetActive(true);
                // if(!Clouds.activeSelf) Clouds.SetActive(true);


                // Raycast hits are sorted by distance, so the first one
                // will be the closest hit.
                var hitPose = s_Hits[0].pose;
                hitPose.position = hitPose.position  + Camera.main.transform.forward * distance;


                spawnedObject.transform.position = hitPose.position;
                arPlanePointer.transform.position = hitPose.position;
                spawnedObject.transform.rotation = hitPose.rotation;

                // get Ground position
                // var groundPosition = new Vector3(hitPose.position.x-Ground.GetComponent<Terrain>().terrainData.size.x/2, hitPose.position.y, hitPose.position.z-Ground.GetComponent<Terrain>().terrainData.size.z/2);
                // Ground.transform.position = groundPosition; // set set ground position

                ParticleForceField.transform.position = hitPose.position;

            }
        }

    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;
}