using UnityEngine;
using System.Collections;

//Este script permite suavizar la rotacion de cualquier objeto mediante Lerp o Damping 
public class SmoothRotation : MonoBehaviour
{
	Transform tr;
	Quaternion currentRotation;
	[SerializeField] private Transform target;
	[SerializeField] private float smoothSpeed = 20f;
	[SerializeField] private bool extrapolateRotation = false;

	public enum UpdateType
	{
		Update,
		LateUpdate
	}
	[SerializeField] private UpdateType updateType;

	private void Awake () 
	{
		if(target == null)
			target = this.transform.parent;

		tr = transform;
		currentRotation = transform.rotation;
	}

	private void OnEnable()
	{
		ResetCurrentRotation();
	}

	private void Update () {
		if(updateType == UpdateType.LateUpdate)
			return;
		SmoothUpdate();
	}

	private void LateUpdate () {
		if(updateType == UpdateType.Update)
			return;
		SmoothUpdate();
	}

	private void SmoothUpdate()
	{
		currentRotation = Smooth (currentRotation, target.rotation, smoothSpeed);
		tr.rotation = currentRotation;
	}

	private Quaternion Smooth(Quaternion _currentRotation, Quaternion _targetRotation, float _smoothSpeed)
	{
		if (extrapolateRotation && Quaternion.Angle(_currentRotation, _targetRotation) < 90f) {
			Quaternion difference = _targetRotation * Quaternion.Inverse (_currentRotation);
			_targetRotation *= difference;
		}
		return Quaternion.Slerp (_currentRotation, _targetRotation, Time.deltaTime * _smoothSpeed);
	}

	public void ResetCurrentRotation()
	{
		currentRotation = target.rotation;
	}
								
}

