using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The ARCameraManager which will produce frame events.")]
    ARCameraManager m_CameraManager;

    /// <summary>
    /// Get or set the <c>ARCameraManager</c>.
    /// </summary>

    public GameObject Timeline; 

    private bool isTimelineEnabled = false; // Enable the timeline only once 
    public GameObject GrowButton;
    public GameObject tutorialStart;

    public GameObject scan_marker_btn;

    public GameObject tapToPlace;

    public GameObject moveDeviceText; 
    public GameObject tapToPlaceText; 

    public ARCameraManager cameraManager
    {
        get { return m_CameraManager; }
        set
        {
            if (m_CameraManager == value)
                return;

            if (m_CameraManager != null)
                m_CameraManager.frameReceived -= FrameChanged;

            m_CameraManager = value;

            if (m_CameraManager != null & enabled)
                m_CameraManager.frameReceived += FrameChanged;
        }
    }

    const string k_FadeOffAnim = "FadeOff";
    const string k_FadeOnAnim = "FadeOn";

    [SerializeField]
    ARPlaneManager m_PlaneManager;

    public ARPlaneManager planeManager
    {
        get { return m_PlaneManager; }
        set { m_PlaneManager = value; }
    }

    [SerializeField]
    Animator m_MoveDeviceAnimation;

    public Animator moveDeviceAnimation
    {
        get { return m_MoveDeviceAnimation; }
        set { m_MoveDeviceAnimation = value; }
    }

    [SerializeField]
    Animator m_TapToPlaceAnimation;

    public Animator tapToPlaceAnimation
    {
        get { return m_TapToPlaceAnimation; }
        set { m_TapToPlaceAnimation = value; }
    }

    static List<ARPlane> s_Planes = new List<ARPlane>();

    bool m_ShowingTapToPlace = false;

    bool m_ShowingMoveDevice = true;

    void OnEnable()
    {
        if (m_CameraManager != null)
            m_CameraManager.frameReceived += FrameChanged;

        PlaceOnPlane.onPlacedObject += PlacedObject;

        // m_ShowingMoveDevice = true;
    }

    void OnDisable()
    {
        if (m_CameraManager != null)
            m_CameraManager.frameReceived -= FrameChanged;

        PlaceOnPlane.onPlacedObject -= PlacedObject;
    }

    void FrameChanged(ARCameraFrameEventArgs args)
    {
        if (PlanesFound() && m_ShowingMoveDevice)
        {
            if (moveDeviceAnimation)
                moveDeviceAnimation.SetTrigger(k_FadeOffAnim);
                moveDeviceText.SetActive(false);


            if (tapToPlaceAnimation)
                tapToPlaceAnimation.SetTrigger(k_FadeOnAnim);
                tapToPlaceText.SetActive(true);

            m_ShowingTapToPlace = true;
            m_ShowingMoveDevice = false;
        }
    }

    bool PlanesFound()
    {
        if (planeManager == null)
            return false;

        return planeManager.trackables.count > 0;
    }

    void PlacedObject()
    {
        if (m_ShowingTapToPlace)
        {
            if (tapToPlaceAnimation)
                tapToPlaceAnimation.SetTrigger(k_FadeOffAnim);


            // deactivate tap to place screen
            tapToPlace.SetActive(false);
            
            if(scan_marker_btn.activeSelf) scan_marker_btn.SetActive(false);

            m_ShowingTapToPlace = false;

        }
        
        // Set Timeline and Growbutton active
        if(!Timeline.activeSelf && !isTimelineEnabled) {
            // Timeline.GetComponent<CanvasGroup>().alpha = 1;
            Timeline.SetActive(true);
            isTimelineEnabled = true;
        }
        if(!GrowButton.activeSelf) {
            GrowButton.SetActive(true);
            GrowButton.GetComponent<CanvasGroup>().alpha = 1;
        }
        if(!tutorialStart.GetComponent<TutorialStartController>().wasShown) {
        if(!tutorialStart.activeSelf) tutorialStart.SetActive(true);

        }
        

    }
}