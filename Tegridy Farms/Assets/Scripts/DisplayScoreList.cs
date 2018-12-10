using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayScoreList : MonoBehaviour {


    private TextMeshProUGUI _highScoreText;

    // Use this for initialization
    void Awake () {
        _highScoreText = GetComponent<TextMeshProUGUI>();
    }
	
	// Update is called once per frame
	void Update () {

        int highestScore = PlayerPrefs.GetInt("HighestScore", 0);
        int highestScore2nd = PlayerPrefs.GetInt("2HighestScore", 0);
        int highestScore3rd = PlayerPrefs.GetInt("3HighestScore", 0);
        int highestScore4rd = PlayerPrefs.GetInt("4HighestScore", 0);
        int highestScore5th = PlayerPrefs.GetInt("5HighestScore", 0);


        _highScoreText.text = "1. " + highestScore + 
                            "\n2. " + highestScore2nd +
                            "\n3. " + highestScore3rd +
                            "\n4. " + highestScore4rd +
                            "\n5. " + highestScore5th;

	}
}
