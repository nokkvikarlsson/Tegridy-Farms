using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour 
{
	private GameController _gameController;
	private GameObject _shopMenu;
	private RectTransform _plantTab;
    private GameObject[] _plots;

    void Awake()
	{
		_shopMenu = Resources.FindObjectsOfTypeAll<Shop>()[0].gameObject;
		GameObject _plantsTabPane = _shopMenu.transform.GetChild(0).GetChild(0).gameObject;
		_plantTab = _plantsTabPane.GetComponent<RectTransform>();
	}

    private void Start()
    {
        _plots = GameObject.FindGameObjectsWithTag("plot");
    }

    public void OpenShop()
	{
		_shopMenu.SetActive(true);
		_plantTab.SetAsLastSibling();

        for (int i = 0; i < _plots.Length; i++)
        {
            _plots[i].GetComponent<BoxCollider2D>().enabled = false;
        }
    }

	public void CloseShop()
	{
		_shopMenu.SetActive(false);

        for (int i = 0; i < _plots.Length; i++)
        {
            _plots[i].GetComponent<BoxCollider2D>().enabled = true;
        }

    }
}