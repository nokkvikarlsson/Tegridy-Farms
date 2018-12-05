using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour 
{
	private GameObject _shopMenu;

	void Awake()
	{
		_shopMenu = Resources.FindObjectsOfTypeAll<Shop>()[0].gameObject;
		_shopMenu.SetActive(false);
	}

	public void OpenShop()
	{
		_shopMenu.SetActive(true);
	}

	public void CloseShop()
	{
		_shopMenu.SetActive(false);
	}
}