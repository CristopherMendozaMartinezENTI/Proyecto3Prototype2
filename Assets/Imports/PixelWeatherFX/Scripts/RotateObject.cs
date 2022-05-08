using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {

	public bool rotate = true;
	public float speed = 7.0f;

	void Start() {
		rotate = true;
	}

	// Update is called once per frame
	void Update () {
		if(rotate == true){
			transform.Rotate ( Vector3.up * (speed * Time.deltaTime ) );
		}
	}
}
