using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {


    public GameObject _optionsMenu;

    //We do this so the volume slider in options has a chance to lower the volume according
    //to the playerprefs before we set the options menu as not active.
    void Update()
    {
        _optionsMenu.SetActive(false);
    }

    public void PlayGame ()
	{
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        SceneManager.LoadScene("NokkviMain");

    }

    public void GoToCredits ()
	{
		SceneManager.LoadScene("Credits");
	}

	public void QuitGame ()
	{
		Debug.Log("QUIT!");
		Application.Quit();
	}
}
