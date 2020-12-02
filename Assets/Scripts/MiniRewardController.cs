using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MiniRewardController : MonoBehaviour
{

    public GameObject star1; 
    public GameObject star2; 
    public GameObject star3; 

    public GameLogicController RainSunResult; 

    public Color SuccessColor = new Color(215,175,98,255);


    // Start is called before the first frame update
    void Start()
    {
        
    }

        // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable() {
        showReward();
        Invoke("doStarAnimation", 0.5f);
    }

    void showReward() {
        if(star1 != null && star2 != null && star3 != null) {
            Debug.Log("All stars are here");

            var result = RainSunResult.GetResult();

            // show low result --> one start
            if(result > 0.625f && result < 0.75f) { 
                star1.GetComponent<Image>().color = SuccessColor;
            }
            // show medium result -> two starts
            else if(result > 0.75f && result < 0.875f) {
                star1.GetComponent<Image>().color = SuccessColor;
                star2.GetComponent<Image>().color = SuccessColor;
            }
            // show high result --> three stars
            else if(result > 0.875f) {
                star1.GetComponent<Image>().color = SuccessColor;
                star2.GetComponent<Image>().color = SuccessColor;
                star3.GetComponent<Image>().color = SuccessColor;

            }

            Debug.Log(" #### " + result + " #### " );
        }
    }

    void doStarAnimation() {
        if(star1 != null && star2 != null && star3 != null) {
            Debug.Log("All stars are here");

            var result = RainSunResult.GetResult();

            // show low result --> one start
            if(result > 0.625f && result < 0.75f) { 
                star1.GetComponent<Animator>().SetTrigger("pulse");
            }
            // show medium result -> two starts
            else if(result > 0.75f && result < 0.875f) {
                star1.GetComponent<Animator>().SetTrigger("pulse");
                star2.GetComponent<Animator>().SetTrigger("pulse");

            }
            // show high result --> three stars
            else if(result > 0.875f) {
                star1.GetComponent<Animator>().SetTrigger("pulse");
                star2.GetComponent<Animator>().SetTrigger("pulse");
                star3.GetComponent<Animator>().SetTrigger("pulse");
            }
        }
    }
}
