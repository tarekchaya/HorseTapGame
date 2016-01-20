using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MainMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {

		//FuelSDK.SyncVirtualGoods();

		FuelSDK.SyncChallengeCounts ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartTheGame () {



	}

	/*public void addVirtualGoods (int extraCarrots) {

		carrotAmount += extraCarrots;
		PlayerPrefs.SetFloat("NumbOfCarrots",carrotAmount);

	}*/
}
