using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plants : MonoBehaviour {

	public class Plant {
		public Plant(string _type, int _price, double _growthrate, int _sellvalue, double _suspicion){
			this.type = _type;
			this.price = _price;
			this.growthrate = _growthrate; //progress made from 0 to 1, per game hour
			this.sellvalue = _sellvalue;
			this.suspicion = _suspicion; //suspicion added for every unlaundered dollar earned. suspicion is from 0 to 1.
		}
		public string type;
		public int price;
		public double growthrate;
		public int sellvalue;
		public double suspicion;
	}

	public List<Plant> AllPlants;

	// Use this for initialization
	void Start() {
		AllPlants.Add(new Plant("Cannabis", 30, 0.25, 50, 0.05));
		AllPlants.Add(new Plant("Tobacco", 10, 0.2, 15, 0));
		AllPlants.Add(new Plant("Hash", 50, 0.18, 80, 0.05));
	}
	
	// Update is called once per frame
	void Update() {
		
	}
}
