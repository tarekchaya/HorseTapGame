using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FuelSDKSimpleJSON;

public class MyFuelSDKListener : FuelSDKListener {

	private enum EventType
	{
		leaderboard = 0,
		mission = 1,
		quest = 2
	};

	public string TournamentID { get; set; }
	public string MatchID { get; set; }

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
		PlayerControllerClass().startMultiplayerGame();
	}

	public override void OnCompeteChallengeCount (int count)
	{
		if (count > 0) {
			PlayerControllerClass().ShowChallengeCount(count);
		} else {
			PlayerControllerClass().HideChallengeCount();
		}
	}

	public override void OnVirtualGoodList (string transactionID, List<object> virtualGoods)
	{

		bool consumed = true;
		
		foreach (object virtualGoodObject in virtualGoods) {
			Dictionary<string, object> virtualGood = virtualGoodObject as Dictionary<string, object>;
			
			if (virtualGood == null) {
				Debug.Log ("OnVirtualGoodList - invalid virtual good data type: " + virtualGoodObject.GetType ().Name);
				consumed = false;
				break;
			}
			
			object goodIDObject = virtualGood["goodId"];
			
			if (goodIDObject == null) {
				Debug.Log ("OnVirtualGoodList - missing expected virtual good ID");
				consumed = false;
				break;
			}
			
			if (!(goodIDObject is string)) {
				Debug.Log ("OnVirtualGoodList - invalid virtual good ID data type: " + goodIDObject.GetType ().Name);
				consumed = false;
				break;
			}
			
			string goodID = (string)goodIDObject;

			PlayerControllerClass().AddVirtualGoods(goodID);

			if (consumed) {
				
				PlayerControllerClass().ShowVirtualGoodBoard(goodID);
				
			} else {

				PlayerControllerClass().RemoveVirtualGoods(goodID);

			}
		
		FuelSDK.AcknowledgeVirtualGoods(transactionID, consumed);

		}
	}

	public override void OnVirtualGoodRollback (string transactionID)
	{
		// revert the player's local (and/or remote) virtual goods inventory
		// by comparing the given transaction ID against the cached received
		// virtual good lists
	}

	public override void OnCompeteTournamentInfo (Dictionary<string, string> tournamentInfo)
	{
		if ((tournamentInfo == null) || (tournamentInfo.Count == 0)) {
			// There is no tournament currently running or scheduled.
			PlayerControllerClass().ShowMultiplayerButton();
		} else {
			// A tournament is currently running or is the
			// information for the next scheduled tournament.
			
			// Extract the tournament data.
			string name = tournamentInfo["name"];
			string campaignName = tournamentInfo["campaignName"];
			string sponsorName = tournamentInfo["sponsorName"];
			string startDate = tournamentInfo["startDate"];
			string endDate = tournamentInfo["endDate"];
			string logo = tournamentInfo["logo"];
				
			PlayerControllerClass().ShowTournamentButton();
		}
	}

	static public PlayerController PlayerControllerClass()
	{
		GameObject _PlayerControllerHandler = GameObject.Find("PlayerController");
		if (_PlayerControllerHandler != null) {
			PlayerController _PlayerControllerScript = _PlayerControllerHandler.GetComponent<PlayerController> ();
			if(_PlayerControllerScript != null) {
				return _PlayerControllerScript;
			}
			throw new Exception();
		}
		throw new Exception();
	}

	static public igniteDebugScript IgniteDebugClass()
	{
		GameObject _IgniteDebugHandler = GameObject.Find("QuestUI");
		if (_IgniteDebugHandler != null) {
			igniteDebugScript _IgniteDebugScript = _IgniteDebugHandler.GetComponent<igniteDebugScript> ();
			if(_IgniteDebugScript != null) {
				return _IgniteDebugScript;
			}
			throw new Exception();
		}
		throw new Exception();
	}

	public void GetLeaderBoard(string Id) {
		bool success = FuelSDK.GetLeaderBoard( Id );
		if(success == true) {
			//Everything is good you can expect your data in the event callback
		}
	}
	
	public void GetMission(string Id) {
		bool success = FuelSDK.GetMission( Id );
		if(success == true) {
			//Everything is good you can expect your data in the event callback
		}
	}
	
	public void GetQuest(string Id) {
		bool success = FuelSDK.GetQuest( Id );
		if(success == true) {
			//Everything is good you can expect your data in the event callback
		}
	}
	
	
	public override void OnIgniteEvents( List<object> events ) {
		
		if (events == null) {
			Debug.Log ("OnIgniteEvents - undefined list of events");
			return;
		}
		
		if (events.Count == 0) {
			Debug.Log ("OnIgniteEvents - empty list of events");
			return;
		}
		
		foreach (object eventObject in events) {
			Dictionary<string, object> eventInfo = eventObject as Dictionary<string, object>;
			
			if (eventInfo == null) {
				Debug.Log ("OnIgniteEvents - invalid event data type: " 
				           + eventObject.GetType ().Name);
				continue;
			}
			
			object eventIdObject = eventInfo["id"];
			
			if (eventIdObject == null) {
				Debug.Log ("OnIgniteEvents - missing expected event ID");
				continue;
			}
			
			if (!(eventIdObject is string)) {
				Debug.Log ("OnIgniteEvents - invalid event ID data type: " 
				           + eventIdObject.GetType ().Name);
				continue;
			}
			
			string eventId = (string)eventIdObject;
			
			object eventTypeObject = eventInfo["type"];
			
			if (eventTypeObject == null) {
				Debug.Log ("OnIgniteEvents - missing expected event type");
				continue;
			}
			
			if (!(eventTypeObject is long)) {
				Debug.Log ("OnIgniteEvents - invalid event type data type: " 
				           + eventTypeObject.GetType ().Name);
				continue;
			}
			
			long eventTypeLong = (long)eventTypeObject;
			
			int eventTypeValue = (int)eventTypeLong;
			
			if (!Enum.IsDefined (typeof (EventType), eventTypeValue)) {
				Debug.Log ("OnIgniteEvents - unsupported event type value: " 
				           + eventTypeValue.ToString ());
				continue;
			}
			
			EventType eventType = (EventType)eventTypeValue;
			
			object eventJoinedObject = eventInfo["joined"];
			
			if (eventJoinedObject == null) {
				Debug.Log ("OnIgniteEvents - missing expected event joined status");
				continue;
			}
			
			if (!(eventJoinedObject is bool)) {
				Debug.Log ("OnIgniteEvents - invalid event joined data type: " 
				           + eventJoinedObject.GetType ().Name);
				continue;
			}
			
			bool eventJoined = (bool)eventJoinedObject;
			
			string eventTypeString = eventType.ToString ();
			
			if (eventJoined) {
				Debug.Log ("OnIgniteEvents - player is joined in event of type '" 
				           + eventTypeString + "' with event ID: " + eventId);
				
				switch (eventType) {
				case EventType.leaderboard:
					GetLeaderBoard((string)eventIdObject);
					break;
				case EventType.mission:
					GetMission((string)eventIdObject);
					break;
				case EventType.quest:
					GetQuest((string)eventIdObject);
					break;
				default:
					Debug.Log ("OnIgniteEvents - unsupported event type: " + eventTypeString);
					continue;
				}
			} else {
				Debug.Log ("OnIgniteEvents - player can opt-in to join event of type '" 
				           + eventTypeString + "' with event ID: " + eventId);
			}
			
		}
		
	}

	public override void OnIgniteLeaderBoard (Dictionary<string, object> leaderBoard)
	{
		if (leaderBoard == null) {
			Debug.Log ("OnIgniteLeaderBoard - undefined leaderboard");
			return;
		}
		
		if (leaderBoard.Count == 0) {
			Debug.Log ("OnIgniteLeaderBoard - empty leaderboard");
			return;
		}
		
		string leaderBoardString = FuelSDKCommon.Serialize (leaderBoard);
		
		if (leaderBoardString == null) {
			Debug.Log ("OnIgniteLeaderBoard - unable to serialize the leaderboard");
			return;
		}
		
		Debug.Log ("OnIgniteLeaderBoard - leaderboard: " + leaderBoardString);
		
		// process the leaderboard information
	}
	
	public override void OnIgniteMission (Dictionary<string, object> mission)
	{
		if (mission == null) {
			Debug.Log ("OnIgniteMission - undefined mission");
			return;
		}
		
		if (mission.Count == 0) {
			Debug.Log ("OnIgniteMission - empty mission");
			return;
		}
		
		string missionString = FuelSDKCommon.Serialize (mission);
		
		if (missionString == null) {
			Debug.Log ("OnIgniteMission - unable to serialize the mission");
			return;
		}
		
		Debug.Log ("OnIgniteMission - mission: " + missionString);

		if( mission.ContainsKey("rules") ) {
			List<object> rulesList = mission["rules"] as List<object>;
			foreach(object rule in rulesList ) {
				Dictionary<string,object> ruleDict = rule as Dictionary<string, object>;
				
				if( ruleDict.ContainsKey("id") ) {
					string Id = Convert.ToString( ruleDict["id"] );
					IgniteDebugClass()._id.text = Id;
				}
				if( ruleDict.ContainsKey("score") ) {
					int Score = Convert.ToInt32( ruleDict["score"] );
					IgniteDebugClass()._score.text = "" + Score;
				}
				if( ruleDict.ContainsKey("target") ) {
					int Target = Convert.ToInt32( ruleDict["target"] );
					IgniteDebugClass()._target.text = "" + Target;
				}
				if( ruleDict.ContainsKey("achieved") ) {
					bool Achieved = Convert.ToBoolean( ruleDict["achieved"] );
					IgniteDebugClass()._achieved.text = "" + Achieved;
				}
				if( ruleDict.ContainsKey("variable") ) {
					string Variable = Convert.ToString( ruleDict["variable"] );
					IgniteDebugClass()._variable.text = Variable;
				}
				if( ruleDict.ContainsKey("kind") ) {
					string Kind = Convert.ToString( ruleDict["kind"] );
				}
				if( ruleDict.ContainsKey( "metadata" ) ) {
					string metadataString = Convert.ToString( ruleDict["metadata"] );
					//Parse your meta data
				}
			}
		}
		
		// process the mission information
	}
	
	public override void OnIgniteQuest (Dictionary<string, object> quest)
	{
		if (quest == null) {
			Debug.Log ("OnIgniteQuest - undefined quest");
			return;
		}
		
		if (quest.Count == 0) {
			Debug.Log ("OnIgniteQuest - empty quest");
			return;
		}
		
		string questString = FuelSDKCommon.Serialize (quest);
		
		if (questString == null) {
			Debug.Log ("OnIgniteQuest - unable to serialize the quest");
			return;
		}
		
		Debug.Log ("OnIgniteQuest - quest: " + questString);
		
		// process the quest information
	}
	
}
