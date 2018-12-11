using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop Item")]
public class Plant : ScriptableObject
{
	public Plant() {}
	public Plant(Plant copy)
	{
		type = copy.type;
		price = copy.price;
		growthrate = copy.growthrate;
		sellvalue = copy.sellvalue;
		suspicion = copy.suspicion;
		unlockedAt = copy.unlockedAt;
		shopIndex = copy.shopIndex;
		levels = copy.levels;
	}

	public string type; //string of the type of plant
	public int price; //money to create a plot
	public double growthrate; //growth per game hour(5sek). from 0 to 1.
	public int sellvalue; //money earned when harvested
	public double suspicion; //suspicion that goes up per unlaundered dollar earned
	public int unlockedAt; //plotsize that plant is unlocked at
	public int shopIndex; //index in shop items
	public Sprite[] levels; //different sprites of growth from 0 to 1.
	public bool isBuilding; //0 if plant, 1 if building
}
