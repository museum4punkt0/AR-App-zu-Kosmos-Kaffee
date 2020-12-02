using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LostInTheGarden.KaffeeKosmos;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class TimeController : MonoBehaviour
{
    [Header("Plant")]
    public CoffeePlant tree; 
    public bool treeIsLowQuality;

    [Header("Timeline")]

    public GameObject timeline;
    public Image timelineProgressRound;
    public Image timelineProgressLine;
    public float gameDurationTotal = 240f;
    public TextMeshProUGUI month_number;



    [Header("GamePlay")]
    public GameObject rewardScreen;
    private Animator rewardScreenAnimator;

    public GameObject exploreBackButton;


    [Header("GamePlay")]
    public GameLogicController gameLogicControllerSun;
    public GameLogicController gameLogicControllerRain;
	public float rainGameplayDuration = 15f;
	public float sunGamePlayDuration = 15f;

    public CountdownController countDownControllerSun; 
    public CountdownController countDownControllerRain; 
    public float health_value = 1;
    public GameObject gameplayUIRain;
    public GameObject gameplayUIHintRain;
    public GameObject gameplayUISun;
    public GameObject gameplayUIHintSun;
    public GameObject growButtonIndicator;    
    public GameObject growButtonObj;

    public GameObject rainButton;
    public GameObject sunButton;

    public Material growButtonMaterial; 
    public Material rainButtonMaterial;
    public Material sunButtonMaterial;

    public GameObject Clouds; 


    private Button growButton; 
    private bool isGrowButtonActive = true;

    public ParticleSystem growParticleSystem;
    

    public Image WaterImage; 
    public Image SunImage; 


    private static bool isPlaying = false;
    private float lastFillAmount = 0f;
    private bool isGrowthFinished = false;

    private int growthFactor = -1;

    private Animator gameplayUIAnimRain;
    private Animator gameplayUIHintAnimRain;
    private Animator gameplayUIAnimSun;
    private Animator gameplayUIHintAnimSun;

    private Animator growButtonAnim;

    private bool isGameplayUIvisible = false;


    // timer stuff
	float timer;
	static bool rainTimerRunning;
	static bool sunTimerRunning;
    bool rainGameplayAlreadyStarted = false;
    bool sunGameplayAlreadyStarted = false;

    bool isFlowering = false;
    bool isRipening = false;

    public GameObject miniReward;
    public GameObject miniReward2;

    public GameObject tutorialStart;
    
    
    [Header("Teaser")]
    public GameObject teaser1;
    public GameObject teaser2;
    public GameObject teaser3;
    public GameObject teaser4;
    public GameObject teaser5;
    

    private bool rainButtonSetActive = false;
    private bool sunButtonSetActive = false;

    public Sprite FlowerImage;
    public Sprite CherryImageGreen;
    public Sprite CherryImageRed; 

    public Material flowerMaterial;
    public Material cherryGreenMaterial;
    public Material cherryRedMaterial;




    // Start is called before the first frame update
    void Start()
    {
        // reset the timeline indicator 
        // timelineProgressRound.fillAmount = 0f; 
        timelineProgressLine.fillAmount = 0f; 
        
        // event when tree finished growing
        tree.PlantFinishedGrowing += OnFinishedGrowing;

        tree.IsLowQuality = treeIsLowQuality;

        // get gameplay ui animator
        gameplayUIAnimRain = gameplayUIRain.GetComponent<Animator>();
        gameplayUIHintAnimRain = gameplayUIHintRain.GetComponent<Animator>();
        gameplayUIAnimSun = gameplayUISun.GetComponent<Animator>();
        gameplayUIHintAnimSun = gameplayUIHintSun.GetComponent<Animator>();
        growButtonAnim = growButtonIndicator.GetComponent<Animator>();

        // get grow button
        growButton = growButtonObj.GetComponent<Button>();


        // switch buttons 
        growButtonObj.SetActive(true);
        growParticleSystem.GetComponent<Renderer>().material = growButtonMaterial;
        rainButton.SetActive(false);
        sunButton.SetActive(false);


        // rewardScreenAnimator = rewardScreen.GetComponent<Animator>(); // 

    }

    // Update is called once per frame
    void Update()
    {

        // tutorial start
        if(tutorialStart.activeSelf && lastFillAmount > 0.07) tutorialStart.GetComponent<Animator>().SetBool("show", false);

        // show teaser 1 
        if(lastFillAmount > 0.3 && lastFillAmount < 0.4) {if (!teaser1.activeSelf) teaser1.SetActive(true);}
        else if(teaser1.activeSelf) teaser1.GetComponent<Animator>().SetBool("show", false);

        // show teaser 2
        if(lastFillAmount > 0.65 && lastFillAmount < 0.7) {if (!teaser2.activeSelf) teaser2.SetActive(true);}
        else if(teaser2.activeSelf) teaser2.GetComponent<Animator>().SetBool("show", false);

        // show teaser 3 
        if(lastFillAmount > 0.77 && lastFillAmount < 0.79) {
            if (!teaser3.activeSelf) teaser3.SetActive(true);
            // change to flower grow button
            growButtonObj.GetComponent<Image>().overrideSprite = FlowerImage;
            growParticleSystem.GetComponent<Renderer>().material = flowerMaterial;
        }
        else if(teaser3.activeSelf) teaser3.GetComponent<Animator>().SetBool("show", false);

        // show teaser 4
        if(lastFillAmount > 0.8 && lastFillAmount < 0.82) {
            if (!teaser4.activeSelf) teaser4.SetActive(true);
            // change to cherry growbutton
            growButtonObj.GetComponent<Image>().overrideSprite = CherryImageGreen;
            growParticleSystem.GetComponent<Renderer>().material = cherryGreenMaterial;


        }
        else if(teaser4.activeSelf) teaser4.GetComponent<Animator>().SetBool("show", false);;

        // show teaser 5 
        if(lastFillAmount > 0.93 && lastFillAmount < 0.97) {if (!teaser5.activeSelf) teaser5.SetActive(true);}
        // else if(teaser5.activeSelf) teaser5.SetActive(false);
        else if(teaser5.activeSelf) teaser5.GetComponent<Animator>().SetBool("show", false);

        // start timer for first game
        // if(growthFactor == -1 && lastFillAmount > 0.56f && !rainGameplayAlreadyStarted) {
        if(growthFactor == -1 && lastFillAmount > 0.54f && !rainGameplayAlreadyStarted) {
            if (WaterImage.fillAmount > 0.467f) startTimer(gameLogicControllerRain, rainGameplayDuration); 
            if(Clouds) Clouds.SetActive(true);
        }
        // start timer for second game
        if(growthFactor == -1 && lastFillAmount > 0.85f && !sunGameplayAlreadyStarted) {
            if (SunImage.fillAmount > 0.467f) startTimer(gameLogicControllerSun, sunGamePlayDuration); 
        }

        // set if isflowering flag
        if(lastFillAmount > 0.7f && lastFillAmount < 0.85f) isFlowering = true;
        else isFlowering = false;

        // set if isRipening flag
        if(lastFillAmount > 0.85f && lastFillAmount < 1) isRipening = true;
        else isRipening = false;


        if(growthFactor > 0) {
            var dt = Time.deltaTime / gameDurationTotal;
            for (int i = 0; i < growthFactor; i++)
            {
                // play - fill the timeline indicator 
                // timelineProgressRound.fillAmount += 1.0f / gameDurationTotal * Time.deltaTime;
                timelineProgressLine.fillAmount += 1.0f / gameDurationTotal * Time.deltaTime;
               
                // calculate next month 
                month_number.text = Mathf.RoundToInt(Scale(0f, 1f, 1, 36, timelineProgressLine.fillAmount)).ToString() + '.';
                // save last fill amount 
                lastFillAmount = timelineProgressLine.fillAmount;
                // grow the tree 
                // Debug.Log("Grow: " + dt + ", " + health_value + " || " + gameLogicController.GetResult());
                tree.Grow(dt, health_value);

                // First Game !!!
                if(lastFillAmount > 0.54 && !rainGameplayAlreadyStarted)  {

                    if(growButtonAnim.GetBool("active")) growButtonAnim.SetBool("active", false);

                    growButton.interactable = false; 
                    isGrowButtonActive = false;
                    Time.timeScale = 1;

                    countDownControllerRain.setDuration(rainGameplayDuration);


                    // show tutorial and gameplay UI 
                    gameplayUIAnimRain.SetBool("show", true);
                    gameplayUIHintAnimRain.SetBool("show", true);
                    gameLogicControllerRain.tutorialActive = true; // set tutorialAtctive true

                    // Debug.Log(WaterImage.fillAmount);   
                    // switch buttons
                    EndGrow();
                    growButtonObj.SetActive(false);
                    rainButton.SetActive(true);

                    if(!rainButtonSetActive) {
                        rainButton.GetComponent<UnityEngine.EventSystems.EventTrigger>().enabled = false;
                        Invoke("ActivateRainButton", 1); 
                        rainButtonSetActive = true;
                    }

                    growParticleSystem.GetComponent<Renderer>().material = sunButtonMaterial;

                    growButtonObj.SetActive(false);
                    gameLogicControllerRain.pauseGameplay();

                }


                // Second Game !!!
                if(lastFillAmount > 0.85 && !sunGameplayAlreadyStarted)  {


                    Debug.Log("The Second Game just started!");
                    if(growButtonAnim.GetBool("active")) growButtonAnim.SetBool("active", false);

                    growButton.interactable = false; 
                    isGrowButtonActive = false;
                    Time.timeScale = 1;

                    countDownControllerSun.setDuration(sunGamePlayDuration);


                    // show tutorial and gameplay UI 
                    gameplayUIAnimSun.SetBool("show", true);
                    gameplayUIHintAnimSun.SetBool("show", true);
                    gameLogicControllerSun.tutorialActive = true; // set tutorialAtctive true

                    // Debug.Log(WaterImage.fillAmount);   
                    // switch buttons
                    sunButton.SetActive(true);

                    EndGrow();
                    growButtonObj.SetActive(false);
                    sunButton.SetActive(true);

                    if(!sunButtonSetActive) {
                        sunButton.GetComponent<UnityEngine.EventSystems.EventTrigger>().enabled = false; 
                        Invoke("ActivateSunButton", 1); 
                        sunButtonSetActive = true;
                    }


                    growParticleSystem.GetComponent<Renderer>().material = rainButtonMaterial;

                    growButtonObj.SetActive(false);
                    gameLogicControllerSun.pauseGameplay();
                }


            }
        }        

        // if timer is running -> do actual gameplay stuff
        if(rainTimerRunning) gameplayTimer(rainGameplayDuration, "rain");
        if(sunTimerRunning) gameplayTimer(sunGamePlayDuration, "sun");

    }


    // grow automatically duing gameplay ... 
    public void growAutomatically(int factor, string type) {
            // growButtonAnim.SetBool("active", false);
            isPlaying = true;
            if(type == "rain") {
                health_value = gameLogicControllerRain.GetHealthValue(); 
            }
            if(type == "sun") {
                health_value = gameLogicControllerSun.GetHealthValue(); 
            } 
            growthFactor = factor;
            Time.timeScale = 1f; // dont put it 0.5 works weird with the timer 
    }

    // on grow button ... factor 1 = normal growth 
    public void OnGrowButton(int factor) {
        // keep alsways the best health value when pressing the grow button


        // NOT NEEDED ANYMORE, REMOVE SOONER OR LATER
        //  hide tutorialStart if it is active in scene
        // if(tutorialStart.activeSelf) {
        //     tutorialStart.SetActive(false);
        // }


        if(isGrowButtonActive) {
            // Debug.Log("grow active: " + factor);
            if(!growButtonAnim.GetBool("active")) growButtonAnim.SetBool("active", true);
            health_value = 1; 
            if(isFlowering) growthFactor = 1;
            else if(isRipening) growthFactor = 2;
            else growthFactor = factor;
            Time.timeScale = 1; 
            growParticleSystem.Play();


            if(miniReward.activeSelf) {
                miniReward.GetComponent<Animator>().SetBool("show", false);
            }
            if(miniReward2.activeSelf) {
                miniReward2.GetComponent<Animator>().SetBool("show", false);
            }
        }
    }

    public void EndGrow() {
        if(growButtonAnim.GetBool("active")) growButtonAnim.SetBool("active", false);
        growthFactor = -1;
        isPlaying = false;
        growParticleSystem.Stop();

    }

    private void OnFinishedGrowing()
    {
        rewardScreen.SetActive(true);
        isGrowthFinished = true;   
        print("Finished Growing");
        
        // TempImage.fillAmount = 0.58f;
        
    }

    public void ResetApp() {
        
        // SEND RESET EVENT! 
        ReportRestart(13);

        // reset app completely
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);


        // rewardScreenAnimator = rewardScreen.GetComponent<Animator>();
        // rewardScreenAnimator.SetBool("show", false);

        // // timelineProgressRound.fillAmount = 0f;
        // timelineProgressLine.fillAmount = 0f;
        // rainGameplayAlreadyStarted = false;
        // sunGameplayAlreadyStarted = false;
        // lastFillAmount = 0f;
        // WaterImage.fillAmount = 0f;
        // SunImage.fillAmount = 0f;
        // tree.ResetPlant();
        // print("App resetededed");
    }

    public void enableExploreMode() {
        if(rewardScreen.activeSelf) {
            rewardScreen.GetComponent<Animator>().SetBool("show", false);
        }
        if(timeline.GetComponent<Animator>().GetBool("show")) timeline.GetComponent<Animator>().SetBool("show", false);
        exploreBackButton.SetActive(true);        
    }

    public void disableExploreMode() {
        rewardScreen.SetActive(true);
        exploreBackButton.GetComponent<Animator>().SetBool("show", false);
    }

    public void ReportRestart(int secretID){

        AnalyticsEvent.Custom("app_reset_btn", new Dictionary<string, object>
        {
            { "secret_id", secretID },
            { "time_elapsed", Time.timeSinceLevelLoad }
        });
    }



    public static bool GetIsPlaying()
    {
        return isPlaying;
    }

    // scale value between two ranges
    public float Scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return NewValue;
    }



    void gameplayTimer(float duration, string type) {
        
        // diable grow button
        growButton.interactable = false; 

        // gameplay is running ... 
        timer += Time.deltaTime;
        if(type == "rain") {
            rainGameplayAlreadyStarted = true; 
            growAutomatically(2, type);

        } 
        else if(type == "sun") {
            sunGameplayAlreadyStarted = true;
            growAutomatically(1, type);

        }

       
        // simulate that growbutton is pressed to make it grow automatically

		if (timer > duration) 
		{
            // hide ui elements 
            gameplayUIAnimRain.SetBool("show", false);
            gameplayUIAnimSun.SetBool("show", false);
            isGameplayUIvisible = false; 
            growthFactor = -1;
            EndGrow();

			timer = 0f;
            if(type == "rain") rainTimerRunning = false;
            else if(type == "sun")  sunTimerRunning = false;

            growButton.interactable = true;
            isGrowButtonActive = true;
            health_value = gameLogicControllerRain.GetResult();
            isPlaying = false;
            
            // show mini reward
            if(type == "sun") {
                miniReward2.SetActive(true);

                growButtonObj.GetComponent<Image>().overrideSprite = CherryImageRed;
                growParticleSystem.GetComponent<Renderer>().material = cherryRedMaterial;
            }
            if(type == "rain") {
                miniReward.SetActive(true);
                growParticleSystem.GetComponent<Renderer>().material = growButtonMaterial;
            }

            // Switch Buttons Back
            growButtonObj.SetActive(true);

            // growButtonObj.GetComponent<Button>().interactable = false;
            gameObject.GetComponent<Animator>().SetBool("active", false);
            growButtonObj.GetComponent<UnityEngine.EventSystems.EventTrigger>().enabled = false;
            Invoke("ActivateButton", 1f);


            if(type == "sun") {
                // growthFactor = 1;
            }

            // deactivate whole button

            rainButton.SetActive(false);
            sunButton.SetActive(false);
            

            
            if(type == "sun") {
                gameLogicControllerSun.gamePlayEnabled = false;
            }
            else if (type == "rain") {
                gameLogicControllerRain.gamePlayEnabled = false;
            }

		}
    }

    public void setTimerRunning(bool isRunning) {
        // timerRunning = isRunning;
    }

    public void ActivateButton() {
        // growButtonObj.GetComponent<Button>().interactable = true;
        growButtonObj.GetComponent<UnityEngine.EventSystems.EventTrigger>().enabled = true;
    }

    public void ActivateRainButton() {
        rainButton.GetComponent<UnityEngine.EventSystems.EventTrigger>().enabled = true;

    }

    public void ActivateSunButton() {
        sunButton.GetComponent<UnityEngine.EventSystems.EventTrigger>().enabled = true;
    }

    public void ActivateGrowButton() {
        growButton.GetComponent<UnityEngine.EventSystems.EventTrigger>().enabled = true;
    }

    public void PausePlantGrowth() {
        // set grow factor to a negative value to stop growth 
        growthFactor = -1;
    }

    public void startTimer(GameLogicController controller, float duration) {
        print("lets start the timer ... play play play");
        gameplayUIHintAnimRain.SetBool("show", false);
        gameplayUIHintAnimSun.SetBool("show", false);

        if(controller.sunOnly) {
            controller.startGamePlay(countDownControllerSun, duration);
            sunTimerRunning = true;

        } 
        else {
            controller.startGamePlay(countDownControllerRain, duration);
            rainTimerRunning = true;
        }

    }


    public void showStartTutorial() {
        tutorialStart.SetActive(true);
    }

}
