using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCard : MonoBehaviour
{
	public GameObject itemCard;
	public int shopItemIndex;
	public GameController _gameController;

	// Use this for initialization
	void Awake()
	{
		_gameController = FindObjectOfType<GameController>();
		itemCard = gameObject;
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
		_gameController.SetCurrentItem(shopItemIndex);
		_gameController.CloseShop();
	}
}
