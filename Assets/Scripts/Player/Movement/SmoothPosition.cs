using UnityEngine;
using System.Collections;

//Este script permite suavizar el movimiento de cualquier objeto mediante Lerp o Damping 
public class SmoothPosition : MonoBehaviour
{
	Transform tr;
	[SerializeField] private Transform target;

	Vector3 currentPosition;
	[SerializeField] private float lerpSpeed = 20f;
	[SerializeField] private float smoothDampTime = 0.02f;
	[SerializeField] private bool extrapolatePosition = false;

	public enum UpdateType
	{
		Update,
		LateUpdate
	}
	[SerializeField] private UpdateType updateType;

	public enum SmoothType
	{
		Lerp,
		SmoothDamp, 
	}

	[SerializeField] private SmoothType smoothType;
	Vector3 localPositionOffset;
	Vector3 refVelocity;
		
	void Awake () 
	{	
		if(target == null)
			target = this.transform.parent;

		tr = transform;
		currentPosition = transform.position;
		localPositionOffset = tr.localPosition;
	}

	void OnEnable()
	{
		ResetCurrentPosition();
	}

	void Update () {
		if(updateType == UpdateType.LateUpdate)
			return;
		SmoothUpdate();
	}

	void LateUpdate () {
		if(updateType == UpdateType.Update)
			return;
		SmoothUpdate();
	}

	void SmoothUpdate()
	{
		currentPosition = Smooth (currentPosition, target.position, lerpSpeed);
		tr.position = currentPosition;
	}

	Vector3 Smooth(Vector3 _start, Vector3 _target, float _smoothTime)
	{
		Vector3 _offset = tr.localToWorldMatrix * localPositionOffset;

		if (extrapolatePosition) {
			Vector3 difference = _target - (_start - _offset);
			_target += difference;
		}

		_target += _offset;

		switch (smoothType)
		{
			case SmoothType.Lerp:
				return Vector3.Lerp (_start, _target, Time.deltaTime * _smoothTime);
			case SmoothType.SmoothDamp:
				return Vector3.SmoothDamp (_start, _target, ref refVelocity, smoothDampTime);
			default:
				return Vector3.zero;
		}
	}

	public void ResetCurrentPosition()
	{
		Vector3 _offset = tr.localToWorldMatrix * localPositionOffset;
		currentPosition = target.position + _offset;
	}
}
