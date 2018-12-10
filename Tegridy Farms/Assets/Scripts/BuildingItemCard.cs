using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingItemCard : MonoBehaviour
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
	
	// Update is called once per frame
	void Update()
	{
		
	}

	public void OnMouseClick()
	{

	}
}
