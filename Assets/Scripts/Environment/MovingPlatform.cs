using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingPlatform : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private float movementSpeed = 10f;
	[SerializeField] private float waitTime = 1f;
	[SerializeField] private bool reverseDirection = false;
	public bool movementEnabled = false;
	private bool isWaiting = false;
	Rigidbody r;
	TriggerArea triggerArea;
	[SerializeField] private List<Transform> waypoints = new List<Transform>();
	int currentWaypointIndex = 0;
	Transform currentWaypoint;

	void Start () {
		r = GetComponent<Rigidbody>();
		triggerArea = GetComponentInChildren<TriggerArea>();
		r.freezeRotation = true;
		r.useGravity = false;
		r.isKinematic = true;

		if(waypoints.Count <= 0){
			Debug.LogWarning("Faltan puntos a los que se pueda mover la plataforma. Agregalos a lista crack.");
		} else {
			currentWaypoint = waypoints[currentWaypointIndex];
		}

		StartCoroutine(WaitRoutine());
		StartCoroutine(LateFixedUpdate());
	}

	IEnumerator LateFixedUpdate()
	{
		WaitForFixedUpdate _instruction = new WaitForFixedUpdate();
		while(true)
		{
			yield return _instruction;
			MovePlatform();
		}
	}

	void MovePlatform () 
	{

		if(movementEnabled)
        {
			if (waypoints.Count <= 0)
				return;

			if (isWaiting)
				return;

			Vector3 _toCurrentWaypoint = currentWaypoint.position - transform.position;
			Vector3 _movement = _toCurrentWaypoint.normalized;
			_movement *= movementSpeed * Time.deltaTime;

			if (_movement.magnitude >= _toCurrentWaypoint.magnitude || _movement.magnitude == 0f)
			{
				r.transform.position = currentWaypoint.position;
				UpdateWaypoint();
			}
			else
			{
				r.transform.position += _movement;
			}

			if (triggerArea == null)
				return;

			for (int i = 0; i < triggerArea.rigidbodiesInTriggerArea.Count; i++)
			{
				triggerArea.rigidbodiesInTriggerArea[i].MovePosition(triggerArea.rigidbodiesInTriggerArea[i].position + _movement);
			}
		}
	}

	private void UpdateWaypoint()
	{
		if(reverseDirection)
			currentWaypointIndex --;
		else
			currentWaypointIndex ++;

		if(currentWaypointIndex >= waypoints.Count)
			currentWaypointIndex = 0;

		if(currentWaypointIndex < 0)
			currentWaypointIndex = waypoints.Count - 1;

		currentWaypoint = waypoints[currentWaypointIndex];
		isWaiting = true;
	}

	IEnumerator WaitRoutine()
	{
		WaitForSeconds _waitInstruction = new WaitForSeconds(waitTime);

		while(true)
		{
			if(isWaiting)
			{
				yield return _waitInstruction;
				isWaiting = false;
			}

			yield return null;
		}
	}	
}
