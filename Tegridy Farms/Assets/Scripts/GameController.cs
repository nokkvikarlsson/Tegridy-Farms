using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour 
{
	//PUBLIC:
	public int plotsize; //starts at 2x2
	public GameObject plotPrefab; //the prefab for plot which is copied to expand
	public int money; //starts at 200
	public double suspicion; //0-100. Game ends at >= 100
	public GameTime gameTime; //time in the game: DAY 1 00:00
	public int currentItemIndex; //int of index in shopitem array
	public GameObject currentPlot; //selected plot on which to plant
	public bool isShopOpen; //is the shop menu active?
	public Sprite[] itemSprites; //All Sprites for Items for current item display
	public GameObject shopMenu;
	public int totalMoneyEarned;
	public GameObject[] plots;
    public int displayChecker; //NokkviKilla needs this variable
    public Plant[] allPlants; //List of all Plants and Buildings that player can plant
    public RectTransform cropsTab; //The crops tab in the shop
    public GameObject housePlot; //The House Plot
    public GameObject shopButton;
    public GameObject currentItemButton;
    public int rent;
    [HideInInspector]
    public bool hasLost;

    //PRIVATE:
    private Image _currentItemImageSprite;
	private Text _moneyCounterText;
	private Text _timeCounterText;
	private Text _dayCounterText;
    private Text _currentRentText;
	private Slider _barSlider;
	public bool gameOver;
    private GameObject _lossSuspicionText;
    private GameObject _lossRentText;
    private GameObject _lossCanvas;
    private DisplayScore _displayScore;
    private EventController _eventController;
    private SoundController _soundController;
    private LaunderController _launderController;

    private GameObject _mainCamera;
	private Camera _mainCameraComp;

    //VARIABLES FOR SOUNDCONTROLLER:
    //Tells if a sound has been played
    public AudioSource music;
    private bool _lossSoundPlayed;



    void Awake()
    {
        //Initialize allPlants from Shop Items
        ShopItems shopItems = GameObject.Find("/ShopItems").GetComponent<ShopItems>();
        allPlants = new Plant[shopItems.allPlants.Length];
        for (int i = 0; i < shopItems.allPlants.Length; i++)
        {
            allPlants[i] = Instantiate(shopItems.allPlants[i]);
            //allPlants[i].Clone(shopItems.allPlants[i]);
            Debug.Log(allPlants[i].type);
        }
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
        //initialize plots
        plots = GameObject.FindGameObjectsWithTag("plot");
        //initialize variables
        money = 200;
        suspicion = 0;
        gameTime = new GameTime();
        currentItemIndex = 0;
        currentPlot = null;
        isShopOpen = false;
        gameOver = false;
        rent = 200;
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
        GameObject _plantsTabPane = shopMenu.transform.GetChild(0).GetChild(3).gameObject;
        cropsTab = _plantsTabPane.GetComponent<RectTransform>();
        GameObject _rentObject = GameObject.Find("/UI/CurrentRent/Value");
        _currentRentText = _rentObject.GetComponent<Text>();
        _currentRentText.text = rent.ToString() + "$";
        _mainCamera = GameObject.Find("/Main Camera");
		_mainCameraComp = _mainCamera.GetComponent<Camera>();
        housePlot = GameObject.Find("/HousePlot");
        shopButton = GameObject.Find("/UI/TopPanel/ShopButton");
        currentItemButton = GameObject.Find("/UI/CurrentItemButton");
        //initalize Launder Controller
        _launderController = FindObjectOfType<LaunderController>();
        //Tells if the player has lost.
        hasLost = false;

        //Start Game Time
        StartGameTime();
        //LossText and set active to false
        _lossSuspicionText = GameObject.Find("LossSuspicionText");
        _lossRentText = GameObject.Find("LossRentText");
        _lossCanvas = GameObject.Find("LossCanvas");
        _displayScore = FindObjectOfType<DisplayScore>();
        _lossCanvas.SetActive(false);

        displayChecker = 0; //NokkviKilla needs this

        //SoundController
        _lossSoundPlayed = false;
        //Finding sound controller
        _soundController = FindObjectOfType<SoundController>();
    }

    // Use this for initialization
    void Start()
    {
        cropsTab.SetAsLastSibling();
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
        if(gameTime.hour == 0)
        {
            timetext += "12";
            timetext += ":";
		    if(gameTime.minute < 10) {timetext += "0";}
            timetext += gameTime.minute.ToString() + " AM";
        }
        else if(gameTime.hour == 12)
        {
            timetext += "12";
            timetext += ":";
		    if(gameTime.minute < 10) {timetext += "0";}
            timetext += gameTime.minute.ToString() + " PM";
        }
        else if(gameTime.hour < 12)
        {
            if (gameTime.hour < 10) {timetext += " ";}
            timetext += gameTime.hour.ToString();
            timetext += ":";
		    if(gameTime.minute < 10) {timetext += "0";}
            timetext += gameTime.minute.ToString() + " AM";
        }
        else 
        {
            if(gameTime.hour -12 < 10) {timetext += " ";}
            timetext += (gameTime.hour - 12).ToString();
            timetext += ":";
		    if(gameTime.minute < 10) {timetext += "0";}
            timetext += gameTime.minute.ToString() + " PM";
        }
		_timeCounterText.text = timetext;

        //Update UI BarSlider to match
        _barSlider.value = (float)suspicion;
        /*=================
		Check for Game Over
		=================*/
        if (suspicion >= 100)
        {
            gameOver = true;
            gameOverSequence(true);
        }
        if (money < 0)
        {
            gameOver = true;
            gameOverSequence(false);
        }
    }

    void StartGameTime()
    {
        StartCoroutine(TimeOneTwelfthSecond());
    }

    IEnumerator TimeOneTwelfthSecond()
	{
		while(!gameOver)
		{
			gameTime.AddOneMinute();
            if(suspicion >= 0.014)
            {
                suspicion -= 0.014;
            }
            else
            {
                suspicion = 0;
            }
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

        //Play sound
        _soundController.Play("OpenShop", _soundController.effectSounds);
		
		cropsTab.SetAsLastSibling();
		shopMenu.SetActive(true);

        for (int i = 0; i < plots.Length; i++)
        {
            plots[i].GetComponent<BoxCollider2D>().enabled = false;
        }
        housePlot.GetComponent<BoxCollider2D>().enabled = false;
    }

	public void CloseShop()
	{
		isShopOpen = false;
		shopMenu.SetActive(false);

        for (int i = 0; i < plots.Length; i++)
        {
            plots[i].GetComponent<BoxCollider2D>().enabled = true;
        }
        housePlot.GetComponent<BoxCollider2D>().enabled = true;
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

    public void addSuspicionUnlaundered(int sellvalue, double plantSuspicion)
    {
        suspicion += ((double)sellvalue * plantSuspicion);
        if(suspicion < 0)
        {
            suspicion = 0;
        }
    }

	public void addSuspicion(int sellvalue, double plantSuspicion)
	{
		if(_launderController.currentLaunder == null)
        {
            suspicion += ((double)sellvalue * plantSuspicion);
            if(suspicion < 0)
            {
                suspicion = 0;
            }
        }
        else
        {
            int totalPossibleLaunder = _launderController.currentLaunder.moneyLaunderCapacity - _launderController.currentMoneyLaundered;
            //Plant Sold can be fully laundered
            if(totalPossibleLaunder >= sellvalue)
            {
                //No suspicion added
                //add money to capacity
                _launderController.AddCurrentMoneyLaundered(sellvalue);
            }
            //Only part of plant can be laundered
            else
            {
                int partMoneyLaundered = sellvalue - totalPossibleLaunder;
                //part suspicion added
                suspicion += ((double)partMoneyLaundered * plantSuspicion);
                //add money to capacity
                _launderController.AddCurrentMoneyLaundered(sellvalue);
            }
        }
	}

    private void gameOverSequence(bool isSuspicion)
    {
        //If the player lost due to suspicion play the lossCanvas animation and display the loss text
        if(isSuspicion)
        {
            _lossCanvas.SetActive(true);
            //Checks if the loss sound has already been played
            if(!_lossSoundPlayed)
            { 
                _soundController.Play("LossSound", _soundController.effectSounds);
                _lossSoundPlayed = true;
            }
            CloseShop();
            _lossCanvas.GetComponent<Animator>().enabled = true;
            _lossSuspicionText.SetActive(true);
            hasLost = true;
            music.Stop();

            if(displayChecker == 0)
            {
                DisplayTotalMoney();
            }

            displayChecker = 1;

            //disable everything
            for (int i = 0; i < plots.Length; i++)
            {
                plots[i].GetComponent<BoxCollider2D>().enabled = false;
            }
            shopButton.GetComponent<Button>().enabled = false;
            currentItemButton.GetComponent<Button>().enabled = false;
            housePlot.GetComponent<BoxCollider2D>().enabled = false;
        }
        //If the player lost due to rent play the lossCanvas animation and display the loss text
        else
        {
            _lossCanvas.SetActive(true);
            //Checks if the loss sound has already been played
            if (!_lossSoundPlayed)
            {
                _soundController.Play("LossSound", _soundController.effectSounds);
                _lossSoundPlayed = true;
            }
            CloseShop();
            _lossCanvas.GetComponent<Animator>().enabled = true;
            _lossRentText.SetActive(true);
            hasLost = true;
            music.Stop();

            if (displayChecker == 0)
            {
                DisplayTotalMoney();
            }

            displayChecker = 1;

            //disable everything
            for (int i = 0; i < plots.Length; i++)
            {
                plots[i].GetComponent<BoxCollider2D>().enabled = false;
            }
            shopButton.GetComponent<Button>().enabled = false;
            currentItemButton.GetComponent<Button>().enabled = false;
            housePlot.GetComponent<BoxCollider2D>().enabled = false;
        }

    }

    private void DisplayTotalMoney()
    {
        _displayScore.Display();
    }

    public void RentCollection()
	{
		money -= rent;
        //increase rent every day
        IncreaseRent();
	}

    public void IncreaseRent()
    {
        double newRent = 0.0;
        //Increase rent by 35%
        if(plotsize != 8)
        {
            newRent = (double)rent * 1.35;
        }
        //At 8x8 Increase rent by 50%
        else
        {
            newRent = (double)rent * 1.5;
        }
        //Floor to nearest 50
        newRent = System.Math.Floor(newRent / 50) * 50;
        rent = (int)newRent;
        _currentRentText.text = rent.ToString() + "$";
    }

    public int getDayCounter()
    {
        return gameTime.day;
    }

    public void AdjustCamera()
	{
		//_mainCamera.transform.position += new Vector3(0.25f, -0.5f);
		//_mainCameraComp.orthographicSize += 0.2f;
		StartCoroutine(SlowZoom());
	}

    IEnumerator SlowZoom()
	{
		int i = 0;
		while(i < 100)
		{
			_mainCamera.transform.position += new Vector3(0.0025f, -0.005f);
			_mainCameraComp.orthographicSize += 0.002f;
			i++;
			yield return new WaitForSeconds(0.01f);
		}
	}

    public void CheckFertilizer()
    {
        for(int i = 0; i < plots.Length; i++)//in this plot
        {
            plots[i].GetComponent<Plot>().growthBonus = 0;
        }
        for(int i = 0; i < plots.Length; i++)//in this plot
        {
            for(int j = 0; j < plots.Length; j++)//check surrounding
            {
                //check if fertilizer above
                if(plots[i].transform.position.x == plots[j].transform.position.x
                && plots[i].transform.position.y + 1 == plots[j].transform.position.y
                && plots[j].GetComponent<Plot>().plant.type == "Fertilizer Dispenser"
                && plots[j].GetComponent<Plot>().buildingOn)
                {
                    plots[i].GetComponent<Plot>().growthBonus += ((double)plots[j].GetComponent<Plot>().plant.sellvalue / 100);
                }
                //if below
                if(plots[i].transform.position.x == plots[j].transform.position.x
                && plots[i].transform.position.y - 1 == plots[j].transform.position.y
                && plots[j].GetComponent<Plot>().plant.type == "Fertilizer Dispenser"
                && plots[j].GetComponent<Plot>().buildingOn)
                {
                    plots[i].GetComponent<Plot>().growthBonus += ((double)plots[j].GetComponent<Plot>().plant.sellvalue / 100);
                }
                //if left
                if(plots[i].transform.position.x - 1 == plots[j].transform.position.x
                && plots[i].transform.position.y == plots[j].transform.position.y
                && plots[j].GetComponent<Plot>().plant.type == "Fertilizer Dispenser"
                && plots[j].GetComponent<Plot>().buildingOn)
                {
                    plots[i].GetComponent<Plot>().growthBonus += ((double)plots[j].GetComponent<Plot>().plant.sellvalue / 100);
                }
                //if right
                if(plots[i].transform.position.x + 1 == plots[j].transform.position.x
                && plots[i].transform.position.y == plots[j].transform.position.y
                && plots[j].GetComponent<Plot>().plant.type == "Fertilizer Dispenser"
                && plots[j].GetComponent<Plot>().buildingOn)
                {
                    plots[i].GetComponent<Plot>().growthBonus += ((double)plots[j].GetComponent<Plot>().plant.sellvalue / 100);
                }
                //if top left
                if(plots[i].transform.position.x - 1 == plots[j].transform.position.x
                && plots[i].transform.position.y + 1 == plots[j].transform.position.y
                && plots[j].GetComponent<Plot>().plant.type == "Fertilizer Dispenser"
                && plots[j].GetComponent<Plot>().buildingOn)
                {
                    plots[i].GetComponent<Plot>().growthBonus += ((double)plots[j].GetComponent<Plot>().plant.sellvalue / 100);
                }
                //if top right
                if(plots[i].transform.position.x + 1 == plots[j].transform.position.x
                && plots[i].transform.position.y + 1 == plots[j].transform.position.y
                && plots[j].GetComponent<Plot>().plant.type == "Fertilizer Dispenser"
                && plots[j].GetComponent<Plot>().buildingOn)
                {
                    plots[i].GetComponent<Plot>().growthBonus += ((double)plots[j].GetComponent<Plot>().plant.sellvalue / 100);
                }
                //if bottom left
                if(plots[i].transform.position.x - 1 == plots[j].transform.position.x
                && plots[i].transform.position.y - 1 == plots[j].transform.position.y
                && plots[j].GetComponent<Plot>().plant.type == "Fertilizer Dispenser"
                && plots[j].GetComponent<Plot>().buildingOn)
                {
                    plots[i].GetComponent<Plot>().growthBonus += ((double)plots[j].GetComponent<Plot>().plant.sellvalue / 100);
                }
                //if bottom right
                if(plots[i].transform.position.x + 1 == plots[j].transform.position.x
                && plots[i].transform.position.y - 1 == plots[j].transform.position.y
                && plots[j].GetComponent<Plot>().plant.type == "Fertilizer Dispenser"
                && plots[j].GetComponent<Plot>().buildingOn)
                {
                    plots[i].GetComponent<Plot>().growthBonus += ((double)plots[j].GetComponent<Plot>().plant.sellvalue / 100);
                }
            }
        }
    }
}
