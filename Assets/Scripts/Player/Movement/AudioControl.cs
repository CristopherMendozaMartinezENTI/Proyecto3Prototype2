using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script para los sonidos del player
public class AudioControl : MonoBehaviour 
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

	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip[] footStepClips;
	[SerializeField] private AudioClip jumpClip;
	[SerializeField] private AudioClip landClip;

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

	private void PlayFootstepSound(float _movementSpeed)
	{
		int _footStepClipIndex = Random.Range(0, footStepClips.Length);
		audioSource.PlayOneShot(footStepClips[_footStepClipIndex], audioClipVolume + audioClipVolume * Random.Range(-relativeRandomizedVolumeRange, relativeRandomizedVolumeRange));
	}

	private void OnLand(Vector3 _v)
	{
		if(MathVector.GetDotProduct(_v, tr.up) > -landVelocityThreshold)
			return;
		audioSource.PlayOneShot(landClip, audioClipVolume);

		/*if (defCollider == true)
		{
			Debug.Log("TRUE");
		}
		else 
		{
			audioSource.PlayOneShot(landClip, audioClipVolume);
			Debug.Log("FALSE");
		}*/
	}

	private void OnJump(Vector3 _v)
	{
		audioSource.PlayOneShot(jumpClip, audioClipVolume);
	}
}


