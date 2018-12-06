using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour 
{
	private GameController _gameController;
	private GameObject _shopMenu;

	void Awake()
	{
		_shopMenu = Resources.FindObjectsOfTypeAll<Shop>()[0].gameObject;
	}

    private void Start()
    {
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