using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weatherController : MonoBehaviour
{

    public string type;
    
    public Image indicatorFill2D;
    public Light mainLight;
    public ParticleSystem clouds;
    public ParticleSystem rain;

    public GameObject rainButton;

    // get main element of the particle elememts to set startcolor
    private ParticleSystem.MainModule mainClouds;
    private ParticleSystem.MainModule mainRain;


    public GameObject rainButtonIndicator;
    private Animator rainButtonAnim;



    public ParticleSystem rainParticleSystem;


    private float rainAlpha = 0;
    private float cloudAlpha = 1;
    private float rainDecreaseValue = 0.005f;
    private float rainIncreaseValue = 0.005f;

    private bool doAffectLight = false; 
    private float tempLightValue; 


    private bool btn_pressed = false;


    public GameLogicController gameLogicController;
    // public GameLogicController gameLogicControllerSun;


    // Start is called before the first frame update
    void Start()
    {

        mainClouds = clouds.main;
        mainRain = rain.main; 

        // set cloud color white & halftransparent
        mainClouds.startColor = new Color(1f, 1f, 1f, .5f);
        mainRain.startColor = new Color(1f, 1f, 1f, rainAlpha);

        rainButtonAnim = rainButtonIndicator.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        // print(GameLogicController.Instance.gamePlayEnabled);
        if(type == "rain") {


            if(btn_pressed) doRain();  
            else if(!btn_pressed) stopRain();
        } 
        else if(type == "sun") {


            if(gameLogicController.tutorialActive) {
                mainLight.intensity = 1f;
            }

            

            if(btn_pressed) doSun();
            else if(!btn_pressed) stopSun();
        }
    }

    public void onRainPressed() {
        btn_pressed = true; 
        rainParticleSystem.Play();
        if(!rainButtonAnim.GetBool("active")) rainButtonAnim.SetBool("active", true);
    }
    public void onRainReleased() {
        btn_pressed = false; 
        rainParticleSystem.Stop();
        if(rainButtonAnim.GetBool("active")) rainButtonAnim.SetBool("active", false);

    }

    public void doRain()
    {



        if(gameLogicController.gamePlayEnabled ||gameLogicController.tutorialActive ) {
        
            if(!rainButtonAnim.GetBool("active")) rainButtonAnim.SetBool("active", true);
                // increase the rain indicator 

            // print("#### Doing Rain | " + GameLogicController.Instance.gamePlayEnabled + " | " +  GameLogicController.Instance.tutorialActive);

            indicatorFill2D.fillAmount += rainIncreaseValue;

            // increase the rain alpha only if its not 1 to prevent endless rain
            if (rainAlpha < 1)
            {
                rainAlpha += 0.05f;
                if(rainAlpha > 1) rainAlpha = 1f;
                mainRain.startColor = new Color(1f, 1f, 1f, rainAlpha);
            }
            else mainRain.startColor = new Color(1f, 1f, 1f, 1f);
            // rainButtonAnim.SetBool("active", true);
        }
        else stopRain();
        
        if(gameLogicController.gamePlayEnabled) {
            if(!rainButtonAnim.GetBool("active")) rainButtonAnim.SetBool("active", true);
        }
    }

    public void stopRain(){
        
        if (rainAlpha > 0)
        {
            rainAlpha -= .05f;
            if(rainAlpha < 0) rainAlpha = 0f;
            mainRain.startColor = new Color(1f, 1f, 1f, rainAlpha);
        }
        else
        {
            rainAlpha = 0f;
            mainRain.startColor = new Color(1f, 1f, 1f, 0f);

        }
        if(rainButton.activeSelf) {
            if(rainButtonAnim.GetBool("active")) {
                rainButtonAnim.SetBool("active", false);
            }
        }

        indicatorFill2D.fillAmount -= rainDecreaseValue;
    }
    public void doSun()
    {
        

        if(gameLogicController.gamePlayEnabled ||gameLogicController.tutorialActive ) {
        
        if(!rainButtonAnim.GetBool("active")) rainButtonAnim.SetBool("active", true);
                // increase the rain indicator 

            // print("#### Doing Rain | " + GameLogicController.Instance.gamePlayEnabled + " | " +  GameLogicController.Instance.tutorialActive);

            indicatorFill2D.fillAmount += rainIncreaseValue;
            
            if(indicatorFill2D.fillAmount > mainLight.intensity/2) doAffectLight = true;
            

            if(doAffectLight) {
                mainLight.intensity = indicatorFill2D.fillAmount*2f;
            }


            // decrease clouds only if their alpha is bigger than 0 to prevent negative numbers
            if (cloudAlpha > 0)
            {
                cloudAlpha -= 0.05f;
                if(cloudAlpha < 0) cloudAlpha = 0f;
                mainClouds.startColor = new Color(1f, 1f, 1f, cloudAlpha);
            }
            else mainClouds.startColor = new Color(1f, 1f, 1f, 0f);
            // rainButtonAnim.SetBool("active", true);
        }
        else stopSun();

        


    }

    public void stopSun(){
        

        if (cloudAlpha < 1)
        {
            cloudAlpha += .05f;
            if(cloudAlpha > 1) cloudAlpha = 1f;
            mainClouds.startColor = new Color(1f, 1f, 1f, cloudAlpha);
        }
        else
        {
            cloudAlpha = 1f;
            mainClouds.startColor = new Color(1f, 1f, 1f, 1f);

        }
        if(rainButton.activeSelf) {
            if(rainButtonAnim.GetBool("active")) {
                rainButtonAnim.SetBool("active", false);
            }
        }
        indicatorFill2D.fillAmount -= rainDecreaseValue;

        if(gameLogicController.gamePlayEnabled || gameLogicController.tutorialActive ) {
            if(doAffectLight) mainLight.intensity = indicatorFill2D.fillAmount*2f;
        }
        // else mainLight.intensity = 1;

    }
}
