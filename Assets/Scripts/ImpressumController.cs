using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpressumController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnEnable() {
        gameObject.GetComponent<Animator>().SetBool("show", true);
    }

    void OnDisable() {
        // gameObject.GetComponent<Animator>().SetBool("show", false);
        // if(growButton != null) growButton.GetComponent<CanvasGroup>().alpha = 1;
    }

    void OnHideAnimationDone() {
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        gameObject.SetActive(false);
    } 

    public void hide() {
        gameObject.GetComponent<Animator>().SetBool("show", false);
    }


}
