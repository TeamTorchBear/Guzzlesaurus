using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Acceleration : MonoBehaviour {

	public Text text;

	void Update() {
		text.text = "Acceleration: " + Input.acceleration;
	}
}
