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
    [HideInInspector]
    public bool hasLost;

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
    private EventController _eventController;
    private SoundController _soundController;
    private LaunderController _launderController;

    //VARIABLES FOR SOUNDCONTROLLER:
    //Tells if a sound has been played
    public AudioSource music;
    private bool _lossSoundPlayed;

    //PRIVATE VARIABLES FOR EVENTCONTROLLER:
    //Tells if the message is aldready being displayed
    private bool _suspicionWarning75;
    private bool _allowedToPlayWarning75;
    private bool _allowedToPlayRentNotification;
    private bool _beginTegridyIntroduction;
    private string[] _suspicionDialogues;
    private string[] _rentDialogues;

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

        //EventController
        _eventController = FindObjectOfType<EventController>();
        _suspicionWarning75 = false;
        _allowedToPlayWarning75 = false;
        _allowedToPlayRentNotification = true;
        _beginTegridyIntroduction = false;

        _suspicionDialogues = new string[4];
        _suspicionDialogues[0] = "This farm looks very suspicious to me chief.";
        _suspicionDialogues[1] = "Hey chief, is corn supposed to be green?";
        _suspicionDialogues[2] = "I have never seen a corn farm this profitable, hmmm.";
        _suspicionDialogues[3] = "*sniff* *sniff* hey chief do you smell weed? I think it’s coming from this farm.";

        _rentDialogues = new string[4];
        _rentDialogues[0] = "Hey the rent is due after 6 hours at 12 AM don't forget it.";
        _rentDialogues[1] = "Howdy, you owe me rent and I’m coming to collect it after six hours at 12 AM.";
        _rentDialogues[2] = "Hey buddy, you better pay the rent in six hours at 12 AM.";
        _rentDialogues[3] = "You know what happens after six hours at 12 AM? rent time.";


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

        //Finding sound controller
        _soundController = FindObjectOfType<SoundController>();

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

        /*=================
         Dialogue checkers
       =================*/
         
        //Starts the introduction
        if(!_beginTegridyIntroduction)
        {
            _beginTegridyIntroduction = true;
            _eventController.DisplayDialogueFarmer("Howdy");
        }

        //If suspicion is 75 or over and the game is still playing then display suspicion warning message.
        if (suspicion >= 75 && !_suspicionWarning75 && !_gameOver)
        {
            int index = Random.Range(0, _suspicionDialogues.Length); 
            _eventController.DisplayDialoguePolice(_suspicionDialogues[index]); //Displayes a rando dialogue for the Police
            _suspicionWarning75 = true; //Tells if suspicion is already been played because the player went over 75 suspicion
            StartCoroutine(AllowSuspicionWarning75());

        }
        //If suspicion goes lower than 75 then the animation may be played again if the player goes over 75 again,
        //but only if the animation is no already playing.
        if (suspicion < 75 && _allowedToPlayWarning75)
        {
            _suspicionWarning75 = false;
            //Tells the controller to wait for the animation to finish before he plays it again.
            _allowedToPlayWarning75 = false;
        }

        //The landlord lets the player know that rent is due soon
        if (_timeCounterText.text == " 6:00 PM" && _allowedToPlayRentNotification)
        {
            _allowedToPlayRentNotification = false;
            int index = Random.Range(0, _rentDialogues.Length);
            _eventController.DisplayDialogueLandlord(_rentDialogues[index]);
            StartCoroutine(AllowToDisplayRentNotification());
        }

    }

    IEnumerator AllowToDisplayRentNotification()
    {
        yield return new WaitForSeconds(1);
        _allowedToPlayRentNotification = true;
    }

    //Waits for the suspicion warning dialogue to finish before allowing it to play again.
    IEnumerator AllowSuspicionWarning75()
    {
        yield return new WaitForSeconds(10);
        _allowedToPlayWarning75 = true;
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
            if(suspicion >= 0.007)
            {
                suspicion -= 0.007;
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
        }

    }

    private void DisplayTotalMoney()
    {
        _displayScore.Display();
    }

    public void RentCollection()
	{
		money -= 200;
	}

    public int getDayCounter()
    {
        return gameTime.day;
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
