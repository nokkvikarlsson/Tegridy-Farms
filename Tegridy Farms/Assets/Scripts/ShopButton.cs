using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour 
{
	private GameController _gameController;
	private GameObject _shopMenu;
	private RectTransform _plantTab;

	void Awake()
	{
		_shopMenu = Resources.FindObjectsOfTypeAll<Shop>()[0].gameObject;
		GameObject _plantsTabPane = _shopMenu.transform.GetChild(0).GetChild(3).gameObject;
		_plantTab = _plantsTabPane.GetComponent<RectTransform>();
	}

    public void OpenShop()
	{
		_shopMenu.SetActive(true);
		_plantTab.SetAsLastSibling();
	}

	public void CloseShop()
	{
		_shopMenu.SetActive(false);
	}
}