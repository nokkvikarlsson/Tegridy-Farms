using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public GameTime(GameTime copy)
	{
		this.day = copy.day;
		this.hour = copy.hour;
		this.minute = copy.minute;
	}

	public static GameTime operator- (GameTime left, GameTime right)
	{
		GameTime output = new GameTime(left.day-right.day, left.hour-right.hour, left.minute-right.minute);
		if(output.minute < 0)
		{
			output.minute += 60;
			output.hour -= 1;
		}
		if(output.hour < 0)
		{
			output.hour += 24;
			output.day -= 1;
		}
		return output;
	}

	public void AddOneMinute()
	{
		this.minute += 1;
		if(this.minute >= 60)
		{
			this.hour += 1;
			this.minute = 0;
		}
		if(this.hour >= 24)
		{
			this.day +=1 ;
			this.hour = 0;
		}
	}
}
