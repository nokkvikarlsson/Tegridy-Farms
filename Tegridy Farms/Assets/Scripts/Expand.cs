using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expand : MonoBehaviour {


    //NokkviKilla Made these
    private bool _plotSize2x2;
    private bool _plotSize3x3;
    GameObject[] _plots;

    void Awake()
    {
        _plotSize2x2 = true;
        _plotSize3x3 = false;

    }

    // Use this for initialization
    void Start () 
    {
        _plots = GameObject.FindGameObjectsWithTag("plot");
    }
	
	// Update is called once per frame
	void Update () 
    {
		
	}


    //Expands the farm by activating plots
    public void ExpandFarm()
    {
        if(_plotSize2x2)
        {

            //3x3 plots
            _plots[2].GetComponent<SpriteRenderer>().enabled = true;
            _plots[2].GetComponent<BoxCollider2D>().enabled = true;

            _plots[6].GetComponent<SpriteRenderer>().enabled = true;
            _plots[6].GetComponent<BoxCollider2D>().enabled = true;

            _plots[8].GetComponent<SpriteRenderer>().enabled = true;
            _plots[8].GetComponent<BoxCollider2D>().enabled = true;

            _plots[9].GetComponent<SpriteRenderer>().enabled = true;
            _plots[9].GetComponent<BoxCollider2D>().enabled = true;

            _plots[10].GetComponent<SpriteRenderer>().enabled = true;
            _plots[10].GetComponent<BoxCollider2D>().enabled = true;

            _plotSize2x2 = false;
            _plotSize3x3 = true;
        }
        else if(_plotSize3x3)
        {

            _plots[3].GetComponent<SpriteRenderer>().enabled = true;
            _plots[3].GetComponent<BoxCollider2D>().enabled = true;

            _plots[7].GetComponent<SpriteRenderer>().enabled = true;
            _plots[7].GetComponent<BoxCollider2D>().enabled = true;

            _plots[11].GetComponent<SpriteRenderer>().enabled = true;
            _plots[11].GetComponent<BoxCollider2D>().enabled = true;

            _plots[12].GetComponent<SpriteRenderer>().enabled = true;
            _plots[12].GetComponent<BoxCollider2D>().enabled = true;

            _plots[13].GetComponent<SpriteRenderer>().enabled = true;
            _plots[13].GetComponent<BoxCollider2D>().enabled = true;

            _plots[14].GetComponent<SpriteRenderer>().enabled = true;
            _plots[14].GetComponent<BoxCollider2D>().enabled = true;

            _plots[15].GetComponent<SpriteRenderer>().enabled = true;
            _plots[15].GetComponent<BoxCollider2D>().enabled = true;

            _plotSize3x3 = false;
        }
    }
}
