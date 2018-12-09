using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateTotalMoney : MonoBehaviour {

    public TextMeshProUGUI totalMoneyText;
    private GameController _gameController;

    void Awake(){
        totalMoneyText = GetComponent<TextMeshProUGUI>();
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
            totalMoneyText.text = "Total money earned: " + _gameController.money.ToString() + "$";
    }


}
