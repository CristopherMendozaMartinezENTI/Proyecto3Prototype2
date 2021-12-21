using UnityEngine;
using System.Collections;

//Esta script controla todo lo relacionado con la deteccion de colisiones del player
public class PlayerColliderController : MonoBehaviour 
{
	[Header("Player Options")]
	[Range(0f, 1f)][SerializeField] private float stepHeightRatio = 0.25f;
	[Header("Collider Options")]
	[SerializeField] private float colliderHeight = 2f;
	[SerializeField] private float colliderThickness = 1f;
	[SerializeField] Vector3 colliderOffset = Vector3.zero;

	BoxCollider boxCollider;
	SphereCollider sphereCollider;
	CapsuleCollider capsuleCollider;

	[Header("Caster Options")]
	[SerializeField] private RayAndSphereCaster.CastType sensorType = RayAndSphereCaster.CastType.Raycast;
	private float sensorRadiusModifier = 0.8f;
	private int currentLayer;
	[SerializeField] private bool isInDebugMode = false;

	[HideInInspector] public Vector3[] raycastArrayPreviewPositions;

	bool isGrounded = false;

	bool IsUsingExtendedSensorRange  = true;
	float baseSensorRange = 0f;

	Vector3 currentGroundAdjustmentVelocity = Vector3.zero;

	Collider col;
	Rigidbody rig;
	Transform tr;
	RayAndSphereCaster caster;

	void Awake()
	{
		Setup();
		caster = new RayAndSphereCaster(this.tr, col);
		RecalculateColliderDimensions();
		RecalibrateSensor();
	}

	void Reset () {
		Setup();
	}

	void OnValidate()
	{
		if(this.gameObject.activeInHierarchy)
			RecalculateColliderDimensions();
	}

	void Setup()
	{
		tr = transform;
		col = GetComponent<Collider>();

		if(col == null)
		{
			tr.gameObject.AddComponent<CapsuleCollider>();
			col = GetComponent<Collider>();
		}

		rig = GetComponent<Rigidbody>();

		if(rig == null)
		{
			tr.gameObject.AddComponent<Rigidbody>();
			rig = GetComponent<Rigidbody>();
		}

		boxCollider = GetComponent<BoxCollider>();
		sphereCollider = GetComponent<SphereCollider>();
		capsuleCollider = GetComponent<CapsuleCollider>();

		rig.freezeRotation = true;
		rig.useGravity = false;
	}

	void LateUpdate()
	{
		if(isInDebugMode)
			caster.DrawDebug();
	}

	public void RecalculateColliderDimensions()
	{
		if(col == null)
		{
			Setup();
			if(col == null)
			{
				Debug.LogWarning("El siguiente objeto no tiene collider " + this.gameObject.name + "MADRE MIA KRIS!");
				return;
			}				
		}

		if(boxCollider)
		{
			Vector3 _size = Vector3.zero;
			_size.x = colliderThickness;
			_size.z = colliderThickness;

			boxCollider.center = colliderOffset * colliderHeight;

			_size.y = colliderHeight * (1f - stepHeightRatio);
			boxCollider.size = _size;

			boxCollider.center = boxCollider.center + new Vector3(0f, stepHeightRatio * colliderHeight/2f, 0f);
		}
		else if(sphereCollider)
		{
			sphereCollider.radius = colliderHeight/2f;
			sphereCollider.center = colliderOffset * colliderHeight;

			sphereCollider.center = sphereCollider.center + new Vector3(0f, stepHeightRatio * sphereCollider.radius, 0f);
			sphereCollider.radius *= (1f - stepHeightRatio);
		}
		else if(capsuleCollider)
		{
			capsuleCollider.height = colliderHeight;
			capsuleCollider.center = colliderOffset * colliderHeight;
			capsuleCollider.radius = colliderThickness/2f;

			capsuleCollider.center = capsuleCollider.center + new Vector3(0f, stepHeightRatio * capsuleCollider.height/2f, 0f);
			capsuleCollider.height *= (1f - stepHeightRatio);

			if(capsuleCollider.height/2f < capsuleCollider.radius)
				capsuleCollider.radius = capsuleCollider.height/2f;
		}

		if(caster != null)
			RecalibrateSensor();
	}

	void RecalibrateSensor()
	{
		caster.SetCastOrigin(GetColliderCenter());
		caster.SetCastDirection(RayAndSphereCaster.CastDirection.Down);
		RecalculateSensorLayerMask();
		caster.castType = sensorType;
		float _radius = colliderThickness/2f * sensorRadiusModifier;
		float _safetyDistanceFactor = 0.001f;

		if(boxCollider)
			_radius = Mathf.Clamp(_radius, _safetyDistanceFactor, (boxCollider.size.y/2f) * (1f - _safetyDistanceFactor));
		else if(sphereCollider)
			_radius = Mathf.Clamp(_radius, _safetyDistanceFactor, sphereCollider.radius * (1f - _safetyDistanceFactor));
		else if(capsuleCollider)
			_radius = Mathf.Clamp(_radius, _safetyDistanceFactor, (capsuleCollider.height/2f) * (1f - _safetyDistanceFactor));

		caster.sphereCastRadius = _radius * tr.localScale.x;
		float _length = 0f;
		_length += (colliderHeight * (1f - stepHeightRatio)) * 0.5f;
		_length += colliderHeight * stepHeightRatio;
		baseSensorRange = _length * (1f + _safetyDistanceFactor) * tr.localScale.x;
		caster.castLength = _length * tr.localScale.x;
		caster.isInDebugMode = isInDebugMode;
		caster.calculateRealDistance = true;
		caster.calculateRealSurfaceNormal = true;
	}


	void RecalculateSensorLayerMask()
	{
		int _layerMask = 0;
		int _objectLayer = this.gameObject.layer;

        for (int i = 0; i < 32; i++)
        {
            if (!Physics.GetIgnoreLayerCollision(_objectLayer, i)) 
				_layerMask = _layerMask | (1 << i);
		}

		if(_layerMask == (_layerMask | (1 << LayerMask.NameToLayer("Ignore Raycast"))))
		{
			_layerMask ^= (1 << LayerMask.NameToLayer("Ignore Raycast"));
		}

		caster.layermask = _layerMask;
		currentLayer = _objectLayer;
	}

	Vector3 GetColliderCenter()
	{
		if(col == null)
			Setup();

		return col.bounds.center;
	}

	void Check()
	{
		currentGroundAdjustmentVelocity = Vector3.zero;

		if(IsUsingExtendedSensorRange)
			caster.castLength = baseSensorRange + (colliderHeight * tr.localScale.x) * stepHeightRatio;
		else
			caster.castLength = baseSensorRange;

		caster.Cast();
		if(!caster.HasDetectedHit())
		{
			isGrounded = false;
			return;
		}

		isGrounded = true;
		float _distance = caster.GetDistance();
		float _upperLimit = ((colliderHeight * tr.localScale.x) * (1f - stepHeightRatio)) * 0.5f;
		float _middle = _upperLimit + (colliderHeight * tr.localScale.x) * stepHeightRatio;
		float _distanceToGo = _middle - _distance;
		currentGroundAdjustmentVelocity = tr.up * (_distanceToGo/Time.fixedDeltaTime);
	}

	public void CheckForGround()
	{
		if(currentLayer != this.gameObject.layer)
			RecalculateSensorLayerMask();
		Check();
	}

	public void SetVelocity(Vector3 _velocity)
	{
		rig.velocity = _velocity + currentGroundAdjustmentVelocity;	
	}	

	public bool IsGrounded()
	{
		return isGrounded;
	}

	public void SetExtendSensorRange(bool _isExtended)
	{
		IsUsingExtendedSensorRange = _isExtended;
	}

	public void SetColliderHeight(float _newColliderHeight)
	{
		if(colliderHeight == _newColliderHeight)
			return;

		colliderHeight = _newColliderHeight;
		RecalculateColliderDimensions();
	}

	public void SetColliderThickness(float _newColliderThickness)
	{
		if(colliderThickness == _newColliderThickness)
			return;

		if(_newColliderThickness < 0f)
			_newColliderThickness = 0f;

		colliderThickness = _newColliderThickness;
		RecalculateColliderDimensions();
	}

	public void SetStepHeightRatio(float _newStepHeightRatio)
	{
		_newStepHeightRatio = Mathf.Clamp(_newStepHeightRatio, 0f, 1f);
		stepHeightRatio = _newStepHeightRatio;
		RecalculateColliderDimensions();
	}

	public Vector3 GetGroundNormal()
	{
		return caster.GetNormal();
	}

	public Vector3 GetGroundPoint()
	{
		return caster.GetPosition();
	}

	public Collider GetGroundCollider()
	{
		return caster.GetCollider();
	}
}

