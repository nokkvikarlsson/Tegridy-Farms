using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateTotalMoney : MonoBehaviour {

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
        int money = _gameController.money;
        int totalScore = day * money;

        _totalMoneyText.text = "Total money earned:" + money + "$\n" + "Total days lasted: " + day.ToString() + "\nTotal Score:" + totalScore;
    }

}
