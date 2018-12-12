using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
	
	public static bool GameIsPaused = false;

	public GameObject pauseMenuUI;
    public GameObject pauseOptionsUI;

    public GameObject[] plots;

    private GameObject _UI;
    private GameObject _shop;
    private GameController _gameController;

    private void Awake()
    {
        _UI = GameObject.FindGameObjectWithTag("UI");
        _shop = Resources.FindObjectsOfTypeAll<Shop>()[0].gameObject;

        _shop.SetActive(false);

        _gameController = FindObjectOfType<GameController>();
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
        _UI.SetActive(true);
        pauseOptionsUI.SetActive(false);
    }

	private void Pause()
	{
        //Cant pause the game if the player has lost.
        if(!_gameController.hasLost)
        {
            for (int i = 0; i < plots.Length; i++)
            {
                plots[i].GetComponent<BoxCollider2D>().enabled = false;
            }

            pauseMenuUI.SetActive(true);
    		Time.timeScale = 0f;
    		GameIsPaused = true;
            
            //Hides the UI when paused
            _UI.SetActive(false);

            if(_shop.activeSelf == true)
            {
                _shop.SetActive(false);
            }
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

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("NokkviMain");
    }

}