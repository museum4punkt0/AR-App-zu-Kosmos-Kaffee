using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour  
{
    
    
    public GameObject timeline;
    TimeController myTimeController;


    void Start() {
        myTimeController = timeline.GetComponent<TimeController>();
    }



    public void PauseGamePlay() {
        myTimeController.PausePlantGrowth(); // sets growthFactor to -1
    }


    
}
