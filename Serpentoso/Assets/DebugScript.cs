using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugScript : MonoBehaviour {

	void Start () {
	}

	void Update () {
		int fps = Mathf.RoundToInt(1 / Time.deltaTime);
		Debug.Log (fps);
	}
}
