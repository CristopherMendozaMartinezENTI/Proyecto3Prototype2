using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Esto es una clase abstracta que se puede utilizar para crear controladores tanto players como IA
public abstract class Controller : MonoBehaviour {

	//Getters;
	public abstract Vector3 GetVelocity();
	public abstract Vector3 GetMovementVelocity();
	public abstract bool IsGrounded();

	//Events;
	public delegate void VectorEvent(Vector3 v);
	public VectorEvent OnJump;
	public VectorEvent OnLand;

}

