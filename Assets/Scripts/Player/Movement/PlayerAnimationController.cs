using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private GameObject playerController;
    private TelekinesisController tkc;
    private bool telequinesis = false;

    private void Start()
    {
        playerController = GameObject.Find("Player");
        tkc = GameObject.Find("TelekinesisGaunlet").GetComponent<TelekinesisController>();
    }

    private void Update()
    {
        if (tkc.usingTelequinesis)
            telequinesis = true;
        else if (!tkc.usingTelequinesis)
            telequinesis = false;


        if (Input.GetKey(KeyCode.W))
        {
            if (telequinesis)
                gameObject.GetComponent<Animator>().Play("Run Telekinesis");
            else
                gameObject.GetComponent<Animator>().Play("Run");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (telequinesis)
                gameObject.GetComponent<Animator>().Play("RunBackwards Telekinesis");
            else
                gameObject.GetComponent<Animator>().Play("RunBackwards");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            gameObject.GetComponent<Animator>().Play("RunRight");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            gameObject.GetComponent<Animator>().Play("RunLeft");
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            gameObject.GetComponent<Animator>().Play("Jump");
        }
        else if (playerController.GetComponent<Controller>().GetVelocity() == Vector3.zero)
        {
            gameObject.GetComponent<Animator>().Play("Iddle");
        }
    }
}
