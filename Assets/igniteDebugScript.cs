using UnityEngine;
using System.Collections;

public class igniteDebugScript : MonoBehaviour {

	public GameObject id;
	public GameObject score;
	public GameObject target;
	public GameObject achieved;
	public GameObject variable;

	[HideInInspector]
	public UILabel _id;
	[HideInInspector]
	public UILabel _score;
	[HideInInspector]
	public UILabel _target;
	[HideInInspector]
	public UILabel _achieved;
	[HideInInspector]
	public UILabel _variable;

	// Use this for initialization
	void Start () {

		_id = id.GetComponent<UILabel>();
		_score = score.GetComponent<UILabel>();
		_target = target.GetComponent<UILabel>();
		_achieved = achieved.GetComponent<UILabel>();
		_variable = variable.GetComponent<UILabel>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
