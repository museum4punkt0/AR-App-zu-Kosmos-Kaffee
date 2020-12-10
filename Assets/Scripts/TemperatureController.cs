using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemperatureController : MonoBehaviour
{
    public Image tempFill;
    public Light mainLight;
    public ParticleSystem clouds;
    public ParticleSystem rain;

    private ParticleSystem.MainModule mainCloud;

    //Material cloudMaterial;

    // Start is called before the first frame update
    void Start()
    {
        //mainCloud = clouds.GetComponent<ParticleSystem>().main;
        //mainCloud.startColor = new Color(1f, 1f, 1f, 0.5f);

        //cloudMaterial = clouds.GetComponent<Material>();
        //clouds.GetComponent<ParticleSystemRenderer>().material.color = Color.red;
        //main.startColor = new Color(0f, 0f, 0f, 1f);
        Color color2 = clouds.GetComponent<ParticleSystem>().main.startColor.colorMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (tempFill.fillAmount < 0.33f)
        {
            //Debug.Log("blabla");
            //cloudMaterial.color = Color.red;
        }
    }

    public void raiseTemperature()
    {
        //tempFill.fillAmount += 0.1f;
        //mainLight.intensity += 0.1f;

        //var alpha = mainCloud.startColor.color.a;
        //alpha = alpha -= .2f;
        //mainCloud.startColor = new Color(1f, 1f, 1f, alpha);
    }
    public void lowerTemperature()
    {
        //tempFill.fillAmount -= 0.1f;
        //mainLight.intensity -= 0.1f;

        //var alpha = mainCloud.startColor.color.a;
        //alpha = alpha += .2f;
        //mainCloud.startColor = new Color(1f, 1f, 1f, alpha);

    }
    public void setMediumTemperature()
    {
        //if (tempFill.fillAmount <= 0.5f)
        //{
        //    tempFill.fillAmount += 0.1f;
        //}
        //else tempFill.fillAmount -= 0.1f;

        //mainLight.intensity = 1f;

    }
}
