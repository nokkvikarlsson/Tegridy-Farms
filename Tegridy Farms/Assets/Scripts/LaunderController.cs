using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunderController : MonoBehaviour {

	public Launder[] allLaunders; //List of every laundering service
    public Launder currentLaunder; //The current Launder service. NULL if none is active
	public int currentMoneyLaundered;
	private GameTime _timeBought;
	private GameTime _durationGT;
	private GameController _gameController;
	private GameObject _launderUI;
	private Text _launderUIMoney;
	private Text _launderUITime;

	void Awake()
	{
		currentLaunder = null;
		currentMoneyLaundered = 0;
		_timeBought = new GameTime(0,0,0);
		_durationGT = new GameTime(0,0,0);
		_gameController = FindObjectOfType<GameController>();
		_launderUI = GameObject.Find("/UI/LaunderInformation");
		_launderUIMoney = GameObject.Find("/UI/LaunderInformation/CapacityValue").GetComponent<Text>();
		_launderUITime = GameObject.Find("/UI/LaunderInformation/TimeValue").GetComponent<Text>();
	}

	void Start()
	{
		_launderUI.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		if(currentLaunder != null)
		{
			GameTime timePassedgt = _gameController.gameTime - _timeBought;
			double hoursPassed = 24*(double)timePassedgt.day + ((double)timePassedgt.hour) + ((double)timePassedgt.minute / 60);
			//Change UI
			_launderUIMoney.text = (currentLaunder.moneyLaunderCapacity - currentMoneyLaundered).ToString() + "$";
			//ADD DAYS/TIME LEFT
			GameTime timeLeft = _durationGT - timePassedgt;
			_launderUITime.text = "";
			if(timeLeft.hour < 10){_launderUITime.text += "0";}
			_launderUITime.text += timeLeft.hour.ToString() + ":";
			if(timeLeft.minute < 10){_launderUITime.text += "0";}
			_launderUITime.text += timeLeft.minute.ToString();
			//CHECK IF Laundering is OVER
			Debug.Log(hoursPassed);
			Debug.Log(currentLaunder.durationDays);
			if(hoursPassed > currentLaunder.durationDays || currentMoneyLaundered >= currentLaunder.moneyLaunderCapacity)
			{
				//Reset all values and launder to null
				currentLaunder = null;
				currentMoneyLaundered = 0;
				_timeBought = new GameTime(0,0,0);
				_launderUI.SetActive(false);
			}
		}
	}

	public void SetCurrentLaunder(int index)
    {
		_launderUI.SetActive(true);
        currentLaunder = allLaunders[index];
		_gameController.removeMoney(currentLaunder.price);

		_timeBought = new GameTime(_gameController.gameTime);
		_durationGT = new GameTime(0,0,0);
		_durationGT.hour = (int)currentLaunder.durationDays;
		currentMoneyLaundered = 0;
    }

	public void AddCurrentMoneyLaundered(int money)
	{
		currentMoneyLaundered += money;
	}
}
