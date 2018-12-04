using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour {

	public string type; //string of type of plant or building. i.e. "Cannabis"
	public double growth; //double ranging from 0 to 1, representing growth progress
						  // 0 is newly planted, 1 is ready for harvest.
	

	private GameController _gameController;

	// Use this for initialization
	void Start() {
		
	}

	void Awake() {
		_gameController = FindObjectOfType<GameController>();
	}
	
	// Update is called once per frame
	void Update() {
		
	}
}
