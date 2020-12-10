using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivityWatcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RestartGameInvoke();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount == 1) {
            Debug.Log("Touch happened");
            RestartGameInvoke();            
        }


    }


    void RestartGameInvoke() {
        CancelInvoke ();
        Invoke ("RestartGame", 120);
    }

    void RestartGame() {
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
