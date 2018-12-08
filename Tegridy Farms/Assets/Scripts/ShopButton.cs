using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour 
{
	private GameController _gameController;

    void Awake()
	{
        _gameController = FindObjectOfType<GameController>();
	}

    private void Start()
    {
        
    }

    public void OpenShop()
	{
		_gameController.OpenShop();
    }

	public void CloseShop()
	{
		_gameController.CloseShop();
    }
}