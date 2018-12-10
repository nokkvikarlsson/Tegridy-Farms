using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayScore : MonoBehaviour {

    private TextMeshProUGUI _totalMoneyText;
    private GameController _gameController;
    private int money;

    void Awake() {
        _gameController = FindObjectOfType<GameController>();
        _totalMoneyText = GetComponent<TextMeshProUGUI>();
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

        //If the totalScore is higher than the current highscore then save that as the highscore.
        if (totalScore >= PlayerPrefs.GetInt("HighestScore", 0))
        {
            Debug.Log("HALLO er í A");

            if (PlayerPrefs.GetInt("HighestScore", 0) != 0)
            {
                Debug.Log("hey er hérna");
                PlayerPrefs.SetInt("2HighestScore", PlayerPrefs.GetInt("HighestScore"));
            }

            PlayerPrefs.SetInt("HighestScore", totalScore);
            _totalMoneyText.text = "Total money earned:" + totalMoney + "$\n" + "Total days lasted: " + day.ToString() + "\nNew highscore: " + totalScore;

            

        }
        //If the total score is lower than the highscore then display the score and the current highscore.
        else
        {
            Debug.Log("HALLO er i B");

            int currentHighscore = PlayerPrefs.GetInt("HighestScore", 0);
            _totalMoneyText.text = "Total money earned:" + totalMoney + "$\n" + "Total days lasted: " 
                                    + day.ToString() + "\nTotal Score:" + totalScore + "\nCurrent highscore: " + currentHighscore;


            //Cheks if the score is lower then the place above and if its higher then the current place in the scoreboard.
            if (totalScore >= PlayerPrefs.GetInt("2HighestScore", 0) && totalScore < PlayerPrefs.GetInt("HighestScore"))
            {
                //Give third place the value of second place
                PlayerPrefs.SetInt("3HighestScore", PlayerPrefs.GetInt("2HighestScore"));
                PlayerPrefs.SetInt("2HighestScore", totalScore);
            }
            if (totalScore >= PlayerPrefs.GetInt("3HighestScore", 0) && totalScore < PlayerPrefs.GetInt("2HighestScore"))
            {
                PlayerPrefs.SetInt("4HighestScore", PlayerPrefs.GetInt("3HighestScore"));
                PlayerPrefs.SetInt("3HighestScore", totalScore);
            }
            if (totalScore >= PlayerPrefs.GetInt("4HighestScore", 0) && totalScore < PlayerPrefs.GetInt("3HighestScore"))
            {
                PlayerPrefs.SetInt("5HighestScore", PlayerPrefs.GetInt("4HighestScore"));
                PlayerPrefs.SetInt("4HighestScore", totalScore);
            }
            if (totalScore >= PlayerPrefs.GetInt("5HighestScore", 0) && totalScore < PlayerPrefs.GetInt("4HighestScore"))
            {
                PlayerPrefs.SetInt("5HighestScore", totalScore);
            }
        }

    }

}
