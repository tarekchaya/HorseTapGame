using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	
	public GameObject HorseHead;
	public GameObject Timer;
	public GameObject HorseButt;
	public GameObject MainMenuGameObject;
	public GameObject CarrotAmount;
	public GameObject SugarAmount;
	public GameObject CosmicAmount;
	public GameObject BestTimeGO;
	public GameObject horseIdle;
	public GameObject horseFast;
	public GameObject hostgameobject;

	//UI elements

	public GameObject ChallengeCount;
	public GameObject ChallengeCountLabel;
	public GameObject VirtualGoodsMessageCarrot;
	public GameObject VirtualGoodsMessageSugar;
	public GameObject VirtualGoodsMessageCosmic;
	public GameObject multiplayerButton;
	public GameObject tournamentButton;

	private UISlider _horseHeadPos;
	private UILabel _labelTimer;
	private UILabel _labelCarrotAmount;
	private UILabel _labelSugarAmount;
	private UILabel _labelCosmicAmount;
	private UILabel _labelBestTime;
	private UILabel _labelChallengeCount;
	
	[HideInInspector]
	public float timer = 1f;
	[HideInInspector]
	public float BestTime =0f;

	[HideInInspector]
	public float HorseHitNumber = 0;
	private float carrotWaitTime = 5f;
	private float carrotAmount = 1f;
	private float sugarAmount = 0f;
	private float cosmicAmount = 0f;


	[HideInInspector]
	public bool GameStart = false;
	private bool finish = false;
	private bool Multiplayer = false;

	private BoxCollider HorseButtColider;

	// Use this for initialization
	void Start () {

		_horseHeadPos = HorseHead.GetComponent<UISlider>();
		_labelTimer = Timer.GetComponent<UILabel>();
		_labelCarrotAmount = CarrotAmount.GetComponent<UILabel>();
		_labelSugarAmount = SugarAmount.GetComponent<UILabel>();
		_labelCosmicAmount = CosmicAmount.GetComponent<UILabel>();
		_labelBestTime = BestTimeGO.GetComponent<UILabel>();
		HorseButtColider = HorseButt.GetComponent<BoxCollider>();
		_labelChallengeCount = ChallengeCountLabel.GetComponent<UILabel>();
		_horseHeadPos.value = 0f;
		getHighScores ();
		HorseButtColider.enabled = false;

		FuelSDK.SyncChallengeCounts ();
		FuelSDK.SyncVirtualGoods();
		FuelSDK.SyncTournamentInfo ();
	
	}

	// Update is called once per frame
	void Update () {

		_labelCarrotAmount.text = "" + carrotAmount;
		_labelSugarAmount.text = "" + sugarAmount;
		_labelCosmicAmount.text = "" + cosmicAmount;
		_labelBestTime.text = "" + (Mathf.Round(BestTime * 100) / 100 ) + " sec";

		if (GameStart == true) {

			HorseButtColider.enabled = true;

			//Sets labels
			_labelTimer.text = ""+ (Mathf.Round(timer * 100) / 100 ) + " sec";

			//Sets status text
			if (HorseHitNumber == 100) {
				if (finish == false) {

					if (Multiplayer == true) {	
						long longRaceTime = Convert.ToInt64(timer *100);
						string s=string.Format("{0:0.00}",timer);
						long ModifiedLongRaceTime = 100000 - longRaceTime;
						getHostGameObjectClass().LaunchFuelWithScore(ModifiedLongRaceTime, s);
						Multiplayer = false;
					}

					carrotAmount ++;
					setHighScores ();
					getHighScores ();
					finish = true;
					GameStart = false;
					HorseButtColider.enabled = false;
					_horseHeadPos.value = 0f;
					HorseHitNumber = 0;
					timer = 0;
					horseFast.SetActive(false);
					horseIdle.SetActive(true);
					MainMenuGameObject.SetActive (true);
				}
			}

			//Moves the horse head icon across the progress bar
			if (_horseHeadPos.value < 1f) {
				_horseHeadPos.value = Mathf.Lerp( _horseHeadPos.value, HorseHitNumber/100f, Time.deltaTime*10) ;
			}
			else if (_horseHeadPos.value > 1f) {
				_horseHeadPos.value = 1f;
			}

			//Starts stops the timer when you reach the finish line.
			if (_horseHeadPos.value < 1f) {
				timer += Time.deltaTime;
			}

			FuelSDK.SyncChallengeCounts ();
			FuelSDK.SyncVirtualGoods();
			FuelSDK.SyncTournamentInfo ();

		}

	}

	static public MyHostGameObject getHostGameObjectClass()
	{
		GameObject _MyHostGameObjectHandler = GameObject.Find("MyHostGameObject");
		if (_MyHostGameObjectHandler != null) {
			MyHostGameObject _MyHostGameObjectScript = _MyHostGameObjectHandler.GetComponent<MyHostGameObject> ();
			if(_MyHostGameObjectScript != null) {
				return _MyHostGameObjectScript;
			}
			throw new Exception();
		}
		throw new Exception();
	}

	public void ShowHorseHit () {

		HorseHitNumber ++;

	}

	private void getHighScores () {

		if (PlayerPrefs.GetFloat("bestTime") != null){
			BestTime = PlayerPrefs.GetFloat("bestTime");
		}

		if (PlayerPrefs.GetFloat("NumbOfCarrots") != null){
			carrotAmount = PlayerPrefs.GetFloat("NumbOfCarrots");
		}

		if (PlayerPrefs.GetFloat("NumbOfSugar") != null){
			sugarAmount = PlayerPrefs.GetFloat("NumbOfSugar");
		}

		if (PlayerPrefs.GetFloat("NumbOfCosmic") != null){
			cosmicAmount = PlayerPrefs.GetFloat("NumbOfCosmic");
		}

	}

	private void setHighScores () {

		if (timer < BestTime) {
			PlayerPrefs.SetFloat("bestTime",timer);
		}

		PlayerPrefs.SetFloat("NumbOfCarrots",carrotAmount);
		PlayerPrefs.SetFloat("NumbOfSugar",sugarAmount);
		PlayerPrefs.SetFloat("NumbOfCosmic",cosmicAmount);
	}

	public void resetPlayerPrefs () {

		PlayerPrefs.DeleteAll();

	}

	public void startGame () {

		MainMenuGameObject.SetActive (false);
		finish = false;
		horseIdle.SetActive(false);
		horseFast.SetActive(true);
		GameStart = true;
		
	}

	public void LaunchFuelMultiplayer () {

		getHostGameObjectClass().LaunchFuel();

	}

	public void startMultiplayerGame () {

		Multiplayer = true;
		MainMenuGameObject.SetActive (false);
		finish = false;
		horseIdle.SetActive(false);
		horseFast.SetActive(true);
		GameStart = true;
		
	}

	public void ShowChallengeCount (int count) {

		ChallengeCount.SetActive (true);
		_labelChallengeCount.text = "" + count;

	}

	public void RefreshChallengeCount () {

		FuelSDK.SyncChallengeCounts ();
		FuelSDK.SyncVirtualGoods();
		FuelSDK.SyncTournamentInfo ();
		setHighScores();
		getHighScores();
	}

	public void HideChallengeCount () {

		ChallengeCount.SetActive (false);

	}

	public void AddVirtualGoods (string good) {

		if (good == "Carrot") {
			carrotAmount += 10f;
		}

		if (good == "Sugar") {
			sugarAmount += 1f;
		}

		if (good == "Cosmic") {
			cosmicAmount += 1f;
		}


	}

	public void RemoveVirtualGoods (string good) {

		if (good == "Carrot") {
			carrotAmount -= 10f;
		}

		if (good == "Sugar") {
			sugarAmount -= 1f;
		}
		
		if (good == "Cosmic") {
			cosmicAmount -= 1f;
		}

	}

	public void ShowVirtualGoodBoard (string good) {

		if (good == "Carrot") {
			VirtualGoodsMessageCarrot.SetActive (true);
		}
		
		if (good == "Sugar") {
			VirtualGoodsMessageSugar.SetActive (true);
		}
		
		if (good == "Cosmic") {
			VirtualGoodsMessageCosmic.SetActive (true);
		}

	}

	public void HideVirtualGoodBoardCarrot () {	
		VirtualGoodsMessageCarrot.SetActive (false);
	}

	public void HideVirtualGoodBoardSugar () {
		VirtualGoodsMessageSugar.SetActive (false);
	}

	public void HideVirtualGoodBoardCosmic () {
		VirtualGoodsMessageCosmic.SetActive (false);
	}

	public void ShowMultiplayerButton () {
		multiplayerButton.SetActive(true);
		tournamentButton.SetActive(false);
	}

	public void ShowTournamentButton () {
		multiplayerButton.SetActive(false);
		tournamentButton.SetActive(true);
	}

}
