using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeepMusic : MonoBehaviour {


	void Awake ()
    {

        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {

        AudioSource backgroundMusic = GetComponent<AudioSource>();

        //Checks if the current scene is either the main or nokkvi killa's main.
        //If it is then destroy the gameobject.
        Scene currentScene = SceneManager.GetActiveScene();
        int buildIndex = currentScene.buildIndex;
        if (buildIndex == 1 || buildIndex == 3)
        {
            backgroundMusic.Stop();
        }
        else if(!backgroundMusic.isPlaying)
        {
            backgroundMusic.Play();
        }
    }

}
