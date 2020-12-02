using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownController : MonoBehaviour
{
    public static CountdownController Instance = null;
    public float mCountdownTime = 10;

    public Image mTimeIndicator;
    public TextMeshProUGUI mTimeText;

    private float mTime;
    private bool mTimerOver = false;
    private bool mTimerRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        // StartCountdown();
        if(Instance == null) {
            Instance = this;
        }

        mTime = mCountdownTime;
        // StartCountdown();

        // disable the countdown on start
        gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if(mTimerRunning) Count();
        // else Debug.Log(mTime + " | " + mCountdownTime);
    }
    
    void Count() {


        if(mTime <= 0) {  
            mTimerOver = true;
            mTimerRunning = false; 
            Debug.Log("timer over!");   
            mTime = mCountdownTime;
            // mTime = 0;
            // mTimeText.text = Mathf.RoundToInt(mTime).ToString();
            mTimeText.text = "";

            
        }
        else {
            mTime -= Time.deltaTime;
            mTimeText.text = Mathf.RoundToInt(mTime).ToString();
            mTimeIndicator.fillAmount = Scale(0, mCountdownTime, 0, 1, mTime);
        }
    }

    public void StartCountdown(float duration) {

        gameObject.SetActive(true);

        // starting immediately repeating each second
        // InvokeRepeating("Count", 0.0f, 1.0f);
        mTime = duration; 
        mTimerRunning = true;
    }

    public void StopCountdown() {
            mTimerRunning = false; 
            CancelInvoke("Count");
    }

    public void setDuration(float duration) {
        mCountdownTime = duration;
        mTimeText.text = Mathf.RoundToInt(mCountdownTime).ToString();
        mTimeIndicator.fillAmount = 1;
    }

    public bool isTimerOver {
        get { return mTimerOver; }
    }

    public float Scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return NewValue;
    }

}
