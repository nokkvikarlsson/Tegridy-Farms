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
    private int EXPANSIONPRICE = 20;
    private GameObject _mainCamera;
	private Image _expansionItemCardImage;

    void Awake()
    {
        _gameController = FindObjectOfType<GameController>();
        _soundController = FindObjectOfType<SoundController>();
        _eventController = FindObjectOfType<EventController>();

		_expansionItemCardBuyPrice = gameObject.transform.GetChild(2).gameObject;
		_expansionItemCardTitle = gameObject.transform.GetChild(0).gameObject;
		_expansionItemCardImage = gameObject.GetComponent<Image>();

        _mainCamera = GameObject.Find("/Main Camera");
    }

    // Use this for initialization
    void Start() 
    {

    }
	
	// Update is called once per frame
	void Update () 
    {
		if(_gameController.plotsize == 8 || _gameController.money < _gameController.plotsize * _gameController.plotsize * _gameController.plotsize * EXPANSIONPRICE)
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
		
        int priceOfExpansion = EXPANSIONPRICE * _gameController.plotsize * _gameController.plotsize * _gameController.plotsize * _gameController.plotsize;

        if(_gameController.money < priceOfExpansion)
        {
            Debug.Log("Not enough cash!");
            return;
        }
        _soundController.Play("Expand", _soundController.effectSounds);
        _gameController.removeMoney(priceOfExpansion);
        _gameController.CloseShop();
        ExpandFarmPlots();
        AddSurroundingPlot();
		AdjustCamera();
		//Add fertilizer on new plots
		_gameController.CheckFertilizer();
        //update price of card in shop
        _expansionItemCardBuyPrice.GetComponent<Text>().text = "-" + (EXPANSIONPRICE * _gameController.plotsize * _gameController.plotsize * _gameController.plotsize * _gameController.plotsize).ToString() + "$";

		if(_gameController.plotsize == 8) 
		{
			_expansionItemCardTitle.GetComponent<Text>().text = "MAXED OUT";

			Image itemCardImage = gameObject.GetComponent<Image>();
			itemCardImage.color = Color.gray;
			_expansionItemCardBuyPrice.GetComponent<Text>().text = "";
		}

        //Play laundererdialogue when the player has expanded his farm for the first time
        if (_gameController.plotsize == 3)
        {
            _eventController.DisplayDialogueLaunderer("Nice business you got there, farmer! Have you considered laundering your income?");
            StartCoroutine(waitToStartLaundererIntroduction2());
        }
    }

    IEnumerator waitToStartLaundererIntroduction2()
    {
        yield return new WaitForSeconds(10);
        _gameController.playLaunderer = true;
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

	void AdjustCamera()
	{
		_mainCamera.transform.position += new Vector3(0.25f, -0.5f);
		Camera mainCameraComp = _mainCamera.GetComponent<Camera>();
		mainCameraComp.orthographicSize += 0.19f;
	}

}
