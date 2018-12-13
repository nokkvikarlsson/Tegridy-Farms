using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunderItemCard : MonoBehaviour 
{
	public int launderItemIndex;
	private Text _itemCardTitleText;
	private GameController _gameController;
	private LaunderController _launderController;

	void Awake()
	{
		_gameController = FindObjectOfType<GameController>();
		_launderController = FindObjectOfType<LaunderController>();
		GameObject itemCardTitle = gameObject.transform.GetChild(0).gameObject;
		_itemCardTitleText = itemCardTitle.GetComponent<Text>();
	}

	// Use this for initialization
	void Start() 
	{
		if(_launderController.allLaunders[launderItemIndex].unlockedAt > _gameController.plotsize)
		{
			int unlockedAt = _launderController.allLaunders[launderItemIndex].unlockedAt;
			_itemCardTitleText.text = "Need " + unlockedAt.ToString() + "x" + unlockedAt.ToString();

			Image itemCardImage = gameObject.GetComponent<Image>();
			itemCardImage.color = Color.gray;
		}
	}
	
	// Update is called once per frame
	void Update() 
	{
		if(_launderController.allLaunders[launderItemIndex].unlockedAt <= _gameController.plotsize)
		{
			_itemCardTitleText.text = _launderController.allLaunders[launderItemIndex].type;

			Image itemCardImage = gameObject.GetComponent<Image>();
			itemCardImage.color = Color.white;
		}
	}

	public void OnMouseClick()
	{
		if(_launderController.allLaunders[launderItemIndex].unlockedAt <= _gameController.plotsize
		&& _launderController.currentLaunder == null)
		{
			_launderController.SetCurrentLaunder(launderItemIndex);
			_gameController.CloseShop();
		}
		else
		{
			Debug.Log("Not Unlocked yet");
		}
	}
}
