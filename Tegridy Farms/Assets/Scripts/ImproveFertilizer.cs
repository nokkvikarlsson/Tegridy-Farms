using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImproveFertilizer : MonoBehaviour {

	GameController _gameController;
	GameObject _improveItemCardTitle;
	Image _improveItemCardImage;
	bool _bought;

	void Awake() {
		_gameController = FindObjectOfType<GameController>();
		_bought = false;

		_improveItemCardTitle = gameObject.transform.GetChild(0).gameObject;
		_improveItemCardImage = gameObject.GetComponent<Image>();

		_improveItemCardTitle.GetComponent<Text>().text = "Need 5x5";
		_improveItemCardImage.color = Color.gray;
	}

	void Update()
	{
		if(!_bought)
		{
			if(_gameController.plotsize >= 5)
			{
				if(_gameController.money < 750)
				{
					_improveItemCardImage.color = Color.gray;
				}
				else
				{
					_improveItemCardImage.color = Color.white;
				}
			}
			else
			{
				_improveItemCardImage.color = Color.gray;
			}
		}
	}

	public void Improve()
	{
		if(_bought)
		{
			Debug.Log("Already Purchased");
			return;
		}
		if(_gameController.plotsize < 5)
		{
			Debug.Log("Not unlocked yet");
			return;
		}
		if(_gameController.money < 750)
		{
			Debug.Log("Not enough cash");
			return;
		}
		_gameController.removeMoney(750);
		_gameController.allPlants[9].sellvalue = 50;
		_bought = true;
		_improveItemCardTitle.GetComponent<Text>().text = "Purchased";
		_improveItemCardImage.color = Color.gray;
		_gameController.CloseShop();
	}
}
