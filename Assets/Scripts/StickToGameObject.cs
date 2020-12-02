using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickToGameObject : MonoBehaviour
{

    public GameObject stickyObject; 
    public GameObject fallbackObject; 
    Renderer m_Renderer;

    // Start is called before the first frame update
    void Start()
    {
        m_Renderer = gameObject.GetComponent<Renderer>();
        fallbackObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        // print(m_Renderer.isVisible);

        Vector3 stickyPos = Camera.main.WorldToScreenPoint(this.transform.position);
        
        stickyObject.transform.position = stickyPos;

            if(!m_Renderer.isVisible) {
            fallbackObject.SetActive(true);
        }
        else fallbackObject.SetActive(false);
    }
}
