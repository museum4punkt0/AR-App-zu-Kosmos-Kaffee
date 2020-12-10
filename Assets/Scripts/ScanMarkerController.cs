using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ScanMarkerController : MonoBehaviour
{
    
    

    public ARTrackedImageManager trackedImageManager;
    public ARPlaneManager planeManager;

    public PlaneDetectionController planeDetectionController;

    public PlaceOnPlane placeOnPlane;

    public GameObject MoveDevice; 

    public GameObject timeline; 
    public GameObject growButton;
    public GameObject placeIndicator;

    public GameObject placeObjectButton; 
    public bool isInhouse;


    // Start is called before the first frame update
    void Start()
    {
    }

    void OnEnable() {
        
        // case its not an inhouse device -> show button to place object
        if(!isInhouse) if(!placeObjectButton.activeSelf) placeObjectButton.SetActive(true);

        // hide grow button
        if(growButton != null) {
            growButton.GetComponent<CanvasGroup>().alpha = 0;
            growButton.SetActive(false);
        } 
        // hide timeline
        if(timeline != null) {
            timeline.GetComponent<CanvasGroup>().alpha = 0; 
            timeline.SetActive(false);
        }

        // do enter animation
        gameObject.GetComponent<Animator>().SetBool("show", true); 

        // enable image tracking
        if(!trackedImageManager.enabled) {
            trackedImageManager.enabled = true; 
            Debug.Log("Image tracking is enabled: " + trackedImageManager.enabled);
        } 
        
        // disable plane detection
        if(planeManager.enabled) {
            planeManager.enabled = false; 
            planeDetectionController.SetAllPlanesActive(false);
        }
        
        // hide move device screen and disable placing on plane
        MoveDevice.SetActive(false);
        placeOnPlane.enabled = false;
        placeIndicator.SetActive(false);
    }

    void OnDisable() {
        if(placeObjectButton.activeSelf) placeObjectButton.SetActive(false);
    }

    void OnHideAnimationDone() {
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        gameObject.SetActive(false);

    } 

    public void hide() {
        gameObject.GetComponent<Animator>().SetBool("show", false);
    }

    public void onPlaceObjectButton () {
        gameObject.GetComponent<Animator>().SetBool("show", false);

        if(!planeManager.enabled) {
            planeManager.enabled = true;
            // planeDetectionController.SetAllPlanesActive(true);
        }
        if(trackedImageManager.enabled) trackedImageManager.enabled = false; 

    }
}
