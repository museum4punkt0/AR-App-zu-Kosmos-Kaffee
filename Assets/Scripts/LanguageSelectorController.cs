using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class LanguageSelectorController : MonoBehaviour
{
    public GameObject growButton = null;
    public PlaceOnPlane placeOnPlane = null; 
    public GameObject timeline = null;

    public bool visibleOnStart;

    public ARTrackedImageManager trackedImageManager;

    public ARPlaneManager planeManager;
    public GameObject MoveDevice; 
    public GameObject ScanMarker;

    public GameObject PlaceObjectButton;

    // Start is called before the first frame update
    void Start()
    {
    }

    void OnEnable() {
        if(!visibleOnStart) {
            gameObject.GetComponent<Animator>().SetBool("show", true); 
        }
        if(growButton != null) {
            growButton.GetComponent<CanvasGroup>().alpha = 0;
            growButton.SetActive(false);
        } 
        if(timeline != null) {
            timeline.GetComponent<CanvasGroup>().alpha = 0; 
            timeline.SetActive(false);
            // timeline.GetComponent<Animator>().SetBool("show", false);
        }

        // disable moving on plane
        if(placeOnPlane != null) placeOnPlane.movingEnabled = false;

        // enable image tracking
        // if(!trackedImageManager.enabled) trackedImageManager.enabled = true; 
        // disable plane detection
        // if(planeManager.enabled) planeManager.enabled = false; 
        // MoveDevice.SetActive(false);

    }

    void OnDisable() {
        // gameObject.GetComponent<Animator>().SetBool("show", false);
        // if(growButton != null) growButton.GetComponent<CanvasGroup>().alpha = 1;
    }

    void OnHideAnimationDone() {
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        gameObject.SetActive(false);
        if(placeOnPlane != null) placeOnPlane.movingEnabled = true;
            // if(timeline != null)timeline.SetActive(true);
    } 

    public void hide(bool isStartGame) {
        gameObject.GetComponent<Animator>().SetBool("show", false);
        if(isStartGame) {
            if(growButton != null) growButton.GetComponent<CanvasGroup>().alpha = 1;
            // tapAddsGameObject.SetActive(true);
            MoveDevice.SetActive(true);
        }
   

    }


    public void hideAndStartInhouse(bool isStartGame) {
        gameObject.GetComponent<Animator>().SetBool("show", false);
        if(isStartGame) {
            if(growButton != null) growButton.GetComponent<CanvasGroup>().alpha = 1;
            // tapAddsGameObject.SetActive(true);
            ScanMarker.SetActive(true);
        }

    }


}
