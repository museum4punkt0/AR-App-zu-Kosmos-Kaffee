using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using LostInTheGarden.KaffeeKosmos;

// using LostInTheGarden.KaffeeKosmos;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ARImageTrackerController : MonoBehaviour
{

    public GameObject tree;

    ARTrackedImageManager m_TrackedImageManager;

    public GameObject ScanMarker;



    public GameObject Rain; 
    public GameObject Clouds; 

    public GameObject debugSphere; 
    public GameObject Ground; 
    public GameObject ParticleForceField; 
    public GameObject tutorialStart;

    public GameObject timeline; 
    public GameObject growButton;


    void Awake() {
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();

        Rain.SetActive(false);
        Clouds.SetActive(false);
        Ground.SetActive(false);
    }

    void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        Debug.Log("Image tracking is enabled now!");
    }

    void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        Debug.Log("Image tracking is disabled now!");

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            if(CoffeePlant.Instance != null) {
                Rain.transform.position = new Vector3(CoffeePlant.Instance.transform.position.x, CoffeePlant.Instance.transform.position.y + CoffeePlant.Instance.Trunks[0].Growth + 0.7f, CoffeePlant.Instance.transform.position.z);
                if(Clouds.activeSelf) Clouds.transform.position = new Vector3(CoffeePlant.Instance.transform.position.x, CoffeePlant.Instance.transform.position.y + CoffeePlant.Instance.Trunks[0].Growth + 0.7f, CoffeePlant.Instance.transform.position.z);
            }
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.updated) {

            Debug.Log("Imagestate changed!");

            if(!tree.activeSelf) tree.SetActive(true);
            if(!Rain.activeSelf) Rain.SetActive(true);
            // if(!Clouds.activeSelf) Clouds.SetActive(true);

            Debug.Log("objects set active");

            gameObject.GetComponent<PlaceOnPlane>().movingEnabled = false;
            tree.transform.position = trackedImage.transform.position;
            tree.transform.rotation = trackedImage.transform.rotation;


            Debug.Log("Tree placed ");

            // var rainPosition =  new Vector3(trackedImage.transform.position.x, trackedImage.transform.position.y + 1.6f, trackedImage.transform.position.z);
            // var cloudPosition =  new Vector3(trackedImage.transform.position.x, trackedImage.transform.position.y + 1.6f, trackedImage.transform.position.z);


            Rain.transform.position = new Vector3(trackedImage.transform.position.x, trackedImage.transform.position.y, trackedImage.transform.position.z);
            if(Clouds.activeSelf) Clouds.transform.position = new Vector3(trackedImage.transform.position.x, trackedImage.transform.position.y, trackedImage.transform.position.z);
            // debugSphere.transform.position = new Vector3(trackedImage.transform.position.x, trackedImage.transform.position.y + 0.1f, trackedImage.transform.position.z);

            Debug.Log("Weather placed");


            // get Ground position
            if(!Ground.activeSelf) Ground.SetActive(true);

            Debug.Log("Ground set active");


            Debug.Log("tutorial start shown: " + tutorialStart.GetComponent<TutorialStartController>().wasShown);

            if(!tutorialStart.GetComponent<TutorialStartController>().wasShown) {
                if(!tutorialStart.activeSelf) tutorialStart.SetActive(true);

            }


            Ground.transform.position = trackedImage.transform.position; // set set ground position

            Debug.Log("Ground positioned");            

            ParticleForceField.transform.position = trackedImage.transform.position;
            
            Debug.Log("force placed");

            // enable growbutton and timeline
            timeline.GetComponent<CanvasGroup>().alpha = 1;
            timeline.SetActive(true);


            growButton.GetComponent<CanvasGroup>().alpha = 1;
            growButton.SetActive(true);


            // if image was found hide scan marker card
            ScanMarker.GetComponent<Animator>().SetBool("show", false);

        }

    }
}
