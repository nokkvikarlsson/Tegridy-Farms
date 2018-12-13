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

    private Text _farmerText;
    private Text _landlordText;
    private Text _laundererText;
    private Text _policeText;
    private SoundController _soundController;

    private void Awake()
    {
        _soundController = FindObjectOfType<SoundController>();

        farmerDialogue = GameObject.Find("/UI/Dialogues/DialogueFarmer");
        landlordDialogue = GameObject.Find("/UI/Dialogues/DialogueLandlord");
        laundererDialogue = GameObject.Find("/UI/Dialogues/DialogueLaunderer");
        policeDialogue = GameObject.Find("/UI/Dialogues/DialoguePoliceOfficer");

        _farmerText = farmerDialogue.transform.GetChild(2).gameObject.GetComponent<Text>();
        _landlordText = landlordDialogue.transform.GetChild(2).gameObject.GetComponent<Text>();
        _laundererText = laundererDialogue.transform.GetChild(2).gameObject.GetComponent<Text>();
        _policeText = policeDialogue.transform.GetChild(2).gameObject.GetComponent<Text>();

        farmerDialogue.SetActive(false);
        landlordDialogue.SetActive(false);
        laundererDialogue.SetActive(false);
        policeDialogue.SetActive(false);
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
        farmerDialogue.SetActive(true);
        _soundController.PlayRandom(_soundController.farmerSounds); //Plays a random farmer gibberish
        StartCoroutine(stopForTenSeconds(farmerDialogue));
    }

    public void DisplayDialogueLandlord(string text)
    {
        _landlordText.text = text;
        landlordDialogue.SetActive(true);
        _soundController.PlayRandom(_soundController.landLordSounds); //Plays a random farmer gibberish
        StartCoroutine(stopForTenSeconds(landlordDialogue));
    }

    public void DisplayDialogueLaunderer(string text)
    {
        _laundererText.text = text;
        laundererDialogue.SetActive(true);
        _soundController.PlayRandom(_soundController.laundererSounds); //Plays a random farmer gibberish
        StartCoroutine(stopForTenSeconds(laundererDialogue));

    }

    public void DisplayDialoguePolice(string text)
    {
        _policeText.text = text;
        policeDialogue.SetActive(true);
        _soundController.PlayRandom(_soundController.policeSounds); //Plays a random farmer gibberish
        StartCoroutine(stopForTenSeconds(policeDialogue));
    }

    IEnumerator stopForTenSeconds(GameObject dialogue)
    {
        print(Time.time);
        yield return new WaitForSeconds(10);
        dialogue.SetActive(false);
        print(Time.time);
    }
}
