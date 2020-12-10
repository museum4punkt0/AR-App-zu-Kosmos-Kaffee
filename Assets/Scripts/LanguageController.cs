using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.ARFoundation;


public class LanguageController : MonoBehaviour
{

    public bool isInhouse;

    public ARPlaneManager planeManager;

    public TextMeshProUGUI MoveDevice;

    
    [TextArea]
    public string[] MoveDeviceText;
    public TextMeshProUGUI TapToPlace;
    
    [TextArea]
    public string[] TapToPlaceText;
    public TextMeshProUGUI Tutorial;
    
    [TextArea]
    public string[] ScanMarker1Text;
    public TextMeshProUGUI ScanMarker1;

    [TextArea]
    public string[] ScanMarkerTutorialText;
    public TextMeshProUGUI ScanMarkerTutorial;

        [TextArea]
    public string[] DownloadMarkerText;
    public TextMeshProUGUI DownloadMarker;

    [TextArea]
    public string[] NoMarkerText;
    public TextMeshProUGUI NoMarker;
  
    [TextArea]
    public string[] TutorialText;

    public TextMeshProUGUI Teaser1;
    [TextArea]
    public string[] Teaser1Text;

    public TextMeshProUGUI TutorialRain;
    [TextArea]
    public string[] TutorialRainText;

    public TextMeshProUGUI Teaser2;
    [TextArea]
    public string[] Teaser2Test;

    public TextMeshProUGUI RewardGame1;
    [TextArea]
    public string[] RewardGame1Text;

    public TextMeshProUGUI Teaser3;
    [TextArea]
    public string[] Teaser3Text;

    public TextMeshProUGUI Teaser4;
    [TextArea]
    public string[] Teaser4Text;

    public TextMeshProUGUI TutorialSun;
    [TextArea]
    public string[] TutorialSunText;

    public TextMeshProUGUI RewardGame2;
    [TextArea]
    public string[] RewardGame2Text;

    public TextMeshProUGUI Teaser5;
    [TextArea]
    public string[] Teaser5Text;

    public TextMeshProUGUI Reward1;
    [TextArea]
    public string[] RewardText1;
    public TextMeshProUGUI Reward2;
    [TextArea]
    public string[] RewardText2;
    public TextMeshProUGUI Reward3;
    [TextArea]
    public string[] RewardText3;
    public TextMeshProUGUI Reward4;
    [TextArea]
    public string[] RewardText4;
    public TextMeshProUGUI Reward5;
    [TextArea]
    public string[] RewardText5;
    public TextMeshProUGUI Reward6;
    [TextArea]
    public string[] RewardText6;
    public TextMeshProUGUI ButtonRestart;
    [TextArea]
    public string[] ButtonRestartText;
    public TextMeshProUGUI ButtonExplore;
    [TextArea]
    public string[] ButtonExploreText;
    public TextMeshProUGUI Month;
    [TextArea]
    public string[] MonthText;


    public GameObject resultText; 



    // Start is called before the first frame update
    void Start()
    {


        // setLanguage(1);

    }


    // int lang
    public void setLanguage(int language) {

            planeManager.enabled = true;

            // slightly move text of reward screen
            if(language == 1) {
                if(resultText != null) {
                    if(isInhouse) resultText.transform.position = new Vector2(resultText.transform.position.x-130, resultText.transform.position.y); 
                    else resultText.transform.position = new Vector2(resultText.transform.position.x-70, resultText.transform.position.y);
                }
            }

            MoveDevice.text = MoveDeviceText[language];
            TapToPlace.text = TapToPlaceText[language];
            ScanMarker1.text = ScanMarker1Text[language];
            if(ScanMarkerTutorial != null) ScanMarkerTutorial.text = ScanMarkerTutorialText[language];
            if(DownloadMarker != null) DownloadMarker.text = DownloadMarkerText[language];
            NoMarker.text = NoMarkerText[language];
            Tutorial.text = TutorialText[language];
            Teaser1.text = Teaser1Text[language];
            TutorialRain.text = TutorialRainText[language];
            Teaser2.text = Teaser2Test[language];
            RewardGame1.text = RewardGame1Text[language];
            Teaser3.text = Teaser3Text[language];
            Teaser4.text = Teaser4Text[language];
            TutorialSun.text = TutorialSunText[language];
            RewardGame2.text = RewardGame2Text[language];
            Teaser5.text = Teaser5Text[language];
            Reward1.text = RewardText1[language];
            Reward2.text = RewardText2[language];
            Reward3.text = RewardText3[language];
            Reward4.text = RewardText4[language];
            Reward5.text = RewardText5[language];
            Reward6.text = RewardText6[language];
            Reward4.text = RewardText4[language];
            ButtonRestart.text = ButtonRestartText[language];
            ButtonExplore.text = ButtonExploreText[language];
            Month.text = MonthText[language];



    }

    // Update is called once per frame


    void Update()
    {
        
    }
}
