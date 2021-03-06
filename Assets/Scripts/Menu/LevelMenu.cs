﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour {
	public int current;
	public string[] maps;
	public Selectable[] pmaps;
	bool[] canInteract;
	bool readyToLoad = false;
	int numControllers;
	float limiter = 0;
	GameObject PressA;

	void Start()
	{
		string[] joysticks = Input.GetJoystickNames ();
		current = 0;
		numControllers = joysticks.Length;
		canInteract = new bool[] { true, true, true,true};
		pmaps [current].GetComponent<SpriteRenderer> ().color = Color.magenta;
		PressA = GameObject.Find ("PressAtoPlay");
		PressA.GetComponent<SpriteRenderer> ().color = Color.clear;
	}

	void Update()
	{
		float input;
		for (int i = 0; i < 3; i++) {
			input = Input.GetAxisRaw ("Vertical_P" + (i + 1));
			if (input != 0 && canInteract[i]) {
				canInteract[i] = false;
				StartCoroutine (SelectionChange (input, i));
			}
			if (Input.GetButtonDown ("Accept_P" + (i + 1))) {
				if (readyToLoad == true) {
					GameObject.Find ("GameOptions").GetComponent<GameOptions> ().level = maps [current];
					Destroy (GameObject.Find ("Menu Music"));
					SceneManager.LoadScene (maps [current]);
				} else {
					PressA.GetComponent<SpriteRenderer> ().color = Color.white;
					readyToLoad = true;
				}
			}
		}
	}

	float getSubAmount(int ctrl)
	{
		if (ctrl == 0)
			return 1;
		else if (ctrl == 1)
			return 0;
		else
			return -1;
	}

	IEnumerator SelectionChange(float input, int controller)
	{
		pmaps [current].GetComponent<SpriteRenderer> ().color = Color.white;

		if (input < 0 && current < maps.Length - 1) {
			current++;
		} else if (input > 0 && current > 0) {
			current--;
		}

		float subFrom = getSubAmount (controller);
		readyToLoad = false;

		pmaps [current].GetComponent<SpriteRenderer> ().color = Color.magenta;

		yield return new WaitForSeconds (0.2f);
		canInteract[controller] = true;

	}
}
