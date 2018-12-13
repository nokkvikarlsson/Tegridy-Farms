using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Launder Shop")]
public class Launder : ScriptableObject
{
	public Launder()
	{
		type = "";
		durationDays = 0;
		price = 0;
		moneyLaunderCapacity = 0;
		unlockedAt = 0;
	}

	public string type;
	public double durationDays;
	public int price;
	public int moneyLaunderCapacity;
	public int unlockedAt;
}
