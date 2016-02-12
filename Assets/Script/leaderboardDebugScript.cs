using UnityEngine;
using System.Collections;

public class leaderboardDebugScript : MonoBehaviour {

	public GameObject id;
	public GameObject ranked;
	public GameObject progress;
	public GameObject currentUserId;
	public GameObject leader1;
	public GameObject leader2;
	public GameObject leader3;
	
	[HideInInspector]
	public UILabel _id;
	[HideInInspector]
	public UILabel _ranked;
	[HideInInspector]
	public UILabel _progress;
	[HideInInspector]
	public UILabel _currentUserId;
	[HideInInspector]
	public UILabel _leader1;
	[HideInInspector]
	public UILabel _leader2;
	[HideInInspector]
	public UILabel _leader3;
	
	// Use this for initialization
	void Start () {
		
		_id = id.GetComponent<UILabel>();
		_ranked = ranked.GetComponent<UILabel>();
		_progress = progress.GetComponent<UILabel>();
		_currentUserId = currentUserId.GetComponent<UILabel>();
		_leader1 = leader1.GetComponent<UILabel>();
		_leader2 = leader2.GetComponent<UILabel>();
		_leader3 = leader3.GetComponent<UILabel>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
