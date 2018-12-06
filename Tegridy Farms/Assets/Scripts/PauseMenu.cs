using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
	
	public static bool GameIsPaused = false;

	public GameObject pauseMenuUI;

    public GameObject[] plots;

    GameObject UI;
    GameObject Shop;

    private void Awake()
    {
        UI = GameObject.FindGameObjectWithTag("UI");
        Shop = Resources.FindObjectsOfTypeAll<Shop>()[0].gameObject;

        Shop.SetActive(false);
    }

    void Start()
    {
        plots = GameObject.FindGameObjectsWithTag("plot");
    }

    // Update is called once per frame
    void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(GameIsPaused)
			{
				Resume();
			}
			else
			{
				Pause();
			}
		}
	}

	public void Resume()
	{
		pauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		GameIsPaused = false;

        for (int i = 0; i < plots.Length; i++)
        {
            plots[i].GetComponent<BoxCollider2D>().enabled = true;
        }

        //Shows the UI when resumed
        UI.SetActive(true);

    }

	private void Pause()
	{

        for(int i = 0; i < plots.Length; i++)
        {
            plots[i].GetComponent<BoxCollider2D>().enabled = false;
        }

        pauseMenuUI.SetActive(true);
		Time.timeScale = 0f;
		GameIsPaused = true;
        
        //Hides the UI when resumed
        UI.SetActive(false);

        if(Shop.activeSelf == true)
        {
            Shop.SetActive(false);
        }

    }

	public void GoToMenu()
	{
        Time.timeScale = 1f;
		SceneManager.LoadScene("MainMenu");
	}

	public void QuitGame()
	{
        Debug.Log("Quitting game...");
        Application.Quit();
	}

}