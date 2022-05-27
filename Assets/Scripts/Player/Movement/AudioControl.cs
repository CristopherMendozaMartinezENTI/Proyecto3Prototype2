using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script para los sonidos del player
public class AudioControl : MonoBehaviour 
{
	[SerializeField] private AudioSource audioSource;

	private Controller controller;
	private Animator animator;
	private Controller mover;
	private Transform tr;

	public string floorTag;

	//Esto es para cuando el player tenga animaciones
	[SerializeField] private bool useAnimationBasedFootsteps = true;

	[SerializeField] private float landVelocityThreshold = 5f;
	[SerializeField] private float footstepDistance = 0.2f;
	private float currentFootstepDistance = 0f;

	private float currentFootStepValue = 0f;

	[Range(0f, 1f)]
	[SerializeField] private float audioClipVolume = 0.1f;
	[SerializeField] private float relativeRandomizedVolumeRange = 0.2f;

	[SerializeField] private AudioClip[] footStepClips;
	[SerializeField] private AudioClip[] footMetalStepClips;
	[SerializeField] private AudioClip[] footRugStepClips;
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


		RaycastHit hit;
		if (Physics.Raycast((transform.position), transform.up * -1, out hit))
		{
			//This is the colliders tag
			floorTag = hit.collider.tag;
		}
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
		switch (floorTag)
		{

			case "RugGround":
				audioSource.PlayOneShot(footRugStepClips[_footStepClipIndex], audioClipVolume + audioClipVolume * Random.Range(-relativeRandomizedVolumeRange, relativeRandomizedVolumeRange));
				break;
			case "MetalGround":
				audioSource.PlayOneShot(footMetalStepClips[_footStepClipIndex], audioClipVolume + audioClipVolume * Random.Range(-relativeRandomizedVolumeRange, relativeRandomizedVolumeRange));
				break;
			case "Untagged":
				audioSource.PlayOneShot(footStepClips[_footStepClipIndex], audioClipVolume + audioClipVolume * Random.Range(-relativeRandomizedVolumeRange, relativeRandomizedVolumeRange));
				break;

		}
	}

	private void OnLand(Vector3 _v)
	{
		if(MathVector.GetDotProduct(_v, tr.up) > -landVelocityThreshold)
			return;
		audioSource.PlayOneShot(landClip, audioClipVolume);
	}

	private void OnJump(Vector3 _v)
	{
		audioSource.PlayOneShot(jumpClip, audioClipVolume);
	}
}


