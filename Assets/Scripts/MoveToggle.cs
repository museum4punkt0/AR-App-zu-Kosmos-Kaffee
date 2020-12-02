using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveToggle : MonoBehaviour
{

    public Button sun;
    public Button sunCloud;
    public Button cloud;
    public Image knob; // knob that moves
    public Image temperatureFill;

    public static Button currentButton;
    public static Button lastButton;

    // starting value for the Lerp
    static float t = 0.0f;
    static Vector3 destinationPosition;

    private float oldFillAmount;
    private float newFillAmount;


    // Start is called before the first frame update
    void Start()
    {
        currentButton = sun;
        destinationPosition = sun.transform.position;
        oldFillAmount = temperatureFill.fillAmount;
        MoveToSun();
    }

    public void MoveToSun()
    {
        oldFillAmount = temperatureFill.fillAmount;
        newFillAmount = 1f;
        lastButton = currentButton;

        currentButton = sun;
        destinationPosition = sun.transform.position;
    }
    public void MoveToSunCloud()
    {
        oldFillAmount = temperatureFill.fillAmount;
        newFillAmount = 0.5f;
        

        currentButton = sunCloud;
        destinationPosition = sunCloud.transform.position;
    }
    public void MoveToCloud()
    {
        oldFillAmount = temperatureFill.fillAmount;
        newFillAmount = 0f;
        //Debug.Log("Starting Animation");
        //knob.transform.position = Vector3.Lerp(knob.transform.position, cloud.transform.position, 2f);
        currentButton = cloud;
        destinationPosition = cloud.transform.position;
    }


    private void Update()
    {
        knob.transform.position = Vector3.Lerp(knob.transform.position, destinationPosition, Time.deltaTime / 0.1f);
        temperatureFill.fillAmount = Mathf.Lerp(temperatureFill.fillAmount, newFillAmount, Time.deltaTime / 7f);
    }

}   