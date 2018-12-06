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
	private GameTime _timePlanted;

	void Awake() 
	{
		_gameController = FindObjectOfType<GameController>();
		_spriteR = gameObject.GetComponent<SpriteRenderer>();
		_shopItems = FindObjectOfType<ShopItems>();
		plant = _shopItems.allPlants[0];
	}
	
	// Update is called once per frame
	void Update()
	{
		
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
		}
	}

	void SetPlot(int _index)
	{
		plant = _shopItems.allPlants[_index];
		_spriteR.sprite = plant.levels[0];
		_gameController.currentPlot = null;
		_timePlanted = _gameController.gameTime;
	}
}
