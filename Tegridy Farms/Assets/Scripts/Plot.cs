using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour 
{
	public Plant plant;
	public double growth; //growth from 0 to 1. 0 is newly planted. 1 is harvestable
	public GameObject sparklePrefab;
	public GameObject smokePrefab;
	private GameController _gameController;
    private SoundController _soundController;
    private SpriteRenderer _spriteR;
	private GameTime _timePlanted;
	private GameObject sparkle;
	private GameObject smoke;

	public bool buildingOn;
	public double growthBonus;

	void Awake() 
	{
		_gameController = FindObjectOfType<GameController>();
        _soundController = FindObjectOfType<SoundController>();
		_spriteR = gameObject.GetComponent<SpriteRenderer>();
		plant = _gameController.allPlants[0];
		sparkle = null;
		_timePlanted = new GameTime(0,0,0);
		growth = 0;
		buildingOn = false;
		growthBonus = 0;
	}

	void Start(){}
	
	// Update is called once per frame
	void Update()
	{
		if(plant.type != "Empty")
		{
			if(!plant.isBuilding)
			{
				PlantUpdate();
			}
			else
			{
				BuildingUpdate();
			}
		}
	}

	void PlantUpdate()
	{
		GameTime currentTime = new GameTime(_gameController.gameTime);
		GameTime hoursPassedgt = currentTime - _timePlanted;
		double hoursPassed = (24*hoursPassedgt.day) + hoursPassedgt.hour + ((double)hoursPassedgt.minute/60);
		growth = plant.growthrate * hoursPassed * (1+growthBonus);
		if(growth < 0.2)
		{
			_spriteR.sprite = _gameController.allPlants[plant.shopIndex].levels[0];
		}
		else if(growth < 0.4)
		{
			_spriteR.sprite = plant.levels[1];
		}
		else if(growth < 0.6)
		{
			_spriteR.sprite = plant.levels[2];
		}
		else if(growth < 0.8)
		{
			_spriteR.sprite = plant.levels[3];
		}
		else if(growth < 1)
		{
			_spriteR.sprite = plant.levels[4];
		}
		else if(growth >= 1)
		{
			//add flashing white circle sprite
			_spriteR.sprite = plant.levels[5];
			if(sparkle == null)
			{
				sparkle = (GameObject)Instantiate(sparklePrefab);
				sparkle.transform.position = gameObject.transform.position;
			}
		}
	}

	void BuildingUpdate()
	{
		if(buildingOn && (plant.type == "LSD Distillery" || plant.type == "Cocaine Refinery"))
		{
			GameTime currentTime = new GameTime(_gameController.gameTime);
			GameTime hoursPassedgt = currentTime - _timePlanted;
			double hoursPassed = (24*hoursPassedgt.day) + hoursPassedgt.hour + ((double)hoursPassedgt.minute/60);
			growth = plant.growthrate * hoursPassed;
			if(growth < 1)
			{

			}
			else if(growth >= 1)
			{
				//destroy smoke to show it is not working
				if(smoke != null)
				{
					Destroy(smoke);
					smoke = null;
				}

				if(plant.type == "LSD Distillery")
				{
					//add flashing white circle sprite
					if(sparkle == null)
					{
						sparkle = (GameObject)Instantiate(sparklePrefab);
						sparkle.transform.position = gameObject.transform.position;
					}
				}
				else if(plant.type == "Cocaine Refinery")
				{
					if(buildingOn)
					{
						buildingOn = false;
						_gameController.allPlants[6].sellvalue -= plant.sellvalue; 
						_gameController.allPlants[6].suspicion -= plant.suspicion;

						growth = 0;
						_timePlanted = new GameTime(0,0,0);
					}
				}
			}
		}
		else if(buildingOn && plant.type == "Fertilizer Dispenser")
		{
			GameTime currentTime = new GameTime(_gameController.gameTime);
			GameTime hoursPassedgt = currentTime - _timePlanted;
			double hoursPassed = (24*hoursPassedgt.day) + hoursPassedgt.hour + ((double)hoursPassedgt.minute/60);
			growth = plant.growthrate * hoursPassed;
			if((growth % 1.2) < 0.2)
			{
				_spriteR.sprite = _gameController.allPlants[plant.shopIndex].levels[0];
			}
			else if((growth % 1.2) < 0.4)
			{
				_spriteR.sprite = plant.levels[1];
			}
			else if((growth % 1.2) < 0.6)
			{
				_spriteR.sprite = plant.levels[2];
			}
			else if((growth % 1.2) < 0.8)
			{
				_spriteR.sprite = plant.levels[3];
			}
			else if((growth % 1.2) < 1)
			{
				_spriteR.sprite = plant.levels[4];
			}
			else if((growth % 1.2) < 1.2)
			{
				_spriteR.sprite = plant.levels[5];
			}
		}
	}

	void OnMouseDown()
	{
		if(plant.type == "Empty")
		{
			_gameController.SetCurrentPlot(gameObject);
			if(_gameController.currentItemIndex == 0)
			{
				_gameController.OpenShop();
			}
			else
			{
				SetPlot(_gameController.currentItemIndex);
                //Plays a random planting sound
                _soundController.PlayRandom(_soundController.plantingSounds);
			}
		}
		//IF PLANT
		else if(!plant.isBuilding)
		{
			PlantOnMouseDown();
		}
		//IF BUILDING
		else
		{
			BuildingOnMouseDown();
		}
	}

	void PlantOnMouseDown()
	{
		if(growth < 1)
		{
			//OPEN DELETE OPTION or DO NOTHING
		}
		else
		{
			//HARVEST
			_gameController.addMoney(plant.sellvalue);
			_gameController.addSuspicion(plant.sellvalue, plant.suspicion);
			//RESET PLANT
			plant = _gameController.allPlants[0];
			growth = 0;
			_spriteR.sprite = plant.levels[0];
			_timePlanted = new GameTime(0,0,0);
			//REMOVE SPARKLE
			Destroy(sparkle);
			sparkle = null;
		}
	}

	void BuildingOnMouseDown()
	{
		if(plant.type == "LSD Distillery")
		{
			if(!buildingOn)
			{
				buildingOn = true;
				_timePlanted = new GameTime(_gameController.gameTime);
				//Create cloud
				smoke = (GameObject)Instantiate(smokePrefab);
				smoke.transform.position = gameObject.transform.position;
			}
			else if(growth >= 1)
			{
				//HARVEST
				_gameController.addMoney(plant.sellvalue);
				_gameController.addSuspicion(plant.sellvalue, plant.suspicion);
				//RESET PLANT
				growth = 0;
				_timePlanted = new GameTime(0,0,0);
				buildingOn = false;
				//REMOVE SPARKLE
				Destroy(sparkle);
				sparkle = null;
			}
		}
		else if(plant.type == "Cocaine Refinery")
		{
			//TURN ON and Improve Cocaine
			if(!buildingOn)
			{
				buildingOn = true;
				_timePlanted = new GameTime(_gameController.gameTime);
				_gameController.allPlants[6].sellvalue += plant.sellvalue; 
				_gameController.allPlants[6].suspicion += plant.suspicion;
				//Adds cloud
				if(smoke == null)
				{
					Debug.Log("Make Smoke");
					smoke = (GameObject)Instantiate(smokePrefab);
					smoke.transform.position = gameObject.transform.position;
				}
			}
		}
		else if(plant.type == "Fertilizer Dispenser")
		{
			if(!buildingOn)
			{
				buildingOn = true;
				//Get plots and increase growthbonus by sellvalue
				_gameController.CheckFertilizer();
			}
		}
	}

	void SetPlot(int _index)
	{
		plant = _gameController.allPlants[_index];
		if(_gameController.money < plant.price)
		{
			Debug.Log("Not enough money");
			plant = _gameController.allPlants[0];
			return;
		}
		_gameController.removeMoney(plant.price);
		_spriteR.sprite = plant.levels[0];
		_gameController.currentPlot = null;
		GameTime currentTime = new GameTime(_gameController.gameTime);
		_timePlanted.day = currentTime.day;
		_timePlanted.hour = currentTime.hour;
		_timePlanted.minute = currentTime.minute;
	}
}
