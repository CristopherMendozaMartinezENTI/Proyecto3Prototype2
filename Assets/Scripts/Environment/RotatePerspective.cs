using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Este script permite cambiar la direccion respecto la se aplica la gravedad al los controllers que colisionen con el boxCollider que lo contenga
//Creando el efecto de que el escenario se ha rotado sobre su eje
public class RotatePerspective : MonoBehaviour
{
	private Transform tr;
	private AudioSource audioSource;
	private GameObject telekinesisController;
	[SerializeField] private bool isField = false;

	private void Start()
	{
		tr = transform;
		audioSource = GetComponent<AudioSource>();
		telekinesisController = GameObject.Find("TelekinesisGaunlet");
	}

	private void OnTriggerEnter(Collider col)
	{
		if(col.GetComponent<Controller>() == null)
			return;

		SwitchDirection(tr.forward, col.GetComponent<Controller>());
	}

	private void SwitchDirection(Vector3 _newUpDirection, Controller _controller)
	{
		float _angleThreshold = 0.001f;
		float _angleBetweenUpDirections = Vector3.Angle(_newUpDirection, _controller.transform.up);
		if(_angleBetweenUpDirections < _angleThreshold)
			return;

		audioSource.Play();
		
		if(isField)
			_controller.transform.position = tr.position;

		telekinesisController.GetComponent<TelekinesisController>().SetDistanceToMin();

		Transform _transform = _controller.transform;
		Quaternion _rotationDifference = Quaternion.FromToRotation(_transform.up, _newUpDirection);
		_transform.rotation = _rotationDifference * _transform.rotation;
	}
}
