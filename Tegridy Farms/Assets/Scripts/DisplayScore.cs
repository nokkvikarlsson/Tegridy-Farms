using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayScore : MonoBehaviour {

    private TextMeshProUGUI _totalMoneyText;
    private GameController _gameController;

    void Awake() {
        _gameController = FindObjectOfType<GameController>();
        _totalMoneyText = GetComponent<TextMeshProUGUI>();

        //Uncomment this to delete all preferences saved. (Highscores and options)
        //Debug.Log("Deleting playerprefs");
        //PlayerPrefs.DeleteAll();
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        //Display();
    }

    public void Display () {

        int day = _gameController.getDayCounter();
        int totalMoney = _gameController.totalMoneyEarned;
        int totalScore = day * totalMoney;

        if(totalScore < 0)
        {
            totalScore = 0;
        }

        //If the totalScore is higher than the current highscore then save that as the highscore
        //and move every score down by one place.
        if (totalScore >= PlayerPrefs.GetInt("HighestScore", 0))
        {

            FifthToFourth(1);

            PlayerPrefs.SetInt("HighestScore", totalScore);
            _totalMoneyText.text = "Total money earned:" + totalMoney + "$\n" + "Total days lasted: " + day.ToString() + "\nNew highscore: " + totalScore;

        }
        //If the total score is lower than the highscore then display the score and the current highscore.
        else
        {

            int currentHighscore = PlayerPrefs.GetInt("HighestScore", 0);
            _totalMoneyText.text = "Total money earned:" + totalMoney + "$\n" + "Total days lasted: " 
                                    + day.ToString() + "\nTotal Score:" + totalScore + "\nCurrent highscore: " + currentHighscore;


            //Cheks if the score is lower then the score in this place above and if its higher then the current place in the scoreboard.
            //If it is the move the values in the places below it down by one place
            if (totalScore >= PlayerPrefs.GetInt("2HighestScore", 0) && totalScore < PlayerPrefs.GetInt("HighestScore"))
            {
                FifthToFourth(2);
                PlayerPrefs.SetInt("2HighestScore", totalScore);
            }
            else if (totalScore >= PlayerPrefs.GetInt("3HighestScore", 0) && totalScore < PlayerPrefs.GetInt("2HighestScore"))
            {
                FifthToFourth(3);
                PlayerPrefs.SetInt("3HighestScore", totalScore);
            }
            else if (totalScore >= PlayerPrefs.GetInt("4HighestScore", 0) && totalScore < PlayerPrefs.GetInt("3HighestScore"))
            {
                FifthToFourth(4);
                PlayerPrefs.SetInt("4HighestScore", totalScore);
            }
            else if (totalScore >= PlayerPrefs.GetInt("5HighestScore", 0) && totalScore < PlayerPrefs.GetInt("4HighestScore"))
            {
                PlayerPrefs.SetInt("5HighestScore", totalScore);
            }

        }

    }


    //Helper functions that move the score to the place below it
    private void SecondToFirst()
    {
        PlayerPrefs.SetInt("2HighestScore", PlayerPrefs.GetInt("HighestScore"));

    }

    private void ThirdToSecond(int stop)
    {
        PlayerPrefs.SetInt("3HighestScore", PlayerPrefs.GetInt("2HighestScore"));

        if(stop != 2)
        { 
            SecondToFirst();
        }

    }

    private void FourthToThird(int stop)
    {
        PlayerPrefs.SetInt("4HighestScore", PlayerPrefs.GetInt("3HighestScore"));

        if(stop != 3)
        { 
            ThirdToSecond(stop);
        }

    }

    private void FifthToFourth(int stop)
    {
        PlayerPrefs.SetInt("5HighestScore", PlayerPrefs.GetInt("4HighestScore"));

        if (stop != 4)
        {
            FourthToThird(stop);
        }
    }
}
