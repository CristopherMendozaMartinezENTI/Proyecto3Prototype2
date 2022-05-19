using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ESTO SE TIENE QUE BORRAR
using UnityEngine.SceneManagement;


////Esta script permite controlar el movimiento del player
public class PlayerMovementController : Controller 
{
	private CheckPointSystem cps;

	protected Transform tr;
	protected PlayerColliderController collCtrl;
	protected PlayerInput characterInput;

	private bool jumpInputIsLocked = false;
	private bool jumpKeyWasPressed = false;
	private bool jumpKeyWasLetGo = false;
	private bool jumpKeyIsPressed = false;

	[Header("Player Options")]
	[SerializeField] private float movementSpeed = 7f;
	[SerializeField] private float airControlRate = 2f;
	[SerializeField] private float jumpSpeed = 10f;
	[SerializeField] private float jumpDuration = 0.2f;
	float currentJumpStartTime = 0f;
	[SerializeField] private float airFriction = 0.5f;
	[SerializeField] private float groundFriction = 100f;

	protected Vector3 momentum = Vector3.zero;
	private Vector3 savedVelocity = Vector3.zero;
	private Vector3 savedMovementVelocity = Vector3.zero;

	[SerializeField] private float gravity = 30f;
	[SerializeField] private float slideGravity = 5f;
	[SerializeField] private float slopeLimit = 80f;
	[SerializeField] private bool useLocalMomentum = false;

	public enum ControllerState
	{
		Grounded,
		Sliding,
		Falling,
		Rising,
		Jumping
	}

	private ControllerState currentControllerState = ControllerState.Falling;

	[SerializeField] private Transform cameraTransform;

	private void Awake () {
		collCtrl = GetComponent<PlayerColliderController>();
		tr = transform;
		characterInput = GetComponent<PlayerInput>();
		if(characterInput == null)
			Debug.LogWarning("Al siguiente objeto le falta el script PlayerInput" + this.gameObject + "MADRE MIA KRIS!");
		Setup();
	}
    private void Start()
    {
		/*
		cps = GameObject.FindGameObjectWithTag("CPS").GetComponent<CheckPointSystem>();
		this.transform.position = cps.lastCheckPoint;
		this.transform.rotation = cps.lastRotation;
		*/
    }

    protected virtual void Setup(){}

	private void FixedUpdate()
	{
		ControllerUpdate();
	}

	private void Update()
	{
		HandleJumpKeyInput();
        
		//ESTO SE TIENE QUE BORRAR.
		if (Input.GetKeyDown(KeyCode.Keypad0))
        {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
	}

	private void ControllerUpdate()
	{
		collCtrl.CheckForGround();
		currentControllerState = DetermineControllerState();
		HandleMomentum();
		HandleJumping();

		Vector3 _velocity = Vector3.zero;
		if(currentControllerState == ControllerState.Grounded)
			_velocity = CalculateMovementVelocity();
			
		Vector3 _worldMomentum = momentum;
		if(useLocalMomentum)
			_worldMomentum = tr.localToWorldMatrix * momentum;

		_velocity += _worldMomentum;

		collCtrl.SetExtendSensorRange(IsGrounded());
		collCtrl.SetVelocity(_velocity);
		savedVelocity = _velocity;
		savedMovementVelocity = CalculateMovementVelocity();
		jumpKeyWasLetGo = false;
		jumpKeyWasPressed = false;
	}

	protected virtual Vector3 CalculateMovementDirection()
	{
		if(characterInput == null)
			return Vector3.zero;

		Vector3 _velocity = Vector3.zero;
		if(cameraTransform == null)
		{
			_velocity += tr.right * characterInput.GetHorizontalMovementInput();
			_velocity += tr.forward * characterInput.GetVerticalMovementInput();
		}
		else
		{
			_velocity += Vector3.ProjectOnPlane(cameraTransform.right, tr.up).normalized * characterInput.GetHorizontalMovementInput();
			_velocity += Vector3.ProjectOnPlane(cameraTransform.forward, tr.up).normalized * characterInput.GetVerticalMovementInput();
		}
		if(_velocity.magnitude > 1f)
			_velocity.Normalize();
		return _velocity;
	}

	protected virtual Vector3 CalculateMovementVelocity()
	{
		Vector3 _velocity = CalculateMovementDirection();
		_velocity *= movementSpeed;
		return _velocity;
	}

	private void HandleJumpKeyInput()
	{
		bool _newJumpKeyPressedState = IsJumpKeyPressed();
		if (jumpKeyIsPressed == false && _newJumpKeyPressedState == true)
			jumpKeyWasPressed = true;
		if (jumpKeyIsPressed == true && _newJumpKeyPressedState == false)
		{
			jumpKeyWasLetGo = true;
			jumpInputIsLocked = false;
		}
		jumpKeyIsPressed = _newJumpKeyPressedState;
	}

	protected virtual bool IsJumpKeyPressed()
	{
		if(characterInput == null)
			return false;
		return characterInput.IsJumpKeyPressed();
	}

	private ControllerState DetermineControllerState()
	{
		bool _isRising = IsRisingOrFalling() && (MathVector.GetDotProduct(GetMomentum(), tr.up) > 0f);
		bool _isSliding = collCtrl.IsGrounded() && IsGroundTooSteep();

		if(currentControllerState == ControllerState.Grounded)
		{
			if(_isRising){
				OnGroundContactLost();
				return ControllerState.Rising;
			}
			if(!collCtrl.IsGrounded()){
				OnGroundContactLost();
				return ControllerState.Falling;
			}
			if(_isSliding){
				OnGroundContactLost();
				return ControllerState.Sliding;
			}
			return ControllerState.Grounded;
		}

		if(currentControllerState == ControllerState.Falling)
		{
			if(_isRising){
				return ControllerState.Rising;
			}
			if(collCtrl.IsGrounded() && !_isSliding){
				OnGroundContactRegained();
				return ControllerState.Grounded;
			}
			if(_isSliding){
				return ControllerState.Sliding;
			}
			return ControllerState.Falling;
		}
	
		if(currentControllerState == ControllerState.Sliding)
		{	
			if(_isRising){
				OnGroundContactLost();
				return ControllerState.Rising;
			}
			if(!collCtrl.IsGrounded()){
				OnGroundContactLost();
				return ControllerState.Falling;
			}
			if(collCtrl.IsGrounded() && !_isSliding){
				OnGroundContactRegained();
				return ControllerState.Grounded;
			}
			return ControllerState.Sliding;
		}

		if(currentControllerState == ControllerState.Rising)
		{
			if(!_isRising){
				if(collCtrl.IsGrounded() && !_isSliding){
					OnGroundContactRegained();
					return ControllerState.Grounded;
				}
				if(_isSliding){
					return ControllerState.Sliding;
				}
				if(!collCtrl.IsGrounded()){
					return ControllerState.Falling;
				}
			}
			return ControllerState.Rising;
		}

		if(currentControllerState == ControllerState.Jumping)
		{
			if((Time.time - currentJumpStartTime) > jumpDuration)
				return ControllerState.Rising;

			if(jumpKeyWasLetGo)
				return ControllerState.Rising;

			return ControllerState.Jumping;
		}
		return ControllerState.Falling;
	}

	private void HandleJumping()
    {
        if (currentControllerState == ControllerState.Grounded)
        {
            if ((jumpKeyIsPressed == true || jumpKeyWasPressed) && !jumpInputIsLocked)
            {
                OnGroundContactLost();
                OnJumpStart();

                currentControllerState = ControllerState.Jumping;
            }
        }
    }

	private void HandleMomentum()
	{
		if(useLocalMomentum)
			momentum = tr.localToWorldMatrix * momentum;

		Vector3 _verticalMomentum = Vector3.zero;
		Vector3 _horizontalMomentum = Vector3.zero;

		if(momentum != Vector3.zero)
		{
			_verticalMomentum = MathVector.ExtractDotVector(momentum, tr.up);
			_horizontalMomentum = momentum - _verticalMomentum;
		}

		_verticalMomentum -= tr.up * gravity * Time.deltaTime;


		if(currentControllerState == ControllerState.Grounded && MathVector.GetDotProduct(_verticalMomentum, tr.up) < 0f)
			_verticalMomentum = Vector3.zero;


		if(!IsGrounded())
		{
			Vector3 _movementVelocity = CalculateMovementVelocity();


			if(_horizontalMomentum.magnitude > movementSpeed)
			{

				if(MathVector.GetDotProduct(_movementVelocity, _horizontalMomentum.normalized) > 0f)
					_movementVelocity = MathVector.RemoveDotVector(_movementVelocity, _horizontalMomentum.normalized);
					
				float _airControlMultiplier = 0.25f;
				_horizontalMomentum += _movementVelocity * Time.deltaTime * airControlRate * _airControlMultiplier;
			}

			else
			{

				_horizontalMomentum += _movementVelocity * Time.deltaTime * airControlRate;
				_horizontalMomentum = Vector3.ClampMagnitude(_horizontalMomentum, movementSpeed);
			}
		}

		if(currentControllerState == ControllerState.Sliding)
		{

			Vector3 _pointDownVector = Vector3.ProjectOnPlane(collCtrl.GetGroundNormal(), tr.up).normalized;
			Vector3 _slopeMovementVelocity = CalculateMovementVelocity();
			_slopeMovementVelocity = MathVector.RemoveDotVector(_slopeMovementVelocity, _pointDownVector);
			_horizontalMomentum += _slopeMovementVelocity * Time.fixedDeltaTime;
		}

		if(currentControllerState == ControllerState.Grounded)
			_horizontalMomentum = MathVector.IncrementVectorTowardTargetVector(_horizontalMomentum, groundFriction, Time.deltaTime, Vector3.zero);
		else
			_horizontalMomentum = MathVector.IncrementVectorTowardTargetVector(_horizontalMomentum, airFriction, Time.deltaTime, Vector3.zero); 

		momentum = _horizontalMomentum + _verticalMomentum;

		if(currentControllerState == ControllerState.Sliding)
		{
			momentum = Vector3.ProjectOnPlane(momentum, collCtrl.GetGroundNormal());
			if(MathVector.GetDotProduct(momentum, tr.up) > 0f)
				momentum = MathVector.RemoveDotVector(momentum, tr.up);
			Vector3 _slideDirection = Vector3.ProjectOnPlane(-tr.up, collCtrl.GetGroundNormal()).normalized;
			momentum += _slideDirection * slideGravity * Time.deltaTime;
		}
			
		if(currentControllerState == ControllerState.Jumping)
		{
			momentum = MathVector.RemoveDotVector(momentum, tr.up);
			momentum += tr.up * jumpSpeed;
		}

		if(useLocalMomentum)
			momentum = tr.worldToLocalMatrix * momentum;
	}

	private void OnJumpStart()
	{
		if(useLocalMomentum)
			momentum = tr.localToWorldMatrix * momentum;
		momentum += tr.up * jumpSpeed;
		currentJumpStartTime = Time.time;
        jumpInputIsLocked = true;

        if (OnJump != null)
			OnJump(momentum);
		if(useLocalMomentum)
			momentum = tr.worldToLocalMatrix * momentum;
	}

	private void OnGroundContactLost()
	{
		if(useLocalMomentum)
			momentum = tr.localToWorldMatrix * momentum;

		Vector3 _velocity = GetMovementVelocity();
		if(_velocity.sqrMagnitude >= 0f && momentum.sqrMagnitude > 0f)
		{
			Vector3 _projectedMomentum = Vector3.Project(momentum, _velocity.normalized);
			float _dot = MathVector.GetDotProduct(_projectedMomentum.normalized, _velocity.normalized);
			if(_projectedMomentum.sqrMagnitude >= _velocity.sqrMagnitude && _dot > 0f)
				_velocity = Vector3.zero;
			else if(_dot > 0f)
				_velocity -= _projectedMomentum;	
		}
		momentum += _velocity;

		if(useLocalMomentum)
			momentum = tr.worldToLocalMatrix * momentum;
	}

	private void OnGroundContactRegained()
	{
		if(OnLand != null)
		{
			Vector3 _collisionVelocity = momentum;
			if(useLocalMomentum)
				_collisionVelocity = tr.localToWorldMatrix * _collisionVelocity;

			OnLand(_collisionVelocity);
		}
				
	}

	private bool IsRisingOrFalling()
	{
		Vector3 _verticalMomentum = MathVector.ExtractDotVector(GetMomentum(), tr.up);
		float _limit = 0.001f;
		return(_verticalMomentum.magnitude > _limit);
	}

	private bool IsGroundTooSteep()
	{
		if(!collCtrl.IsGrounded())
			return true;

		return (Vector3.Angle(collCtrl.GetGroundNormal(), tr.up) > slopeLimit);
	}

	public override Vector3 GetVelocity ()
	{
		return savedVelocity;
	}

	public override Vector3 GetMovementVelocity()
	{
		return savedMovementVelocity;
	}

	public Vector3 GetMomentum()
	{
		Vector3 _worldMomentum = momentum;
		if(useLocalMomentum)
			_worldMomentum = tr.localToWorldMatrix * momentum;

		return _worldMomentum;
	}

	public override bool IsGrounded()
	{
		return(currentControllerState == ControllerState.Grounded || currentControllerState == ControllerState.Sliding);
	}

	public bool IsSliding()
	{
		return(currentControllerState == ControllerState.Sliding);
	}

	public void AddMomentum (Vector3 _momentum)
	{
		if(useLocalMomentum)
			momentum = tr.localToWorldMatrix * momentum;

		momentum += _momentum;	

		if(useLocalMomentum)
			momentum = tr.worldToLocalMatrix * momentum;
	}

	public void SetMomentum(Vector3 _newMomentum)
	{
		if(useLocalMomentum)
			momentum = tr.worldToLocalMatrix * _newMomentum;
		else
			momentum = _newMomentum;
	}
}
