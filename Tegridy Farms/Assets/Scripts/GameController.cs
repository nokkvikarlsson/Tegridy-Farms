using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
	public Sprite[] itemSprites; //
	public GameObject shopMenu;
	public int totalMoneyEarned;
	public GameObject[] plots;

    //PRIVATE:
    private Image _currentItemImageSprite;
	private RectTransform _cropsTab;
	private Text _moneyCounterText;
	private Text _timeCounterText;
	private Text _dayCounterText;
	private Slider _barSlider;
	private bool _gameOver;
    private GameObject _lossSuspicionText;
    private GameObject _lossRentText;
    private GameObject _lossCanvas;
    private DisplayScore _displayScore;
    private DisplayScore _DisplayScore;

    void Awake()
    {
        //initalize plotsize at 2x2
        plotsize = 2;
        //Create starting plots at (0,0) (1,0) (0,-1), (1,-1)
        for (int x = 0; x < plotsize; x++)
        {
            for (int y = 0; y > -1 * (plotsize); y--)
            {
                GameObject plot = (GameObject)Instantiate(plotPrefab);
                plot.transform.position = new Vector3(x, y);
            }
        }
        //Create surrounding plots

        //initialize plots
        plots = GameObject.FindGameObjectsWithTag("plot");
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
        shopMenu = Resources.FindObjectsOfTypeAll<Shop>()[0].gameObject;
        GameObject _currentItemImage = GameObject.Find("/UI/CurrentItemButton/CurrentItemImage");
        _currentItemImageSprite = _currentItemImage.GetComponent<Image>();
        GameObject _plantsTabPane = shopMenu.transform.GetChild(0).GetChild(0).gameObject;
        _cropsTab = _plantsTabPane.GetComponent<RectTransform>();

        //Start Game Time
        StartGameTime();
        //LossText and set active to false
        _lossSuspicionText = GameObject.Find("LossSuspicionText");
        _lossRentText = GameObject.Find("LossRentText");
        _lossCanvas = GameObject.Find("LossCanvas");
        _DisplayScore = FindObjectOfType<DisplayScore>();
    }

    // Use this for initialization
    void Start()
    {
        _cropsTab.SetAsLastSibling();
        _lossRentText.SetActive(false);
        _lossSuspicionText.SetActive(false);
        _lossCanvas.GetComponent<Animator>().enabled = false;
    }

    // Update is called once per frame
    void Update()
	{
		/*=================
		Update Values of UI
		=================*/
		//Update UI MoneyCounter to match
		_moneyCounterText.text = money.ToString() + "$";
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
        if (suspicion >= 100)
        {
            _gameOver = true;
            gameOverSequence(true);
        }
        if (money < 0)
		{
			_gameOver = true;
            gameOverSequence(false);
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
			if(gameTime.day != 1 && gameTime.hour == 0 && gameTime.minute == 0)
			{
				Debug.Log("Rent Collection!");
				RentCollection();
			}
			yield return new WaitForSeconds(1f/12);
		}
	}

	public void SetCurrentItem(int _index)
	{
		currentItemIndex = _index;
		_currentItemImageSprite.sprite = itemSprites[_index];
	}

	public void SetCurrentPlot(GameObject _plot)
	{
		currentPlot = _plot;
	}

	public void OpenShop()
	{
		isShopOpen = true;
		
		_cropsTab.SetAsLastSibling();
		shopMenu.SetActive(true);

        for (int i = 0; i < plots.Length; i++)
        {
            plots[i].GetComponent<BoxCollider2D>().enabled = false;
        }
    }

	public void CloseShop()
	{
		isShopOpen = false;
		shopMenu.SetActive(false);

        for (int i = 0; i < plots.Length; i++)
        {
            plots[i].GetComponent<BoxCollider2D>().enabled = true;
        }
    }

	public void removeMoney(int price)
	{
		money -= price;
	}

	public void addMoney(int sellvalue)
	{
		money += sellvalue;
		totalMoneyEarned += sellvalue;
	}

	public void addSuspicion(int sellvalue, double plantSuspicion)
	{
		//add launder stuff in future
		suspicion += ((double)sellvalue * plantSuspicion);
		if(suspicion < 0)
		{
			suspicion = 0;
		}
	}

    private void gameOverSequence(bool isSuspicion)
    {
        //If the player lost due to suspicion play the lossCanvas animation and display the loss text
        if(isSuspicion)
        {
            CloseShop();
           DisplayTotalMoney();
            _lossCanvas.GetComponent<Animator>().enabled = true;
            _lossSuspicionText.SetActive(true);
        }
        //If the player lost due to rent play the lossCanvas animation and display the loss text
        else
        {
            CloseShop();
            DisplayTotalMoney();
            _lossCanvas.GetComponent<Animator>().enabled = true;
            _lossRentText.SetActive(true);
        }

    }

    private void DisplayTotalMoney()
    {
        _DisplayScore.Display();
    }

    public void RentCollection()
	{
		money -= 200;
	}

    public int getDayCounter()
    {
        return gameTime.day;
    }
}
