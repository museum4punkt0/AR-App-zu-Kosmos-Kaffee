using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WaterController : MonoBehaviour
{
    public Image waterFill;
    public Light mainLight;


    public ParticleSystem rain;
    private ParticleSystem.MainModule mainRain;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        waterFill.fillAmount -= 0.001f;
    }

    public void raiseWater()
    {
        waterFill.fillAmount += 0.1f;
        mainLight.intensity -= 0.2f;

        var alpha = mainRain.startColor.color.a;
        alpha = alpha += .5f;
        mainRain.startColor = new Color(1f, 1f, 1f, alpha);
    }

}
