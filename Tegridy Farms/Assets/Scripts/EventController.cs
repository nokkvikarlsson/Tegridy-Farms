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
    public GameObject policeDialogue;
    [HideInInspector]
    public GameObject farmerDialogue2;

    private Text _farmerText;
    private Text _landlordText;
    private Text _laundererText;
    private Text _policeText;

    private Text _farmerText2;

    private SoundController _soundController;

    private void Awake()
    {
        _soundController = FindObjectOfType<SoundController>();

        farmerDialogue = GameObject.Find("/UI/Dialogues/DialogueFarmer");
        landlordDialogue = GameObject.Find("/UI/Dialogues/DialogueLandlord");
        laundererDialogue = GameObject.Find("/UI/Dialogues/DialogueLaunderer");
        policeDialogue = GameObject.Find("/UI/Dialogues/DialoguePoliceOfficer");

        //if the farmer needs to tall the player something after an event
        farmerDialogue2 = GameObject.Find("/UI/Dialogues/DialogueFarmer2");

        _farmerText = farmerDialogue.transform.GetChild(2).gameObject.GetComponent<Text>();
        _landlordText = landlordDialogue.transform.GetChild(2).gameObject.GetComponent<Text>();
        _laundererText = laundererDialogue.transform.GetChild(2).gameObject.GetComponent<Text>();
        _policeText = policeDialogue.transform.GetChild(2).gameObject.GetComponent<Text>();

        _farmerText2 = farmerDialogue2.transform.GetChild(2).gameObject.GetComponent<Text>();

        farmerDialogue.SetActive(false);
        landlordDialogue.SetActive(false);
        laundererDialogue.SetActive(false);
        policeDialogue.SetActive(false);


        farmerDialogue2.SetActive(false);
    }

    // Use this for initialization
    void Start() 
    {

	}
	
	// Update is called once per frame
	void Update() 
    {
		
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


    IEnumerator stopForTenSeconds(GameObject dialogue)
    {
        yield return new WaitForSeconds(10);
        dialogue.SetActive(false);
    }
}
