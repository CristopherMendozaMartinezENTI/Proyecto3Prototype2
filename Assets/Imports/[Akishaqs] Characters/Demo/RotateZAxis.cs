using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateZAxis : MonoBehaviour {

	[SerializeField] private int speed = 5;

	// Use this for initialization
	void Update () 
	{
		transform.Rotate(0,0, speed * Time.deltaTime);
	}
}
