using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Este script permite controlar la Camera realizaon rotaciones del gameobject que lo contiene
public class CameraController : MonoBehaviour 
{
	[Range(0f, 90f)]
	[SerializeField] private float upperVerticalLimit = 60f;
	[Range(0f, 90f)]
	[SerializeField] private float lowerVerticalLimit = 60f;

	private float currentXAngle = 0f;
	private float currentYAngle = 0f;
	private float oldHorizontalInput = 0f;
	private float oldVerticalInput = 0f;
	private bool isLocked = false;

	[SerializeField] private float cameraSpeed = 250f;
	[SerializeField] private bool smoothCameraRotation = false;
	[Range(1f, 50f)]
	[SerializeField] private float cameraSmoothingFactor = 25f;

	private Vector3 facingDirection;
	private Vector3 upwardsDirection;

	protected Transform tr;
	protected Camera cam;
	protected CameraInput cameraInput;

	void Awake () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		tr = transform;
		cam = GetComponent<Camera>();
		cameraInput = GetComponent<CameraInput>();

		if(cameraInput == null)
			Debug.LogWarning("OBJETO NO CONTIENE CAMARA. MADREMIA KRIS", this.gameObject);

		if(cam == null)
			cam = GetComponentInChildren<Camera>();

		currentXAngle = tr.localRotation.eulerAngles.x;
		currentYAngle = tr.localRotation.eulerAngles.y;
		RotateCamera(0f, 0f);
		Setup();
	}

	protected virtual void Setup(){}

	private void Update()
	{
		HandleCameraRotation();
	}

	public void LockRotation(bool isLockedNow)
    {
		isLocked = isLockedNow;
	}

	protected virtual void HandleCameraRotation()
	{
		if(isLocked) { return; }

		if(cameraInput == null)
			return;

		float _inputHorizontal = cameraInput.GetHorizontalCameraInput();
		float _inputVertical = cameraInput.GetVerticalCameraInput();
		RotateCamera(_inputHorizontal, _inputVertical);
	}

	protected void RotateCamera(float _newHorizontalInput, float _newVerticalInput)
	{
		if(smoothCameraRotation)
		{
			oldHorizontalInput = Mathf.Lerp (oldHorizontalInput, _newHorizontalInput, Time.deltaTime * cameraSmoothingFactor);
			oldVerticalInput = Mathf.Lerp (oldVerticalInput, _newVerticalInput, Time.deltaTime * cameraSmoothingFactor);
		}
		else
		{
			oldHorizontalInput = _newHorizontalInput;
			oldVerticalInput = _newVerticalInput;
		}

		currentXAngle += oldVerticalInput * cameraSpeed * Time.deltaTime;
		currentYAngle += oldHorizontalInput * cameraSpeed * Time.deltaTime;
		currentXAngle = Mathf.Clamp(currentXAngle, -upperVerticalLimit, lowerVerticalLimit);
		UpdateRotation();
	}

	protected void UpdateRotation()
	{
		tr.localRotation = Quaternion.Euler(new Vector3(0, currentYAngle, 0));
		facingDirection = tr.forward;
		upwardsDirection = tr.up;
		tr.localRotation = Quaternion.Euler(new Vector3(currentXAngle, currentYAngle, 0));
	}

	public void SetFOV(float _fov)
	{
		if(cam)
			cam.fieldOfView = _fov;	
	}

	public void SetRotationAngles(float _xAngle, float _yAngle)
	{
		currentXAngle = _xAngle;
		currentYAngle = _yAngle;
		UpdateRotation();
	}

	public void RotateTowardPosition(Vector3 _position, float _lookSpeed)
	{
		Vector3 _direction = (_position - tr.position);
		RotateTowardDirection(_direction, _lookSpeed);
	}

	public void RotateTowardDirection(Vector3 _direction, float _lookSpeed)
	{
		_direction.Normalize();
		_direction = tr.parent.InverseTransformDirection(_direction);
		Vector3 _currentLookVector = GetAimingDirection();
		_currentLookVector = tr.parent.InverseTransformDirection(_currentLookVector);
		float _xAngleDifference = MathVector.GetAngle(new Vector3(0f, _currentLookVector.y, 1f), new Vector3(0f, _direction.y, 1f), Vector3.right);
		_currentLookVector.y = 0f;
		_direction.y = 0f;
		float _yAngleDifference = MathVector.GetAngle(_currentLookVector, _direction, Vector3.up);
		Vector2 _currentAngles = new Vector2(currentXAngle, currentYAngle);
		Vector2 _angleDifference = new Vector2(_xAngleDifference, _yAngleDifference);
		float _angleDifferenceMagnitude = _angleDifference.magnitude;

		if(_angleDifferenceMagnitude == 0f)
			return;
		Vector2 _angleDifferenceDirection = _angleDifference/_angleDifferenceMagnitude;

		if(_lookSpeed * Time.deltaTime > _angleDifferenceMagnitude)
		{
			_currentAngles += _angleDifferenceDirection * _angleDifferenceMagnitude;
		}
		else
			_currentAngles += _angleDifferenceDirection * _lookSpeed * Time.deltaTime;

		currentYAngle = _currentAngles.y;
		currentXAngle = Mathf.Clamp(_currentAngles.x, -upperVerticalLimit, lowerVerticalLimit);
		UpdateRotation();
	}

	public float GetCurrentXAngle()
	{
		return currentXAngle;
	}

	public float GetCurrentYAngle()
	{
		return currentYAngle;
	}

	public Vector3 GetFacingDirection()
	{
		return facingDirection;
	}

	public Vector3 GetUpDirection()
	{
		return upwardsDirection;
	}

	public Vector3 GetAimingDirection ()
	{
		return tr.forward;
	}

	public Vector3 GetStrafeDirection ()
	{
		return tr.right;
	}
}

