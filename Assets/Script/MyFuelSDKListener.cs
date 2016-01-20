using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FuelSDKSimpleJSON;

public class MyFuelSDKListener : FuelSDKListener {

	public string TournamentID { get; set; }
	public string MatchID { get; set; }

	public MainMenuController mmc;

	public override void OnCompeteUICompletedWithExit () 
	{

		Application.LoadLevel(0);

	}

	public override void OnCompeteUIFailed (string reason)
	{

		Application.LoadLevel(0);
	
	}

	public override void OnCompeteUICompletedWithMatch (Dictionary<string, object> matchResult)
	{
		// Sdk completed with a match.
		
		// Extract the match information and cache if for
		// later when posting the match score.
		TournamentID = matchResult ["tournamentID"].ToString();
		MatchID = matchResult ["matchID"].ToString();
		
		// Extract the params data.
		string paramsJSON = matchResult ["params"].ToString();
		JSONNode json = JSONNode.Parse (paramsJSON);
		
		// Extract the match seed value to be used for any
		// randomization seeding. The seed value will be
		// the same for each match player.
		long seed = 0;
		
		// Must parse long values manually since SimpleJSON
		// doesn't yet provide this function automatically.
		if (!long.TryParse(json ["seed"], out seed))
		{
			// invalid string encoded long value, defaults to 0
		}
		
		// Extract the match round value.
		int round = json ["round"].AsInt;
		
		// Extract the ads allowed flag to be used to
		// determine if in-game ads should be allowed in
		// this match.
		bool adsAllowed = json ["adsAllowed"].AsBool;
		
		// Extract the fair play flag to be used to
		// determine if a level playing field between the
		// match players should be enforced.
		bool fairPlay = json ["fairPlay"].AsBool;
		
		// Extract the options data.
		JSONClass options = json ["options"].AsObject;
		
		// Extract the player's public profile data.
		JSONClass you = json ["you"].AsObject;
		string yourNickname = you ["name"];
		string yourAvatarURL = you ["avatar"];
		
		// Extract the opponent's public profile data.
		JSONClass them = json ["them"].AsObject;
		string theirNickname = them ["name"];
		string theirAvatarURL = them ["avatar"];
		
		// Play the game and pass any extracted match
		// data as necessary.
		mmc.StartTheGame();
	}



}
