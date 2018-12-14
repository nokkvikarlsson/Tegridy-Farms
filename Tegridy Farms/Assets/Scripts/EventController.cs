using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventController : MonoBehaviour 
{

    [HideInInspector]
    public GameObject farmerDialogue;
    [HideInInspector]
    public GameObject landlordDialogue;
    [HideInInspector]
    public GameObject laundererDialogue;
    [HideInInspector]
    public GameObject laundererDialogue2;
    [HideInInspector]
    public GameObject policeDialogue;
    [HideInInspector]
    public GameObject farmerDialogue2;

    private Text _farmerText;
    private Text _landlordText;
    private Text _laundererText;
    private Text _policeText;

    private Text _farmerText2;
    private Text _laundererText2;

    //PRIVATE VARIABLES FOR EVENTCONTROLLER:
    //Tells if the message is aldready being displayed
    private bool _suspicionWarning75;
    private bool _allowedToPlayWarning75;
    private bool _allowedToPlayRentNotification;
    private bool _allowedToPlayRentCollection;
    private bool _beginTegridyIntroduction;
    public bool beginTegridyIntroduction2;
    public bool introdution2Done;
    private bool firstTimeOver10Suspicion;
    public bool playLaunderer;
    public bool playLaunderer2;
    private bool _playLastIntroduction;
    private bool _playLaundryIntroduction;
    public bool playAllLaundryDialogue;

    private string[] _suspicionDialogues;
    private string[] _rentDialogues;

    private SoundController _soundController;
    private GameController _gameController;

    private void Awake()
    {
        _soundController = FindObjectOfType<SoundController>();
        _gameController = FindObjectOfType<GameController>();

        farmerDialogue = GameObject.Find("/UI/Dialogues/DialogueFarmer");
        landlordDialogue = GameObject.Find("/UI/Dialogues/DialogueLandlord");
        laundererDialogue = GameObject.Find("/UI/Dialogues/DialogueLaunderer");
        policeDialogue = GameObject.Find("/UI/Dialogues/DialoguePoliceOfficer");

        //if the farmer needs to tall the player something after an event
        farmerDialogue2 = GameObject.Find("/UI/Dialogues/DialogueFarmer2");

        laundererDialogue2 = GameObject.Find("/UI/Dialogues/DialogueLaunderer2");

        _farmerText = farmerDialogue.transform.GetChild(2).gameObject.GetComponent<Text>();
        _landlordText = landlordDialogue.transform.GetChild(2).gameObject.GetComponent<Text>();
        _laundererText = laundererDialogue.transform.GetChild(2).gameObject.GetComponent<Text>();
        _policeText = policeDialogue.transform.GetChild(2).gameObject.GetComponent<Text>();

        _farmerText2 = farmerDialogue2.transform.GetChild(2).gameObject.GetComponent<Text>();
        _laundererText2 = laundererDialogue2.transform.GetChild(2).gameObject.GetComponent<Text>();

        farmerDialogue.SetActive(false);
        landlordDialogue.SetActive(false);
        laundererDialogue.SetActive(false);
        policeDialogue.SetActive(false);


        farmerDialogue2.SetActive(false);
        laundererDialogue2.SetActive(false);


        _suspicionWarning75 = false;
        _allowedToPlayWarning75 = false;
        _allowedToPlayRentNotification = true;
        _allowedToPlayRentCollection = true;

        //Introduction
        _beginTegridyIntroduction = true;
        beginTegridyIntroduction2 = false;
        introdution2Done = false;
        firstTimeOver10Suspicion = true;
        _playLastIntroduction = false;

        //Laundry
        playLaunderer = false;
        playLaunderer2 = false;
        _playLaundryIntroduction = true;
        playAllLaundryDialogue = true;

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

	}
	
	// Update is called once per frame
	void Update() 
    {
        /*=================
         Dialogue checkers
       =================*/

        //***************************Introduction*******************************
        //Starts the introduction
        if (_beginTegridyIntroduction)
        {
            _beginTegridyIntroduction = false;
            DisplayDialogueFarmer("Howdy, farmer! Let's get to work. Select the crop you wish to plant from the SHOP menu!");
        }

        if (beginTegridyIntroduction2 && !introdution2Done)
        {
            introdution2Done = true;
            beginTegridyIntroduction2 = false;
            DisplayDialogueFarmer2("Now that you have selected a crop, click on a plot to plant it! Remember to harvest it when it's ready.");
        }

        if (_gameController.suspicion >= 4 && firstTimeOver10Suspicion)
        {
            firstTimeOver10Suspicion = false;
            DisplayDialogueFarmer("You have to manage your suspicion. Harvesting illegal crops raises it but you can use tobacco to lower it.");
            StartCoroutine(AllowToPlayLastIntroduction());

        }

        if (_playLastIntroduction)
        {
            DisplayDialogueFarmer2("Rent is collected every day at midnight so make sure you have enough by then.");
            _playLastIntroduction = false;
        }


        //***********************Suspicion Warning****************************
        //If suspicion is 75 or over and the game is still playing then display suspicion warning message.
        if (_gameController.suspicion >= 75 && !_suspicionWarning75 && !_gameController.gameOver)
        {
            int index = Random.Range(0, _suspicionDialogues.Length);
            DisplayDialoguePolice(_suspicionDialogues[index]); //Displayes a rando dialogue for the Police
            _suspicionWarning75 = true; //Tells if suspicion is already been played because the player went over 75 suspicion
            StartCoroutine(AllowSuspicionWarning75());

        }
        //If suspicion goes lower than 75 then the animation may be played again if the player goes over 75 again,
        //but only if the animation is no already playing.
        if (_gameController.suspicion < 75 && _allowedToPlayWarning75)
        {
            _suspicionWarning75 = false;
            //Tells the controller to wait for the animation to finish before he plays it again.
            _allowedToPlayWarning75 = false;
        }


        //************************Rent Warning********************************
        //The landlord lets the player know that rent is due soon
        if (_gameController.gameTime.hour == 18 && _gameController.gameTime.minute == 0 && _allowedToPlayRentNotification)
        {
            _allowedToPlayRentNotification = false;
            int index = Random.Range(0, _rentDialogues.Length);
            DisplayDialogueLandlord(_rentDialogues[index]);
            StartCoroutine(AllowToDisplayRentNotification());
        }

        if(_gameController.gameTime.hour == 23 && _gameController.gameTime.minute == 59 && _allowedToPlayRentCollection)
        {
            _allowedToPlayRentCollection = false;
            DisplayDialogueLandlord("Rent Time! Rent will now be higher >:)");
        }
        else
        {
            _allowedToPlayRentNotification = true;
        }


        //**********************Laundry Introduction*************************
        //Play laundererdialogue when the player has expanded his farm for the first time
        if (_gameController.plotsize == 3 && _playLaundryIntroduction)
        {
            _playLaundryIntroduction = false;
            DisplayDialogueLaunderer("Nice business you got there, farmer! Have you considered laundering your income?");
            StartCoroutine(waitToStartLaundererIntroduction2());
        }

        if (playLaunderer && playAllLaundryDialogue)
        {
            playLaunderer = false;
            DisplayDialogueLaunderer("You can pay businesses to launder a part of your income so it doesn't raise suspicion");
            StartCoroutine(waitToStartLaundererIntroduction3());
        }

        if(playLaunderer2 && playAllLaundryDialogue)
        {
            playLaunderer2 = false;
            DisplayDialogueLaunderer("Check the 'Launder' tab in the SHOP menu to view available businesses;)");
        }
    }

    IEnumerator waitToStartLaundererIntroduction3()
    {
        yield return new WaitForSeconds(10);
        playLaunderer2 = true;
    }

    IEnumerator waitToStartLaundererIntroduction2()
    {
        yield return new WaitForSeconds(10);
        playLaunderer = true;
    }

    IEnumerator AllowToPlayLastIntroduction()
    {
        yield return new WaitForSeconds(7);
        _playLastIntroduction = true;
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


    public void DisplayDialogueFarmer(string text)
    {
        _farmerText.text = text;
        farmerDialogue.GetComponent<RectTransform>().SetAsLastSibling();
        farmerDialogue.SetActive(true);
        _soundController.PlayRandom(_soundController.farmerSounds); //Plays a random farmer gibberish
        StartCoroutine(stopForTenSeconds(farmerDialogue));
    }

    public void DisplayDialogueLandlord(string text)
    {
        _landlordText.text = text;
        landlordDialogue.GetComponent<RectTransform>().SetAsLastSibling();
        landlordDialogue.SetActive(true);
        _soundController.PlayRandom(_soundController.landLordSounds); //Plays a random landlord gibberish
        StartCoroutine(stopForTenSeconds(landlordDialogue));
    }

    public void DisplayDialogueLaunderer(string text)
    {
        _laundererText.text = text;
        laundererDialogue.GetComponent<RectTransform>().SetAsLastSibling();
        laundererDialogue.SetActive(true);
        _soundController.PlayRandom(_soundController.laundererSounds); //Plays a random launderer gibberish
        StartCoroutine(stopForTenSeconds(laundererDialogue));
    }

    public void DisplayDialoguePolice(string text)
    {
        _policeText.text = text;
        policeDialogue.GetComponent<RectTransform>().SetAsLastSibling();
        policeDialogue.SetActive(true);
        _soundController.PlayRandom(_soundController.policeSounds); //Plays a random police gibberish
        StartCoroutine(stopForTenSeconds(policeDialogue));
    }

    public void DisplayDialogueFarmer2(string text)
    {
        _farmerText2.text = text;
        farmerDialogue2.GetComponent<RectTransform>().SetAsLastSibling();
        farmerDialogue2.SetActive(true);
        _soundController.PlayRandom(_soundController.farmerSounds); //Plays a random farmer gibberish
        StartCoroutine(stopForTenSeconds(farmerDialogue2));
    }

    public void DisplayDialogueLaunderer2(string text)
    {
        _laundererText2.text = text;
        laundererDialogue2.GetComponent<RectTransform>().SetAsLastSibling();
        laundererDialogue2.SetActive(true);
        _soundController.PlayRandom(_soundController.laundererSounds); //Plays a random launderer gibberish
        StartCoroutine(stopForTenSeconds(laundererDialogue2));
    }


    IEnumerator stopForTenSeconds(GameObject dialogue)
    {
        yield return new WaitForSeconds(10);
        dialogue.SetActive(false);
    }
}
