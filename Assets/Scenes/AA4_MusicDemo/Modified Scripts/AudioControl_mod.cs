using UnityEngine;

//Script para los sonidos del player
public class AudioControl_mod : MonoBehaviour 
{
	private Controller controller;
	private Animator animator;
	private Controller mover;
	private Transform tr;

	//Esto es para cuando el player tenga animaciones
	[SerializeField] private bool useAnimationBasedFootsteps = true;

	[SerializeField] private float landVelocityThreshold = 5f;
	[SerializeField] private float footstepDistance = 0.2f;
	private float currentFootstepDistance = 0f;

	private float currentFootStepValue = 0f;

	[Range(0f, 1f)]
	[SerializeField] private float audioClipVolume = 0.1f;
	[SerializeField] private float relativeRandomizedVolumeRange = 0.2f;

	private void Start ()
	{
		controller = GetComponent<Controller>();
		animator = GetComponentInChildren<Animator>();
		mover = GetComponent<Controller>();
		tr = transform;
		controller.OnLand += OnLand;
		controller.OnJump += OnJump;
		if(!animator)
			useAnimationBasedFootsteps = false;
	}

	private void Update ()
	{

		Vector3 _velocity = controller.GetVelocity();
		Vector3 _horizontalVelocity = MathVector.RemoveDotVector(_velocity, tr.up);
		FootStepUpdate(_horizontalVelocity.magnitude);
	}

	private void FootStepUpdate(float _movementSpeed)
	{
		float _speedThreshold = 0.05f;
		if(useAnimationBasedFootsteps)
		{
			//Cogemos las animaciones para reproducir los pasos
			float _newFootStepValue = animator.GetFloat("FootStep");
			if((currentFootStepValue <= 0f && _newFootStepValue > 0f) || (currentFootStepValue >= 0f && _newFootStepValue < 0f))
			{
				if(tag == "Dirt")
                {
						AkSoundEngine.SetSwitch("Footsteps", "Dirt", gameObject);
                }
				else if (tag == "Wooden")
                {
						AkSoundEngine.SetSwitch("Footsteps", "Wood", gameObject);
                }
				else if (tag == "Grass")
                {
						AkSoundEngine.SetSwitch("Footsteps", "Grass", gameObject);
                }
					
				if (mover.IsGrounded() && _movementSpeed > _speedThreshold)
				{
					AkSoundEngine.PostEvent("Play_Footsteps", gameObject);
				}
			}
			currentFootStepValue = _newFootStepValue;
		}
		else
		{
			currentFootstepDistance += Time.deltaTime * _movementSpeed;
			if(currentFootstepDistance > footstepDistance)
			{
				if(mover.IsGrounded() && _movementSpeed > _speedThreshold)
					//AQUI TIENEN QUE HABER CAMBIOS			PlayFootstepSound(_movementSpeed);
					currentFootstepDistance = 0f;
			}
		}
	}

	private void OnLand(Vector3 _v)
	{
		if(MathVector.GetDotProduct(_v, tr.up) > -landVelocityThreshold)
			return;
		AkSoundEngine.PostEvent("Play_Land", gameObject);
	}

	private void OnJump(Vector3 _v)
	{
		AkSoundEngine.PostEvent("Play_Jump", gameObject);
	}
}


