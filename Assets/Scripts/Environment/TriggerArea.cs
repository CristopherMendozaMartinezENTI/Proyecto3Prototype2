using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Este script lo usan las plataformas moviles
public class TriggerArea : MonoBehaviour {

	public List <Rigidbody> rigidbodiesInTriggerArea = new List<Rigidbody>();
	private void OnTriggerEnter(Collider col)
	{
		if(col.attachedRigidbody != null && col.GetComponent<PlayerColliderController>() != null)
		{
			rigidbodiesInTriggerArea.Add(col.attachedRigidbody);
		}
	}

	private void OnTriggerExit(Collider col)
	{
		if(col.attachedRigidbody != null && col.GetComponent<PlayerColliderController>() != null)
		{
			rigidbodiesInTriggerArea.Remove(col.attachedRigidbody);
		}
	}
}

