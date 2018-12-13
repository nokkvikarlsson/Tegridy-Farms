using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousePlot : MonoBehaviour {

	private GameController _gameController;

	// Use this for initialization
	void Awake ()
	{
		_gameController = FindObjectOfType<GameController>();
	}
	
	void OnMouseDown()
	{
		_gameController.OpenShop();
	}
}
