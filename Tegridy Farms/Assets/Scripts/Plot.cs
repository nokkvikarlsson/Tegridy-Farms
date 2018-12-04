using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour 
{
	public Plant plant;
	public double growth; //growth from 0 to 1. 0 is newly planted. 1 is harvestable
	//private GameController _gameController;
	private SpriteRenderer _spriteR;
	private ShopItems _shopItems;

	void Awake() 
	{
		//_gameController = FindObjectOfType<GameController>();
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
		plant = _shopItems.allPlants[1];
		_spriteR.sprite = plant.levels[0];
	}
}
