using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunderController : MonoBehaviour {

	public Launder[] allLaunders; //List of every laundering service
    public Launder currentLaunder; //The current Launder service. NULL if none is active
	public int currentMoneyLaundered;
	private GameTime _timeBought;
	
	private GameController _gameController;

	void Awake()
	{
		currentLaunder = null;
		currentMoneyLaundered = 0;
		_timeBought = new GameTime(0,0,0);
		_gameController = FindObjectOfType<GameController>();
	}

	// Update is called once per frame
	void Update()
	{
		if(currentLaunder != null)
		{
			GameTime hoursPassedgt = _gameController.gameTime - _timeBought;
			double daysPassed = hoursPassedgt.day + ((double)hoursPassedgt.hour / 60) + ((double)hoursPassedgt.minute / 3600);
			//CHECK IF Laundering is OVER
			if(daysPassed > currentLaunder.durationDays || currentMoneyLaundered > currentLaunder.moneyLaunderCapacity)
			{
				//Reset all values and launder to null
				currentLaunder = null;
				currentMoneyLaundered = 0;
				_timeBought = new GameTime(0,0,0);
			}
		}
	}

	public void SetCurrentLaunder(int index)
    {
        currentLaunder = allLaunders[index];
		_timeBought.day = _gameController.gameTime.day;
		_timeBought.hour = _gameController.gameTime.hour;
		_timeBought.minute = _gameController.gameTime.minute;
		currentMoneyLaundered = 0;
    }

	public void AddCurrentMoneyLaundered(int money)
	{
		currentMoneyLaundered += money;
	}
}
