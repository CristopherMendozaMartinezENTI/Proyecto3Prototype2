using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Clase para control por teclado
public class PlayerKeyboardInput : PlayerInput
{
	[SerializeField] private string horizontalInputAxis = "Horizontal";
	[SerializeField] private string verticalInputAxis = "Vertical";
	[SerializeField] private KeyCode jumpKey = KeyCode.Space;

	//Para usar los Raw inputs de unity o no
	[SerializeField] private bool useRawInput = true;

    public override float GetHorizontalMovementInput()
	{
		if(useRawInput)
			return Input.GetAxisRaw(horizontalInputAxis);
		else
			return Input.GetAxis(horizontalInputAxis);
	}

	public override float GetVerticalMovementInput()
	{
		if(useRawInput)
			return Input.GetAxisRaw(verticalInputAxis);
		else
			return Input.GetAxis(verticalInputAxis);
	}

	public override bool IsJumpKeyPressed()
	{
		return Input.GetKey(jumpKey);
	}
}

