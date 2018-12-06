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
