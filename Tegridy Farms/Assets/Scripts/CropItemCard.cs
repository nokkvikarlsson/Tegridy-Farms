﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CropItemCard : MonoBehaviour
{
	public int shopItemIndex;
	private Text _itemCardTitleText;
	private GameController _gameController;
	private ShopItems _shopItems;

	// Use this for initialization
	void Awake()
	{
		_gameController = FindObjectOfType<GameController>();
		_shopItems = FindObjectOfType<ShopItems>();
		GameObject itemCardTitle = gameObject.transform.GetChild(0).gameObject;
		_itemCardTitleText = itemCardTitle.GetComponent<Text>();
	}
	
	void Start()
	{
		if(_shopItems.allPlants[shopItemIndex].unlockedAt > _gameController.plotsize)
		{
			int unlockedAt = _shopItems.allPlants[shopItemIndex].unlockedAt;
			_itemCardTitleText.text = "Need " + unlockedAt.ToString() + "x" + unlockedAt.ToString();

			Image itemCardImage = gameObject.GetComponent<Image>();
			itemCardImage.color = Color.gray;
		}
	}

	// Update is called once per frame
	void Update()
	{
		//if Unlocked change to white and name to actual name. Not "Need NxN"
		if(_shopItems.allPlants[shopItemIndex].unlockedAt <= _gameController.plotsize)
		{
			_itemCardTitleText.text = _shopItems.allPlants[shopItemIndex].type;

			Image itemCardImage = gameObject.GetComponent<Image>();
			itemCardImage.color = Color.white;
		}
	}

	public void OnMouseClick()
	{
		if(_shopItems.allPlants[shopItemIndex].unlockedAt <= _gameController.plotsize)
		{
			_gameController.SetCurrentItem(shopItemIndex);
			_gameController.CloseShop();
		}
		else
		{
			Debug.Log("Not Unlocked yet");
		}
	}
}