using UnityEngine;
using System.Collections;

public class loadingSceneController : MonoBehaviour {

	// Use this for initialization
	void Start () {

		StartCoroutine(MoveToNextScene());
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator MoveToNextScene () {

		yield return new WaitForSeconds(5);
		Application.LoadLevel(1);


	}
}
