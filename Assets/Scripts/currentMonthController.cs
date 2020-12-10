using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class currentMonthController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject elementToPlace; 
    private RectTransform elementToPlaceRectTransform;

    private Image timelineImage;
    private RectTransform imgRect;
    private RectTransform rectEdge;

    float fillAmount = 0;

    public bool isInhouse;

    void Start()    
    {
        timelineImage = GetComponent<Image>();
        imgRect = GetComponent<RectTransform>();
        elementToPlaceRectTransform = elementToPlace.GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void Update()
    {
        fillAmount = timelineImage.fillAmount;

        elementToPlaceRectTransform.anchorMin = new Vector2(fillAmount, elementToPlaceRectTransform.anchorMin.y);
        elementToPlaceRectTransform.anchorMax = new Vector2(fillAmount, elementToPlaceRectTransform.anchorMax.y);        

        /*  Probably not needed anymore
            if(fillAmount > .9f && isInhouse) elementToPlaceRectTransform.anchoredPosition = new Vector2(-200, -70); // CASE PUBLIC
            if(fillAmount > .8f && !isInhouse) elementToPlaceRectTransform.anchoredPosition = new Vector2(-280, -70); // CASE PUBLIC MOBILD
         */
        
            if(isInhouse) {
                elementToPlaceRectTransform.anchoredPosition = new Vector2(0, 0); // CASE INHOUSE 
                if(fillAmount > .9f) elementToPlaceRectTransform.anchoredPosition = new Vector2(-200, 0); // CASE PUBLIC    
            }
            // CASE PUBLIC APP 
            else {
                elementToPlaceRectTransform.anchoredPosition = new Vector2(0, 0); 
                if(fillAmount > .8f && !isInhouse) elementToPlaceRectTransform.anchoredPosition = new Vector2(-280, 0); 
            }        
    }
}
