using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Este script se encarga de castear Raycast
//Es utilizada por PlayerColliderController
[System.Serializable]
public class RayCaster 
{
	public float castLength = 1f;
	private Vector3 origin = Vector3.zero;

	public enum CastDirection
	{
		Forward,
		Right,
		Up,
		Backward, 
		Left,
		Down
	}

	private CastDirection castDirection;
	private bool hasDetectedHit;
	private Vector3 hitPosition;
	private Vector3 hitNormal;
	private float hitDistance;
	private List<Collider> hitColliders = new List<Collider>();
	private List<Transform> hitTransforms = new List<Transform>();

	private Transform tr;
	public LayerMask layermask = 255;
	int ignoreRaycastLayer;

	private Collider[] ignoreList;
	private int[] ignoreListLayers;

	public RayCaster(Transform _transform, Collider _collider)
	{
		tr = _transform;

		if(_collider == null)
			return;

		ignoreList = new Collider[1];
		ignoreList[0] = _collider;
		ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
		ignoreListLayers = new int[ignoreList.Length];
	}

	private void Reset()
	{
		hasDetectedHit = false;
		hitPosition = Vector3.zero;
		hitNormal = -GetCastDirection();
		hitDistance = 0f;

		if (hitColliders.Count > 0)
			hitColliders.Clear();
		if (hitTransforms.Count > 0)
			hitTransforms.Clear();
	}

	public void Cast()
	{
		Reset();
		Vector3 _worldDirection = GetCastDirection();
		Vector3 _worldOrigin = tr.TransformPoint(origin);

		if(ignoreListLayers.Length != ignoreList.Length)
		{
			ignoreListLayers = new int[ignoreList.Length]; 
		}

		for(int i = 0; i < ignoreList.Length; i++)
		{
			ignoreListLayers[i] = ignoreList[i].gameObject.layer;
			ignoreList[i].gameObject.layer = ignoreRaycastLayer;
		}

		CastRay(_worldOrigin, _worldDirection);

		for(int i = 0; i < ignoreList.Length; i++)
		{
			ignoreList[i].gameObject.layer = ignoreListLayers[i];
		}
	}

	private void CastRay(Vector3 _origin, Vector3 _direction)
	{
		RaycastHit _hit;
		hasDetectedHit = Physics.Raycast(_origin, _direction, out _hit, castLength, layermask, QueryTriggerInteraction.Ignore);

		if(hasDetectedHit)
		{
			hitPosition = _hit.point;
			hitNormal = _hit.normal;

			hitColliders.Add(_hit.collider);
			hitTransforms.Add(_hit.transform);

			hitDistance = _hit.distance;
		}
	}

	Vector3 GetCastDirection()
	{
		switch(castDirection)
		{
		case CastDirection.Forward:
			return tr.forward;

		case CastDirection.Right:
			return tr.right;

		case CastDirection.Up:
			return tr.up;

		case CastDirection.Backward:
			return -tr.forward;

		case CastDirection.Left:
			return -tr.right;

		case CastDirection.Down:
			return -tr.up;
		default:
			return Vector3.one;
		}
	}

	public bool HasDetectedHit()
	{
		return hasDetectedHit;
	}

	public float GetDistance()
	{
		return hitDistance;
	}

	public Vector3 GetNormal()
	{
		return hitNormal;
	}

	public Vector3 GetPosition()
	{
		return hitPosition;
	}

	public Collider GetCollider()
	{
		return hitColliders[0];
	}

	public Transform GetTransform()
	{
		return hitTransforms[0];
	}

	public void SetCastOrigin(Vector3 _origin)
	{
		if(tr == null)
			return;
		origin = tr.InverseTransformPoint(_origin);
	}

	public void SetCastDirection(CastDirection _direction)
	{
		if(tr == null)
			return;

		castDirection = _direction;
	}
}
