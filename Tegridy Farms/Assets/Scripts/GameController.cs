using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour 
{
	//PUBLIC:
	public int plotsize; //starts at 2x2
	public GameObject plotPrefab;
	public int money; //starts at 100
	public double suspicion; //0-100
	public GameTime gameTime; //time in the game: DAY 1 00:00
	public int currentItemIndex; //int of index in shopitem array
	public GameObject currentPlot; //selected plot on which to plant
	public bool isShopOpen; //is the shop menu active?
	public Sprite[] itemSprites;

	//PRIVATE:
	private GameObject _shopButton;
	private GameObject _shopMenu;
	private SpriteRenderer _currentItemImageSpriteRenderer;
	private Text _moneyCounterText;
	private Text _timeCounterText;
	private Text _dayCounterText;
	private Slider _barSlider;
	private bool _gameOver;

	// Use this for initialization
	void Start()
	{

	}

	void Awake()
	{
		//initalize plotsize at 2x2
		plotsize = 2;
		//Create starting plots at (0,0) (1,0) (0,-1), (1,-1)
		for(int x = 0; x < 2; x++)
		{
			for(int y = 0; y > -2; y--)
			{
				GameObject plot = (GameObject)Instantiate(plotPrefab);
				plot.transform.position = new Vector3(x, y);
			}
		}
		//initialize variables
		money = 100;
		suspicion = 0;
		gameTime = new GameTime();
		currentItemIndex = 0;
		currentPlot = null;
		isShopOpen = false;
		_gameOver = false;
		//initialize gameobjects
		GameObject _moneyCounter = GameObject.Find("/UI/TopPanel/MoneyCounter");
		_moneyCounterText = _moneyCounter.GetComponent<Text>();
		GameObject _timeCounter = GameObject.Find("/UI/TopPanel/TimeCounter");
		_timeCounterText = _timeCounter.GetComponent<Text>();
		GameObject _dayCounter = GameObject.Find("/UI/TopPanel/DayCounter");
		_dayCounterText = _dayCounter.GetComponent<Text>();
		GameObject _bar = GameObject.Find("/UI/TopPanel/Suspicion bar/Bar");
		_barSlider = _bar.GetComponent<Slider>();
		_shopButton = GameObject.Find("/UI/TopPanel/Button");
		_shopMenu = Resources.FindObjectsOfTypeAll<Shop>()[0].gameObject;
		GameObject _currentItemImage = GameObject.Find("/CurrentItem/CurrentItemImage");
		_currentItemImageSpriteRenderer = _currentItemImage.GetComponent<SpriteRenderer>();
		//Start Game Time
		StartGameTime();
	}
	
	// Update is called once per frame
	void Update()
	{
		/*=================
		Update Values of UI
		=================*/
		//Update UI MoneyCounter to match
		_moneyCounterText.text = "Money: " + money.ToString() + "$";
		//Update UI DayCounter to match
		_dayCounterText.text = "Day " + gameTime.day.ToString();
		//Update UI TimeCounter to match
		string timetext = "";
		if(gameTime.hour < 10) {timetext += "0";}
		timetext += gameTime.hour.ToString();
		timetext += ":";
		if(gameTime.minute < 10) {timetext += "0";}
		timetext += gameTime.minute.ToString();
		_timeCounterText.text = timetext;
		//Update UI BarSlider to match
		_barSlider.value = (float)suspicion;
		/*=================
		Check for Game Over
		=================*/
		if(money < 0 || suspicion >= 100)
		{
			_gameOver = true;
		}
	}

	void StartGameTime()
	{
		StartCoroutine(TimeOneTwelfthSecond());
	}

	IEnumerator TimeOneTwelfthSecond()
	{
		while(!_gameOver)
		{
			gameTime.AddOneMinute();
			yield return new WaitForSeconds(1f/12);
		}
	}

	public void SetCurrentItem(int _index)
	{
		currentItemIndex = _index;
		_currentItemImageSpriteRenderer.sprite = itemSprites[_index];
	}

	public void SetCurrentPlot(GameObject _plot)
	{
		currentPlot = _plot;
	}

	public void OpenShop()
	{
		isShopOpen = true;
		_shopMenu.SetActive(true);
	}

	public void CloseShop()
	{
		isShopOpen = false;
		_shopMenu.SetActive(false);
	}
}
