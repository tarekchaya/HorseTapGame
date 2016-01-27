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
	public GameObject BestTimeGO;
	public GameObject horseIdle;
	public GameObject horseFast;
	public GameObject hostgameobject;
	public GameObject ChallengeCount;
	public GameObject ChallengeCountLabel;
	public GameObject VirtualGoodsMessage;

	//public MyHostGameObject myhostGO;

	private UISlider _horseHeadPos;
	private UILabel _labelTimer;
	private UILabel _labelCarrotAmount;
	private UILabel _labelBestTime;
	private UILabel _labelChallengeCount;
	
	[HideInInspector]
	public float timer = 1f;
	[HideInInspector]
	public float BestTime =0f;
	[HideInInspector]
	public float RaceNumber = 0f;

	[HideInInspector]
	public float HorseHitNumber = 0;
	//private bool carrotHappiness = false;
	private float carrotWaitTime = 5f;
	private float carrotAmount = 1f;

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
		_labelBestTime = BestTimeGO.GetComponent<UILabel>();
		HorseButtColider = HorseButt.GetComponent<BoxCollider>();
		_labelChallengeCount = ChallengeCountLabel.GetComponent<UILabel>();
		_horseHeadPos.value = 0f;
		getHighScores ();
		HorseButtColider.enabled = false;

		FuelSDK.SyncChallengeCounts ();
		FuelSDK.SyncVirtualGoods();
	
	}

	// Update is called once per frame
	void Update () {

		_labelCarrotAmount.text = "" + carrotAmount;
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

		if (PlayerPrefs.GetFloat("raceNumber") != null){
			RaceNumber = PlayerPrefs.GetFloat("raceNumber");
		}

		if (PlayerPrefs.GetFloat("NumbOfCarrots") != null){
			carrotAmount = PlayerPrefs.GetFloat("NumbOfCarrots");
		}

		RaceNumber++;

	}

	private void setHighScores () {

		if (RaceNumber == 1) {
			BestTime = timer;
			PlayerPrefs.SetFloat("bestTime",timer);
		}

		if (timer < BestTime) {
			PlayerPrefs.SetFloat("bestTime",timer);
		}

		PlayerPrefs.SetFloat("raceNumber",RaceNumber);
		PlayerPrefs.SetFloat("NumbOfCarrots",carrotAmount);
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
	}

	public void HideChallengeCount () {

		ChallengeCount.SetActive (false);

	}

	public void AddVirtualGoods () {

		carrotAmount += 100f;

	}

	public void RemoveVirtualGoods () {

		carrotAmount -= 100f;

	}

	public void ShowVirtualGoodBoard () {

		VirtualGoodsMessage.SetActive (true);

	}

	public void HideVirtualGoodBoard () {
		
		
		VirtualGoodsMessage.SetActive (false);
	}
	
	IEnumerator carrotTime () {

		//carrotHappiness = true;
		carrotAmount --;
		yield return new WaitForSeconds(carrotWaitTime);
		//carrotHappiness = false;

	}

}
