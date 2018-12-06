using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCard : MonoBehaviour
{
	public GameObject itemCard;
	public int shopItemIndex;
	private GameController _gameController;

	// Use this for initialization
	void Awake()
	{
		itemCard = gameObject;

		_gameController = FindObjectOfType<GameController>();
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}

	void OnMouseClick()
	{
		_gameController.SetCurrentItem(shopItemIndex);
	}
}
