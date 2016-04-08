﻿using UnityEngine;
using System.Collections;

public class CharacterMenu : MonoBehaviour {
	public int current;
	public Selectable[] characters;
	public int[] selected;
	public GameObject[] selectors;
	bool[] canInteract;
	int numControllers;
	float limiter = 0;

	void Start()
	{
		string[] joysticks = Input.GetJoystickNames ();
		selected = new int[joysticks.Length];
		numControllers = joysticks.Length;
		canInteract = new bool[] { true, true, true };
		selected = new int[] { 0, 0, 0 };
	}

	void Update()
	{
		int input;
		for (int i = 0; i < 3; i++) {
			input = (int) Input.GetAxisRaw ("Horizontal_P" + (i + 1));
			if (input != 0 && canInteract[i]) {
				canInteract[i] = false;
				StartCoroutine (SelectionChange (input, i));
			}
		}
	}

	void MoveSelector(GameObject sel, int curCtrl, float sub)
	{
		Vector3 charPos = new Vector3(characters [selected [curCtrl]].transform.position.x - sub,
			sel.transform.position.y, sel.transform.position.z);
		sel.transform.position = charPos;
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

	IEnumerator SelectionChange(int input, int controller)
	{
		if (input > 0 && selected[controller] < characters.Length - 1) {
			selected[controller]++;
		} else if (input < 0 && selected[controller] > 0) {
			selected[controller]--;
		}

		float subFrom = getSubAmount (controller);

		MoveSelector (selectors[controller], controller, subFrom);
		yield return new WaitForSeconds (0.2f);
		canInteract[controller] = true;
	}
}
