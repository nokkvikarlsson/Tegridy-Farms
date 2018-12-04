using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Plant Item")]
public class Plant : ScriptableObject
{
	public Plant() {}

	public string type; //string of the type of plant
	public int price; //money to create a plot
	public double growthrate; //growth per game hour(5sek). from 0 to 1.
	public int sellvalue; //money earned when harvested
	public double suspicion; //suspicion that goes up per unlaundered dollar earned
	public int unlockedAt; //plotsize that plant is unlocked at
	public Sprite[] levels; //different sprites of growth from 0 to 1.
}
