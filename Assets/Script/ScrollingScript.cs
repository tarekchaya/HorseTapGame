using UnityEngine;
using System.Collections;

public class ScrollingScript : MonoBehaviour {

	public float speed = 0.5f;
	public GameObject plyctrl;

	PlayerController playerController;

	// Use this for initialization
	void Start () {
	
		playerController = plyctrl.GetComponent<PlayerController>();

	}
	
	// Update is called once per frame
	void Update () {

		if (playerController.GameStart == true) {
			speed = 0.1f;
		} else {
			speed = 0f;
		}

		Vector2 offset = new Vector2 (Time.time * speed, 0);

		GetComponent<Renderer>().material.mainTextureOffset = offset;
	
	}
}
