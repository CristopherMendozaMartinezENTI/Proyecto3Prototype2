using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script para los sonidos del player
public class AudioControl : MonoBehaviour 
{
	Controller controller;
	Animator animator;
	Controller mover;
	Transform tr;
	[SerializeField] private AudioSource audioSource;

	//Esto es para cuando el player tenga animaciones
	[SerializeField] private bool useAnimationBasedFootsteps = true;

	[SerializeField] private float landVelocityThreshold = 5f;
	[SerializeField] private float footstepDistance = 0.2f;
	float currentFootstepDistance = 0f;

	private float currentFootStepValue = 0f;

	[Range(0f, 1f)]
	[SerializeField] private float audioClipVolume = 0.1f;
	[SerializeField] private float relativeRandomizedVolumeRange = 0.2f;

	[SerializeField] private AudioClip[] footStepClips;
	[SerializeField] private AudioClip jumpClip;
	[SerializeField] private AudioClip landClip;

	void Start ()
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
		
	void Update ()
	{

		Vector3 _velocity = controller.GetVelocity();
		Vector3 _horizontalVelocity = MathVector.RemoveDotVector(_velocity, tr.up);
		FootStepUpdate(_horizontalVelocity.magnitude);
	}

	void FootStepUpdate(float _movementSpeed)
	{
		float _speedThreshold = 0.05f;
		if(useAnimationBasedFootsteps)
		{
			//Cogemos las animaciones para reproducir los pasos
			float _newFootStepValue = animator.GetFloat("FootStep");
			if((currentFootStepValue <= 0f && _newFootStepValue > 0f) || (currentFootStepValue >= 0f && _newFootStepValue < 0f))
			{
				if(mover.IsGrounded() && _movementSpeed > _speedThreshold)
					PlayFootstepSound(_movementSpeed);
			}
			currentFootStepValue = _newFootStepValue;
		}
		else
		{
			currentFootstepDistance += Time.deltaTime * _movementSpeed;
			if(currentFootstepDistance > footstepDistance)
			{
				if(mover.IsGrounded() && _movementSpeed > _speedThreshold)
					PlayFootstepSound(_movementSpeed);
				currentFootstepDistance = 0f;
			}
		}
	}

	void PlayFootstepSound(float _movementSpeed)
	{
		int _footStepClipIndex = Random.Range(0, footStepClips.Length);
		audioSource.PlayOneShot(footStepClips[_footStepClipIndex], audioClipVolume + audioClipVolume * Random.Range(-relativeRandomizedVolumeRange, relativeRandomizedVolumeRange));
	}

	void OnLand(Vector3 _v)
	{
		if(MathVector.GetDotProduct(_v, tr.up) > -landVelocityThreshold)
			return;
		audioSource.PlayOneShot(landClip, audioClipVolume);
	}

	void OnJump(Vector3 _v)
	{
		audioSource.PlayOneShot(jumpClip, audioClipVolume);
	}
}


