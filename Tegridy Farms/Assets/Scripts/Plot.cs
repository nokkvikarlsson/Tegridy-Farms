using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour 
{
	public Plant plant;
	public double growth; //growth from 0 to 1. 0 is newly planted. 1 is harvestable
	private GameController _gameController;
	private SpriteRenderer _spriteR;
	private ShopItems _shopItems;
	public GameTime _timePlanted;

	void Awake() 
	{
		_gameController = FindObjectOfType<GameController>();
		_spriteR = gameObject.GetComponent<SpriteRenderer>();
		_shopItems = FindObjectOfType<ShopItems>();
	}

	void Start()
	{
		_timePlanted = new GameTime(0,0,0);
		growth = 0;
	}
	
	// Update is called once per frame
	void Update()
	{
		if(plant.type != "Empty")
		{
			GameTime currentTime = new GameTime(_gameController.gameTime);
			GameTime hoursPassedgt = currentTime - _timePlanted;
			double hoursPassed = (24*hoursPassedgt.day) + hoursPassedgt.hour + ((double)hoursPassedgt.minute/60);
			growth = plant.growthrate * hoursPassed;
			if(growth < 0.2)
			{
				_spriteR.sprite = _shopItems.allPlants[plant.shopIndex].levels[0];
			}
			else if(growth < 0.4)
			{
				_spriteR.sprite = plant.levels[1];
			}
			else if(growth < 0.6)
			{
				_spriteR.sprite = plant.levels[2];
			}
			else if(growth < 0.8)
			{
				_spriteR.sprite = plant.levels[3];
			}
			else if(growth < 1)
			{
				_spriteR.sprite = plant.levels[4];
			}
			else if(growth >= 1)
			{
				//add flashing white circle sprite
				_spriteR.sprite = plant.levels[5];
			}
		}
	}

	void OnMouseDown()
	{
		if(plant.name == "Empty")
		{
			_gameController.SetCurrentPlot(gameObject);
			if(_gameController.currentItemIndex == 0)
			{
				_gameController.OpenShop();
			}
			else
			{
				SetPlot(_gameController.currentItemIndex);
			}
		}
		else if(growth < 1)
		{
			//OPEN DELETE OPTION or DO NOTHING
		}
		else
		{
			//HARVEST
			_gameController.addMoney(plant.sellvalue);
			//add suspicion
			_gameController.addSuspicion(plant.sellvalue, plant.suspicion);
			plant = _shopItems.allPlants[0];
			growth = 0;
			_spriteR.sprite = plant.levels[0];
			_timePlanted = new GameTime(0,0,0);
		}
	}

	void SetPlot(int _index)
	{
		plant = _shopItems.allPlants[_index];
		if(_gameController.money < plant.price)
		{
			Debug.Log("Not enough money");
			plant = _shopItems.allPlants[0];
			return;
		}
		_gameController.removeMoney(plant.price);
		_spriteR.sprite = plant.levels[0];
		_gameController.currentPlot = null;
		GameTime currentTime = new GameTime(_gameController.gameTime);
		_timePlanted.day = currentTime.day;
		_timePlanted.hour = currentTime.hour;
		_timePlanted.minute = currentTime.minute;
	}
}
