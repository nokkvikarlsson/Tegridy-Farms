using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCard : MonoBehaviour
{
	public GameObject itemCard;
	public int shopItemIndex;
	private GameController _gameController;
	private ShopItems _shopItems;

	// Use this for initialization
	void Awake()
	{
		_gameController = FindObjectOfType<GameController>();
		itemCard = gameObject;
		_shopItems = FindObjectOfType<ShopItems>();
	}
	
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
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
