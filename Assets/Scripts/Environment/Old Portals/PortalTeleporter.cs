using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour {

	[SerializeField] private Transform player;
	[SerializeField] private Transform reciever;

	private bool playerOverlaps = false;

	void Update () {
		if (playerOverlaps)
		{
			Vector3 portalToPlayer = player.position - transform.position;
			float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

			// If this is true: The player has moved across the portal
			if (dotProduct < 0f)
			{
				// Teleport
				float rotationDiff = -Quaternion.Angle(transform.rotation, reciever.rotation);
				rotationDiff += 180;
				player.Rotate(Vector3.up, rotationDiff);

				Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
				player.position = reciever.position + positionOffset;

				playerOverlaps = false;
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player")
		{
			playerOverlaps = true;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Player")
		{
			playerOverlaps = false;
		}
	}
}
