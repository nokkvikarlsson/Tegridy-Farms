using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
	public int plotsize; //starts at 2x2
	public GameObject plotPrefab;
	public int money; //starts at 100
	public double suspicion; //0-100
	public GameTime gameTime; //time in the game: DAY 1 00:00
	public int timmy;

	public class GameTime
	{
		public int day;
		public int hour;
		public int minute;
		public GameTime() 
		{
			this.day = 1;
			this.hour = 0;
			this.minute = 0;
		}
		public GameTime(int _day, int _hour, int _minute) 
		{
			this.day = _day;
			this.hour = _hour;
			this.minute = _minute;
		}
		public void AddOneMinute()
		{
			this.minute += 1;
			if(this.minute >= 60){
				this.hour += 1;
				this.minute = 0;
			}
			if(this.hour >= 24){
				this.day +=1 ;
				this.hour = 0;
			}
		}
	}

	// Use this for initialization
	void Start()
	{

	}

	void Awake()
	{
		//initalize plotsize at 2x2
		plotsize = 2;
		//Create starting plots at (0,0) (1,0) (0,-1), (1,-1)
		for(int x = 0; x < 2; x++)
		{
			for(int y = 0; y > -2; y--)
			{
				GameObject plot = (GameObject)Instantiate(plotPrefab);
				plot.transform.position = new Vector3(x, y);
			}
		}
		//initialize money at 100
		money = 100;
		//initialize suspicion at 0
		suspicion = 0;
		//initialize gametime
		gameTime = new GameTime();
	}
	
	// Update is called once per frame
	void Update()
	{
		timmy = gameTime.minute;
	}

	void StartGameTime()
	{
		StartCoroutine(TimeOneTwelfthSecond());
	}

	IEnumerator TimeOneTwelfthSecond()
	{
		while(true)
		{
			gameTime.AddOneMinute();
			yield return new WaitForSeconds(1f/12);
		}
	}
}
