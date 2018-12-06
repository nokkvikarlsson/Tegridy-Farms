using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expand : MonoBehaviour {


    //NokkviKilla Made these
    private bool _plotSize2x2;
    private bool _plotSize3x3;
    private GameObject[] _plots;
    private GameObject _field2x2;
    private GameObject _field3x3;
    private GameObject _field4x4;

    void Awake()
    {
        _plotSize2x2 = true;
        _plotSize3x3 = false;

    }

    // Use this for initialization
    void Start () 
    {
        _plots = GameObject.FindGameObjectsWithTag("plot");
        _field2x2 = GameObject.Find("Tilemap_field_2x2");
        _field3x3 = GameObject.Find("Tilemap_field_3x3");
        _field4x4 = GameObject.Find("Tilemap_field_4x4");
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

            _field2x2.SetActive(false);
            _field3x3.SetActive(true);

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

            _field3x3.SetActive(false);
            _field4x4.SetActive(true);

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
