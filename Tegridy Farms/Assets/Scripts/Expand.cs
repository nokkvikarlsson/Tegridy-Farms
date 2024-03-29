﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Expand : MonoBehaviour {

    public GameObject plotPrefab;
    //Surrounding Plot Prefabs
	public GameObject surroundingPlot;
	public GameObject upperPlotPrefab;
	public GameObject upperLeftPlotPrefab;
	public GameObject upperRightPlotPrefab;
	public GameObject lowerPlotPrefab;
	public GameObject lowerRightPlotPrefab;
	public GameObject lowerLeftPlotPrefab;
	public GameObject leftPlotPrefab;
	public GameObject rightPlotPrefab;

    //For the sound
    private SoundController _soundController;

    private EventController _eventController;
    private GameController _gameController;
    private GameObject _expansionItemCardBuyPrice;

	private GameObject _expansionItemCardTitle;
    private int EXPANSIONPRICE = 15;
	private Image _expansionItemCardImage;

    void Awake()
    {
        _gameController = FindObjectOfType<GameController>();
        _soundController = FindObjectOfType<SoundController>();
        _eventController = FindObjectOfType<EventController>();

		_expansionItemCardBuyPrice = gameObject.transform.GetChild(2).gameObject;
		_expansionItemCardTitle = gameObject.transform.GetChild(0).gameObject;
		_expansionItemCardImage = gameObject.GetComponent<Image>();
    }

    // Use this for initialization
    void Start() 
    {

    }
	
	// Update is called once per frame
	void Update () 
    {
		if(_gameController.plotsize == 8 || _gameController.money < CalculatePrice())
		{
			_expansionItemCardImage.color = Color.gray;
		}
		else
		{
			_expansionItemCardImage.color = Color.white;
		}
	}


    //Expands the farm by activating plots
    public void ExpandFarm()
    {
		if(_gameController.plotsize == 8) {return;}
		
        int priceOfExpansion = CalculatePrice();

        if(_gameController.money < priceOfExpansion)
        {
            Debug.Log("Not enough cash!");
            _soundController.Play("CantAfford", _soundController.effectSounds);
            return;
        }
        _soundController.Play("Expand", _soundController.effectSounds);
        _gameController.removeMoney(priceOfExpansion);
        ExpandFarmPlots();
        AddSurroundingPlot();
		_gameController.AdjustCamera();
		//Increase Rent
		_gameController.IncreaseRent();
		//Add fertilizer on new plots
		_gameController.CheckFertilizer();
        //update price of card in shop
        _expansionItemCardBuyPrice.GetComponent<Text>().text = "-" + (CalculatePrice()).ToString() + "$";

		if(_gameController.plotsize == 8) 
		{
			_expansionItemCardTitle.GetComponent<Text>().text = "MAXED OUT";

			Image itemCardImage = gameObject.GetComponent<Image>();
			itemCardImage.color = Color.gray;
			_expansionItemCardBuyPrice.GetComponent<Text>().text = "";
		}
		_gameController.CloseShop();
    }

    public void ExpandFarmPlots()
	{
        int plotsize = _gameController.plotsize;
		for(int i = 0; i < plotsize; i++)
		{
			GameObject plot = (GameObject)Instantiate(plotPrefab);
			plot.transform.position = new Vector3(plotsize, -i);
		}
		for(int i = 0; i < plotsize; i++)
		{
			GameObject plot = (GameObject)Instantiate(plotPrefab);
			plot.transform.position = new Vector3(i, -plotsize);
		}
		GameObject cornerplot = (GameObject)Instantiate(plotPrefab);
		cornerplot.transform.position = new Vector3(plotsize, -plotsize);
		_gameController.plotsize++;
		_gameController.plots = GameObject.FindGameObjectsWithTag("plot");
	}

	void AddSurroundingPlot()
	{
        int plotsize = _gameController.plotsize;
		//move lowerleft down
		GameObject lowerleft = GameObject.FindGameObjectsWithTag("LowerLeftPlot")[0];
		lowerleft.transform.position += Vector3.down;
		//move upperright right
		GameObject upperright = GameObject.FindGameObjectsWithTag("UpperRightPlot")[0];
		upperright.transform.position += Vector3.right;
		//move lowerright down-right
		GameObject lowerright = GameObject.FindGameObjectsWithTag("LowerRightPlot")[0];
		lowerright.transform.position += (Vector3.right + Vector3.down);
		//add new left and upper plot
		GameObject left = (GameObject)Instantiate(leftPlotPrefab);
		left.transform.position = new Vector3(-1, -1*(plotsize-1));
		GameObject upper = (GameObject)Instantiate(upperPlotPrefab);
		upper.transform.position = new Vector3(plotsize-1, 1);
		//move lower plots down and add one
		GameObject[] lower = GameObject.FindGameObjectsWithTag("LowerPlot");
		for(int i = 0; i < lower.Length; i++)
		{
			lower[i].transform.position += Vector3.down;
		}
		GameObject newLower = (GameObject)Instantiate(lowerPlotPrefab);
		newLower.transform.position = new Vector3(plotsize-1, -plotsize);
		//move right plots right and add one
		GameObject[] right = GameObject.FindGameObjectsWithTag("RightPlot");
		for(int i = 0; i < lower.Length; i++)
		{
			right[i].transform.position += Vector3.right;
		}
		GameObject newRight = (GameObject)Instantiate(rightPlotPrefab);
		newRight.transform.position = new Vector3(plotsize, -(plotsize-1));
	}

	int CalculatePrice()
	{
		return (int)System.Math.Floor((double)(EXPANSIONPRICE * System.Math.Pow((double)_gameController.plotsize, (double)4) / 100)) * 100;
	}
}
