using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaserController : MonoBehaviour
{

    public bool wasShown;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }


    void OnEnable() {
        wasShown = true;

        gameObject.GetComponent<Animator>().SetBool("show", true);


    }

    void OnHideAnimationDone() {
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        gameObject.SetActive(false);
    } 

    void OnDisable() {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
