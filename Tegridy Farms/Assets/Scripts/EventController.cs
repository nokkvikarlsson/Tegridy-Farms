using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventController : MonoBehaviour 
{


    private GameObject _farmerDialogue;
    private GameObject _landlordDialogue;
    private GameObject _laundererDialogue;
    private GameObject _policeDialogue;

    private Text _farmerText;
    private Text _landlordText;
    private Text _laundererText;
    private Text _policeText;
    private SoundController _soundController;

    private void Awake()
    {
        _soundController = FindObjectOfType<SoundController>();

        _farmerDialogue = GameObject.Find("/UI/Dialogues/DialogueFarmer");
        _landlordDialogue = GameObject.Find("/UI/Dialogues/DialogueLandlord");
        _laundererDialogue = GameObject.Find("/UI/Dialogues/DialogueLaunderer");
        _policeDialogue = GameObject.Find("/UI/Dialogues/DialoguePoliceOfficer");

        _farmerText = _farmerDialogue.transform.GetChild(2).gameObject.GetComponent<Text>();
        _landlordText = _landlordDialogue.transform.GetChild(2).gameObject.GetComponent<Text>();
        _laundererText = _laundererDialogue.transform.GetChild(2).gameObject.GetComponent<Text>();
        _policeText = _policeDialogue.transform.GetChild(2).gameObject.GetComponent<Text>();

        _farmerDialogue.SetActive(false);
        _landlordDialogue.SetActive(false);
        _laundererDialogue.SetActive(false);
        _policeDialogue.SetActive(false);
    }

    // Use this for initialization
    void Start() 
    {

	}
	
	// Update is called once per frame
	void Update() 
    {
		
	}

    public void DisplayDialoueFarmer(string text)
    {
        _farmerText.text = text;
        _farmerDialogue.SetActive(true);
        _soundController.PlayRandom(_soundController.farmerSounds); //Plays a random farmer gibberish
        StartCoroutine(stopForTenSeconds(_farmerDialogue));
    }

    public void DisplayDialogueLandlord(string text)
    {
        _landlordText.text = text;
        _landlordDialogue.SetActive(true);
        _soundController.PlayRandom(_soundController.landLordSounds); //Plays a random farmer gibberish
        StartCoroutine(stopForTenSeconds(_landlordDialogue));
    }

    public void DisplayDialogueLaunderer(string text)
    {
        _laundererText.text = text;
        _laundererDialogue.SetActive(true);
        _soundController.PlayRandom(_soundController.laundererSounds); //Plays a random farmer gibberish
        StartCoroutine(stopForTenSeconds(_laundererDialogue));

    }

    public void DisplayDialoguePolice(string text)
    {
        _policeText.text = text;
        _policeDialogue.SetActive(true);
        _soundController.PlayRandom(_soundController.policeSounds); //Plays a random farmer gibberish
        StartCoroutine(stopForTenSeconds(_policeDialogue));
    }

    IEnumerator stopForTenSeconds(GameObject dialogue)
    {
        print(Time.time);
        yield return new WaitForSeconds(10);
        dialogue.SetActive(false);
        print(Time.time);
    }
}
