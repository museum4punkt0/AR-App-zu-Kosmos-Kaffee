using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class RewardController : MonoBehaviour
{

    public GameObject growButton = null;
    public GameObject tapAddsGameObject = null; 
    public ARTrackedImageManager imageTracker = null;

    public GameObject timeline;
    
    public GameLogicController Rain; 
    public GameLogicController Sun;

    public float resultRain;
    public float resultSun;

    public TextMeshProUGUI resultGramm;
    public TextMeshProUGUI resultCoffeeCups;

    private HitTestAddsGameObject hitTestController;



    public GameObject star1; 
    public GameObject star2; 
    public GameObject star3; 

    public Color SuccessColor = new Color(215,175,98,255);


    // Start is called before the first frame update
    void Start()
    {
        if(tapAddsGameObject != null) hitTestController = tapAddsGameObject.GetComponent<HitTestAddsGameObject>();

        if(star1 && star2 && star3) {
            showReward();
            Invoke("doStarAnimation", 0.5f);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable() {

        if(imageTracker != null) if(imageTracker.enabled) imageTracker.enabled = false;

        if(Rain != null && Sun != null) {

            resultRain = Rain.GetResult();
            resultSun = Sun.GetResult();

            var result = (resultRain + resultSun) / 2;
            var result_gramm = Mathf.RoundToInt(Scale(0,1,0,500, result));
            var result_cups = Mathf.RoundToInt(Scale(0,1,0,500, result)/10);
            
            
            Debug.Log("RESULT (%): " + result); 
            Debug.Log("RESULT (gramm): " + result_gramm); 
            Debug.Log("RESULT (cups): " + result_cups); 
            

            resultGramm.text = result_gramm.ToString();
            resultCoffeeCups.text = result_cups.ToString();

        }


        if(timeline != null && timeline.activeSelf) timeline.GetComponent<Animator>().SetBool("show", false);
        gameObject.GetComponent<Animator>().SetBool("show", true);
        if(growButton != null) growButton.GetComponent<CanvasGroup>().alpha = 0;
        if(tapAddsGameObject != null) hitTestController.movingEnabled = false;
    }

    void OnDisable() {
        // gameObject.GetComponent<Animator>().SetBool("show", false);
        // if(growButton != null) growButton.GetComponent<CanvasGroup>().alpha = 1;
    }

    void OnHideAnimationDone() {
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        gameObject.SetActive(false);
        if(tapAddsGameObject != null) hitTestController.movingEnabled = true;
    } 
    public float Scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return NewValue;
    }


    void doStarAnimation() {
        if(star1 != null && star2 != null && star3 != null) {
            Debug.Log("All stars are here");

            var result = ( Rain.GetResult() + Sun.GetResult() ) / 2;

            // show low result --> one start
            if(result > 0.5f && result < 0.666) { 
                star1.GetComponent<Animator>().SetTrigger("pulse");
            }
            // show medium result -> two starts
            else if(result > 0.666f && result < 0.833) {
                star1.GetComponent<Animator>().SetTrigger("pulse");
                star2.GetComponent<Animator>().SetTrigger("pulse");

            }
            // show high result --> three stars
            else if(result > 0.833) {
                star1.GetComponent<Animator>().SetTrigger("pulse");
                star2.GetComponent<Animator>().SetTrigger("pulse");
                star3.GetComponent<Animator>().SetTrigger("pulse");
            }
        }
    }


     void showReward() {
        if(star1 != null && star2 != null && star3 != null) {
            Debug.Log("All stars are here");

            var result = ( Rain.GetResult() + Sun.GetResult() ) / 2;

            // show low result --> one start
            if(result > 0.5f && result < 0.666) { 
                star1.GetComponent<Image>().color = SuccessColor;
            }
            // show medium result -> two starts
            else if(result > 0.666f && result < 0.833) {
                star1.GetComponent<Image>().color = SuccessColor;
                star2.GetComponent<Image>().color = SuccessColor;
            }
            // show high result --> three stars
            else if(result > 0.833) {
                star1.GetComponent<Image>().color = SuccessColor;
                star2.GetComponent<Image>().color = SuccessColor;
                star3.GetComponent<Image>().color = SuccessColor;

            }

            Debug.Log(" #### " + result + " #### " );
        }
    }



}
