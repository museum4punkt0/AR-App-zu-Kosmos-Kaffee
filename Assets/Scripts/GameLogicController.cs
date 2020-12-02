using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LostInTheGarden.KaffeeKosmos;
using TMPro;


public class GameLogicController : MonoBehaviour
{


    public bool isCounting; 

    public static GameLogicController Instance = null;


    // the one and only coffeeplant
    public CoffeePlant tree;

    // enable/disable gameplay
    public bool gamePlayEnabled = false; // todo: refactor to gamePlayActive
    public bool tutorialActive = false;

    public bool rainOnly;
    public bool sunOnly;

    // temperature & rain ui elements
    public Image temperatureFillImage;
    public Image rainFillImage;
    
    public ParticleSystem indicatorParticles;


    public Image indicatorImage;


    public float temperatureSweetSpotValue = 0.5f;
    public float waterSweetSpotValue = 0.5f;
    private float currentTemperatureValue;
    private float currentWaterValue;

    private float healthResult;
    private float fruitPercentage = 0f;
    public float fruitValue = 0f;

    public Slider healthSlider;
    public Slider fruitSlider;

    public GameObject SweetspotArrowLeft; 
    public GameObject SweetspotArrowRight;



    public TextMeshProUGUI resultGramm;
    public TextMeshProUGUI resultCoffeeCups;


    private float GamePlayDuration;



    // start is called before the first frame update
    void Start()
    {
        SweetspotArrowLeft.SetActive(false);
        SweetspotArrowRight.SetActive(false);

        
        if(Instance == null) {
            Instance = this;
        }

        // fruit-ratio callback from the coffeetree
        tree.FruitRatio = () => { 
            // scale value to 0-1 to define the percentage of fruits
            fruitPercentage = Scale(0f, GamePlayDuration, 0f, 1f, fruitValue);
            print("fruitValue: " + fruitValue + "|| fruitPercentage: " + fruitPercentage);
            // if(fruitPercentage < .2f) fruitPercentage = 0.01f;

            if(rainOnly) {
                resultGramm.text = Mathf.RoundToInt(Scale(0,1,0,500, fruitPercentage)).ToString();
                resultCoffeeCups.text = Mathf.RoundToInt(Scale(0,1,0,500, fruitPercentage)/10).ToString();
            }

            return fruitPercentage;
        };


        indicatorParticles.Stop();
        indicatorImage.color = new Color32(255,255,255,255); // white  


        // CountdownController.Instance.setDuration(GamePlayDuration);
    }

    // Update is called once per frame
    void Update()
    {
        // indicatorImage.color = new Color32(2, 128, 61, 255); // green  

        if(gamePlayEnabled && isCounting)
        {
            // increase fruitcount every second by the computed health value
            fruitValue += ComputeHealthValue() * Time.deltaTime; 
        }
        else {
            if(isCounting) ComputeHealthValue();
            // if(!rainOnly || !sunOnly) temperatureFillImage.fillAmount = temperatureSwe   etSpotValue;
        } 
    }

    private float ComputeHealthValue() {

        float result;

        if(rainOnly || sunOnly) {
            // save the acutal values for each frame
            currentWaterValue = rainFillImage.fillAmount;

            // print("currentWaterValue: " + currentWaterValue + " || WaterSweetspotValue: " + waterSweetSpotValue);

            // calculate the absolute difference between sweetspot and current value 
            float waterSweetSpotDiff = Mathf.Abs(waterSweetSpotValue - currentWaterValue);
            result = 1f - waterSweetSpotDiff*2;

            if(waterSweetSpotDiff < 0.07f) ShowGoodIndicator();
            else {
                // show arrows here !!! 
                indicatorParticles.Stop();
                indicatorImage.color = new Color32(255, 255, 255, 255); // white 

                SweetspotArrowLeft.SetActive(true);
                SweetspotArrowRight.SetActive(true);
                SweetspotArrowLeft.GetComponent<Animator>().SetBool("pulse", true);
                SweetspotArrowRight.GetComponent<Animator>().SetBool("pulse", true);

            } 
        }
        else {
            // save the acutal values for each frame
            // currentTemperatureValue = 0;
            currentTemperatureValue = temperatureFillImage.fillAmount;
            currentWaterValue = rainFillImage.fillAmount;

            // calculate the absolute difference between sweetspot and current value 
            float tempSweetSpotDiff = Mathf.Abs(temperatureSweetSpotValue - currentTemperatureValue);
            float waterSweetSpotDiff = Mathf.Abs(waterSweetSpotValue - currentWaterValue);

            float tempWaterDiffSum = tempSweetSpotDiff + waterSweetSpotDiff;
            result = 1f - tempWaterDiffSum;


        }


        //Debug.Log(temperatureSweetSpotValue + " - " + currentWaterValue + " = " + tempSweetSpotDiff);
        //Debug.Log(waterSweetSpotValue + " - " + currentTemperatureValue + " = " + waterSweetSpotDiff);
        //Debug.Log("SUM: " + tempWaterDiffSum + " || Inverted: " + (result))
         
        // Debug.Log("Result: " + result);

        healthResult = result;

        return result;
    }



    public float Scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return NewValue;
    }
   
     public void ResetGamePlay()
    {
        // reset fill values of UI elements to default
        currentTemperatureValue = .5f;
        currentWaterValue = .5f;

        // reset game play values
        healthResult = 0f;
        fruitPercentage = 0f;
        fruitValue = 0f;
        // gamePlayEnabled = false;
    }


    public void pauseGameplay() {
        gamePlayEnabled = false;
    }
    public void startGamePlay(CountdownController countdown, float duration) {
        GamePlayDuration = duration;

        ResetGamePlay();
        gamePlayEnabled = true;
        tutorialActive = false;

        print("Gameplay started! You got " + GamePlayDuration + " seconds");
        countdown.StartCountdown(GamePlayDuration);
    }



    public float GetHealthValue() {
        return healthResult;
    }

    public float GetResult() {
        var tempFruitPercentage = Scale(0f, GamePlayDuration, 0f, 1f, fruitValue);
        return tempFruitPercentage;
    }


    public float GetFruitValue() {
        return fruitValue;
    }


    public float GetTemperatureSweetSpot() {
        return temperatureSweetSpotValue;
    }

    public float GetWaterSweetSpot() {
        return waterSweetSpotValue;
    }


    private void ShowGoodIndicator() {
        indicatorParticles.Play();
        indicatorImage.color = new Color32(2, 128, 61, 255); // green  

        SweetspotArrowLeft.SetActive(false);
        SweetspotArrowRight.SetActive(false);
        SweetspotArrowLeft.GetComponent<Animator>().SetBool("pulse", false);
        SweetspotArrowRight.GetComponent<Animator>().SetBool("pulse", false);
    }


    private static GameLogicController instance;

}


