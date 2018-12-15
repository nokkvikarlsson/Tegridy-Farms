using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CropItemCard : MonoBehaviour
{
	public int shopItemIndex;
	private Text _itemCardTitleText;
	private GameController _gameController;
    private EventController _eventController;
    private SoundController _soundController;

	// Use this for initialization
	void Awake()
	{
		_gameController = FindObjectOfType<GameController>();
        _eventController = FindObjectOfType<EventController>();
        _soundController = FindObjectOfType<SoundController>();
		GameObject itemCardTitle = gameObject.transform.GetChild(0).gameObject;
		_itemCardTitleText = itemCardTitle.GetComponent<Text>();

	}
	
	void Start()
	{
		if(_gameController.allPlants[shopItemIndex].unlockedAt > _gameController.plotsize)
		{
			int unlockedAt = _gameController.allPlants[shopItemIndex].unlockedAt;
			_itemCardTitleText.text = "Need " + unlockedAt.ToString() + "x" + unlockedAt.ToString();

			Image itemCardImage = gameObject.GetComponent<Image>();
			itemCardImage.color = Color.gray;
		}
	}

	// Update is called once per frame
	void Update()
	{
		//if Unlocked change to white and name to actual name. Not "Need NxN"
		if(_gameController.allPlants[shopItemIndex].unlockedAt <= _gameController.plotsize)
		{
			_itemCardTitleText.text = _gameController.allPlants[shopItemIndex].type;

			Image itemCardImage = gameObject.GetComponent<Image>();
			itemCardImage.color = Color.white;
		}
	}

	public void OnMouseClick()
	{
		if(_gameController.allPlants[shopItemIndex].unlockedAt <= _gameController.plotsize)
		{
			_gameController.SetCurrentItem(shopItemIndex);
			_gameController.CloseShop();

            _eventController.beginTegridyIntroduction2 = true;
        }
		else
		{
			Debug.Log("Not Unlocked yet");
            _soundController.Play("CantAfford", _soundController.effectSounds);
        }
	}
}
