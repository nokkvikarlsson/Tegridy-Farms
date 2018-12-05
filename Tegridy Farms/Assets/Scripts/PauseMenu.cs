using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
	
	public static bool GameIsPaused = false;

	public GameObject pauseMenuUI;

    GameObject UI;

    //Does not to hide the shop. Will look at later
    GameObject Shop;

    private void Awake()
    {
        UI = GameObject.FindGameObjectWithTag("UI");

        //Does not work to hide the shop. Will look at later
        Shop = GameObject.FindGameObjectWithTag("shop");
        Shop.SetActive(false);
    }

    void Start()
    {
       
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

        //Shows the UI when resumed
        UI.SetActive(true);

    }

	private void Pause()
	{
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