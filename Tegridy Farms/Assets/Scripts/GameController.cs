using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
	public int plotsize;
	public GameObject plotPrefab;

	// Use this for initialization
	void Start()
	{
		
	}

	void Awake()
	{
		//Create starting plots at (0,0) (1,0) (0,-1), (1,-1)
		for(int x = 0; x < 2; x++)
		{
			for(int y = 0; y > -2; y--)
			{
				GameObject plot = (GameObject)Instantiate(plotPrefab);
				plot.transform.position = new Vector3(x, y);
			}
		}
	}
	
	// Update is called once per frame
	void Update() {
		
	}
}
