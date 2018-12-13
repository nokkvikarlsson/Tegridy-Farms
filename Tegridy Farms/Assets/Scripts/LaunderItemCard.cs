using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunderItemCard : MonoBehaviour 
{
	public int launderItemIndex;
	private Text _itemCardTitleText;
	private Image _itemCardImage;
	private GameController _gameController;
	private LaunderController _launderController;
	private bool _unlocked;

	void Awake()
	{
		_gameController = FindObjectOfType<GameController>();
		_launderController = FindObjectOfType<LaunderController>();
		GameObject itemCardTitle = gameObject.transform.GetChild(0).gameObject;
		_itemCardTitleText = itemCardTitle.GetComponent<Text>();
		_itemCardImage = gameObject.GetComponent<Image>();
		_unlocked = true;
	}

	// Use this for initialization
	void Start() 
	{
		if(_launderController.allLaunders[launderItemIndex].unlockedAt > _gameController.plotsize)
		{
			int unlockedAt = _launderController.allLaunders[launderItemIndex].unlockedAt;
			_itemCardTitleText.text = "Need " + unlockedAt.ToString() + "x" + unlockedAt.ToString();

			_itemCardImage.color = Color.gray;
			_unlocked = false;
		}
	}
	
	// Update is called once per frame
	void Update() 
	{
		if(!_unlocked && _launderController.allLaunders[launderItemIndex].unlockedAt <= _gameController.plotsize)
		{
			_itemCardTitleText.text = _launderController.allLaunders[launderItemIndex].type;

			Image itemCardImage = gameObject.GetComponent<Image>();
			itemCardImage.color = Color.white;
			_unlocked = true;
		}

		if(_unlocked)
		{
			if(_launderController.currentLaunder != null || _gameController.money < _launderController.allLaunders[launderItemIndex].price)
			{
				_itemCardImage.color = Color.gray;
			}
			else
			{
				_itemCardImage.color = Color.white;
			}
		}
	}

	public void OnMouseClick()
	{
		if(_gameController.money < _launderController.allLaunders[launderItemIndex].price)
		{
			Debug.Log("Not enouh cash");
			return;
		}

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
