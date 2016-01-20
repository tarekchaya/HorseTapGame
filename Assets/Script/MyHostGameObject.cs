using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MyHostGameObject : MonoBehaviour {

	private bool m_initialized;
	private MyFuelSDKListener m_listener;
	
	private void Awake ()
	{
		if (!m_initialized) {
			GameObject.DontDestroyOnLoad (gameObject);
			
			if (!Application.isEditor) {
				// Initialize the Fuel SDK listener
				// reference for later use by the launch
				// methods.
				m_listener = new MyFuelSDKListener ();
				FuelSDK.setListener (m_listener);
			}
			m_initialized = true;
		} else {
			GameObject.Destroy (gameObject);
        }
    }

	public void LaunchFuel (){
		// Launches the Fuel SDK online experience.
		FuelSDK.Launch ();
	}

	public void LaunchFuelWithScore (long score, string scoreString)
	{
		// Construct the match results dictionary using the cached match
		// data obtained from the SdkCompletedWithMatch() callback.
		Dictionary<string, object> matchResult = new Dictionary<string, object> ();
		matchResult.Add ("tournamentID", m_listener.TournamentID);
		matchResult.Add ("matchID", m_listener.MatchID);

		// The raw score will be used to compare results
		// between match players. This should be a positive
		// integer value.
		matchResult.Add ("score", score);
		
		// Specify a visual score to represent the raw score
		// in a different format in the UI. If no visual score
		// is provided then the raw score will be used.
		matchResult.Add ("visualScore", scoreString);
		
		// Post the match results to the Fuel SDK.
		FuelSDK.SubmitMatchResult (matchResult);

		// Re-launch the Fuel SDK online experience
		// to see who won or to play another match.
		FuelSDK.Launch ();
    }

}
