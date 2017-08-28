﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player7 : MonoBehaviour {

	public float speed;
	private float edge;

	Transform transform;
	Rigidbody rb;

	public Text winText;
	public Text instructionText;
	public GameObject panel;
	public GameObject player;
	public GameObject retryButton;
	public GameObject isometricButton;
	public GameObject nextButton;

	public GameObject logObject;
	public GameObject target;

	void Start ()
	{
		transform = GetComponent<Transform> ();
		rb = GetComponent<Rigidbody> ();
		edge = transform.localScale.x;

		winText.text = "";
	}

	void Update ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		if (moveHorizontal != 0 || moveVertical != 0)
			logObject.GetComponent<PlaneExplorationLog> ().LogDetail (-moveVertical * speed, 
				moveHorizontal * speed, transform.position.ToString ());

		if (transform.position.x - speed * moveVertical >= edge / 2
			&& transform.position.x - speed * moveVertical <= 3 - edge / 2
			&& transform.position.z + speed * moveHorizontal >= edge / 2
			&& transform.position.z + speed * moveHorizontal <= 3 - edge / 2) {

			transform.position += speed * (new Vector3 (-moveVertical, 0, moveHorizontal));

			if (transform.position.z >= 1 && transform.position.z <= 2 &&
				transform.position.x <= 1 && transform.position.y >= 1.5f) {
				rb.useGravity = false;
				transform.position += speed * (new Vector3 (0, -moveHorizontal, 0));
				transform.eulerAngles = new Vector3 (45, 0, 0);
			} else if ((transform.position.z <= 1 || transform.position.z >= 2) &&
			           transform.position.x >= 1.1 && transform.position.x <= 2) {
				rb.useGravity = false;
				transform.position += speed * (new Vector3 (0, moveVertical, 0));
				transform.eulerAngles = new Vector3 (0, 0, -45);
			}
			else {
				rb.useGravity = true;
			}

		}


	}

	public void Win(){
		panel.SetActive (true);
		winText.text = "You Win!";
		instructionText.text = "";
		player.SetActive (false);
		nextButton.SetActive (true);
		isometricButton.SetActive (true);
		target.SetActive (false);
		DataUtil.UnlockCurrentRoom();
		logObject.GetComponent<PlaneExplorationLog> ().RecordResult (1);
	}
}
