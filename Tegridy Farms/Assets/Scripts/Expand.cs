using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Expand : MonoBehaviour {

    private GameController _gameController;
    private GameObject _expansionItemCardBuyPrice;
    private int EXPANSIONPRICE = 50;

    void Awake()
    {
        _gameController = FindObjectOfType<GameController>();
    }

    // Use this for initialization
    void Start() 
    {
        _gameController.OpenShop();
        _expansionItemCardBuyPrice = GameObject.Find("/Shop/ShopWindow/UpgradesTabPane/Pane/ExpansionItemCard/BuyPrice");
        _gameController.CloseShop();
    }
	
	// Update is called once per frame
	void Update () 
    {
		
	}


    //Expands the farm by activating plots
    public void ExpandFarm()
    {
        int priceOfExpansion = EXPANSIONPRICE * _gameController.plotsize * _gameController.plotsize;

        if(_gameController.money < priceOfExpansion)
        {
            Debug.Log("Not enough cash!");
            return;
        }
        _gameController.removeMoney(priceOfExpansion);
        _gameController.CloseShop();
        _gameController.ExpandFarm();
        //update price of card in shop
        _expansionItemCardBuyPrice.GetComponent<Text>().text = "-" + (EXPANSIONPRICE * _gameController.plotsize * _gameController.plotsize).ToString() + "$";
    }
}
