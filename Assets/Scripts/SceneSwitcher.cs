using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{



    public void NextScene()
    {
        // get the active Scene
        Scene scene = SceneManager.GetActiveScene();
        int numberOfScenes = SceneManager.sceneCountInBuildSettings;

        // if current scene index is smaller than the number of scenes --> go to next scene
        if (scene.buildIndex < numberOfScenes - 1)
        {
            int nextSceneNumber = (int)scene.buildIndex + 1;
            SceneManager.LoadScene(nextSceneNumber, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }


}
