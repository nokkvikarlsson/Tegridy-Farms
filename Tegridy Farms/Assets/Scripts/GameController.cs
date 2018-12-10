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

	//Surrounding Plot Prefabs
	public GameObject surroundingPlot;
	public GameObject upperPlotPrefab;
	public GameObject upperLeftPlotPrefab;
	public GameObject upperRightPlotPrefab;
	public GameObject lowerPlotPrefab;
	public GameObject lowerRightPlotPrefab;
	public GameObject lowerLeftPlotPrefab;
	public GameObject leftPlotPrefab;
	public GameObject rightPlotPrefab;

    //PRIVATE:
    private SpriteRenderer _currentItemImageSpriteRenderer;
	private RectTransform _cropsTab;
	private Text _moneyCounterText;
	private Text _timeCounterText;
	private Text _dayCounterText;
	private Slider _barSlider;
	private bool _gameOver;
    private GameObject _lossSuspicionText;
    private GameObject _lossRentText;
    private GameObject _lossCanvas;
	public GameObject[] _plots;
    private UpdateTotalMoney _updateTotalMoney;
	private GameObject _mainCamera;

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
        _plots = GameObject.FindGameObjectsWithTag("plot");
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
        GameObject _currentItemImage = GameObject.Find("/CurrentItem/CurrentItemImage");
        _currentItemImageSpriteRenderer = _currentItemImage.GetComponent<SpriteRenderer>();
        GameObject _plantsTabPane = shopMenu.transform.GetChild(0).GetChild(0).gameObject;
        _cropsTab = _plantsTabPane.GetComponent<RectTransform>();

		_mainCamera = GameObject.Find("/Main Camera");
        //Start Game Time
        StartGameTime();
        //LossText and set active to false
        _lossSuspicionText = GameObject.Find("LossSuspicionText");
        _lossRentText = GameObject.Find("LossRentText");
        _lossCanvas = GameObject.Find("LossCanvas");
        _updateTotalMoney = FindObjectOfType<UpdateTotalMoney>();
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
		_currentItemImageSpriteRenderer.sprite = itemSprites[_index];
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

        for (int i = 0; i < _plots.Length; i++)
        {
            _plots[i].GetComponent<BoxCollider2D>().enabled = false;
        }
    }

	public void CloseShop()
	{
		isShopOpen = false;
		shopMenu.SetActive(false);

        for (int i = 0; i < _plots.Length; i++)
        {
            _plots[i].GetComponent<BoxCollider2D>().enabled = true;
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
        _updateTotalMoney.Display();
    }

    public void RentCollection()
	{
		money -= 200;
	}

	public void ExpandFarm()
	{
		for(int i = 0; i < plotsize; i++)
		{
			GameObject plot = (GameObject)Instantiate(plotPrefab);
			plot.transform.position = new Vector3(plotsize, -i);
		}
		for(int i = 0; i < plotsize; i++)
		{
			GameObject plot = (GameObject)Instantiate(plotPrefab);
			plot.transform.position = new Vector3(i, -plotsize);
		}
		GameObject cornerplot = (GameObject)Instantiate(plotPrefab);
		cornerplot.transform.position = new Vector3(plotsize, -plotsize);
		plotsize++;
		_plots = GameObject.FindGameObjectsWithTag("plot");
		AddSurroundingPlot();
		AdjustCamera();
	}

	void AddSurroundingPlot()
	{
		//move lowerleft down
		GameObject lowerleft = GameObject.FindGameObjectsWithTag("LowerLeftPlot")[0];
		lowerleft.transform.position += Vector3.down;
		//move upperright right
		GameObject upperright = GameObject.FindGameObjectsWithTag("UpperRightPlot")[0];
		upperright.transform.position += Vector3.right;
		//move lowerright down-right
		GameObject lowerright = GameObject.FindGameObjectsWithTag("LowerRightPlot")[0];
		lowerright.transform.position += (Vector3.right + Vector3.down);
		//add new left and upper plot
		GameObject left = (GameObject)Instantiate(leftPlotPrefab);
		left.transform.position = new Vector3(-1, -1*(plotsize-1));
		GameObject upper = (GameObject)Instantiate(upperPlotPrefab);
		upper.transform.position = new Vector3(plotsize-1, 1);
		//move lower plots down and add one
		GameObject[] lower = GameObject.FindGameObjectsWithTag("LowerPlot");
		for(int i = 0; i < lower.Length; i++)
		{
			lower[i].transform.position += Vector3.down;
		}
		GameObject newLower = (GameObject)Instantiate(lowerPlotPrefab);
		newLower.transform.position = new Vector3(plotsize-1, -plotsize);
		//move right plots right and add one
		GameObject[] right = GameObject.FindGameObjectsWithTag("RightPlot");
		for(int i = 0; i < lower.Length; i++)
		{
			right[i].transform.position += Vector3.right;
		}
		GameObject newRight = (GameObject)Instantiate(rightPlotPrefab);
		newRight.transform.position = new Vector3(plotsize, -(plotsize-1));
	}

	void AdjustCamera()
	{
		_mainCamera.transform.position += new Vector3(0.25f, -0.25f);
		Camera mainCameraComp = _mainCamera.GetComponent<Camera>();
		mainCameraComp.orthographicSize += 0.19f;
	}

    public int getDayCounter()
    {
        return gameTime.day;
    }
}
