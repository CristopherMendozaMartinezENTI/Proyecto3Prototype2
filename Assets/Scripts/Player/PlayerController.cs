using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform playerCam;
    [SerializeField] private Transform orientation;
    private Rigidbody rb;

    //Rotation and look
    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;

    [Header("Movement Variables")]
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private float moveSpeed = 4500;
    [SerializeField] private float maxSpeed = 20;
    [SerializeField] private float startMaxSpeed;
    [SerializeField] private float counterMovement = 0.175f;
    [SerializeField] private float threshold = 0.01f;
    [SerializeField] private float maxSlopeAngle = 35f;
    private bool grounded;

    [Header("Slide Variables")]
    [SerializeField] private float slideForce = 400;
    [SerializeField] private float slideCounterMovement = 0.2f;
    [SerializeField] private float crouchGravityMultiplier;
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;

    [Header("Jumping Variables")]
    [SerializeField] private float jumpForce = 550f;
    [SerializeField] private int startDoubleJumps = 1;
    private int doubleJumpsLeft;
    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;

    [Header("Wallrunning Variables")]
    [SerializeField] private LayerMask WallLayer;
    [SerializeField] private float wallrunForce, maxWallrunTime, maxWallSpeed;
    [SerializeField] private float maxWallRunCameraTilt;
    private float wallRunCameraTilt;
    private bool isWallRight, isWallLeft, isWallRunning;

    //Input Varibles
    private float x, y;
    private bool jumping, sprinting, crouching;

    [Header("Air Acceleration Variables")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashTime;
    private bool allowDashForceCounter;
    private bool readyToRun;
    private int wTapTimes = 0;
    private Vector3 dashStartVector;

    //Sliding
    private Vector3 normalVector = Vector3.up;

    [Header("Acceleration Variables")]
    [SerializeField] private float maxAcceleration;
    [SerializeField] private float accelerationForce;
    [SerializeField] private float timeBetweenMaxSpeed;
    private float timePassedOnMaxSpeed;

    [Header("Climbling Variables")]
    [SerializeField] private LayerMask LadderLayer;
    [SerializeField] private float climbForce, maxClimbSpeed;
    private bool alreadyStoppedAtLadder;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startMaxSpeed = maxSpeed;
        playerScale = transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        movementUpdate();
    }

    private void Update()
    {
        getInputs();
        lookAround();
        checkForWall();
        buildSpeed();
        wallRunInput();
    }

    private void getInputs()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
        crouching = Input.GetKey(KeyCode.LeftControl);

        //Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
            startCrouch();
        if (Input.GetKeyUp(KeyCode.LeftControl))
            stopCrouch();

        //Double Jumping
        if (Input.GetButtonDown("Jump") && !grounded && doubleJumpsLeft >= 1)
        {
            jump();
            doubleJumpsLeft--;
        }

        //Runing
        if (Input.GetKeyDown(KeyCode.W) && wTapTimes <= 1)
        {
            wTapTimes++;
            Invoke("ResetTapTimes", 0.3f);
        }
        if (wTapTimes == 2 && readyToRun) run();

        //Climbing
        if (Physics.Raycast(transform.position, orientation.forward, 1, LadderLayer) && y > .9f)
            climb();
        else alreadyStoppedAtLadder = false;

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            Time.timeScale = 0.5f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
        }
    }

    private void resetTapTimes()
    {
        wTapTimes = 0;
    }

    private void startCrouch()
    {
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (rb.velocity.magnitude > 0.5f)
        {
            if (grounded)
            {
                rb.AddForce(orientation.transform.forward * slideForce);
            }
        }
    }

    private void stopCrouch()
    {
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private void movementUpdate()
    {
        //Extra gravity
        //Needed that the Ground Check works better!
        float gravityMultiplier = 10f;

        if (crouching) gravityMultiplier = crouchGravityMultiplier;

        rb.AddForce(Vector3.down * Time.deltaTime * gravityMultiplier);

        //Find actual velocity relative to where player is looking
        Vector2 mag = findVelocityRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //Counteract sliding and sloppy movement
        counterMovementUpdate(x, y, mag);

        //If holding jump && ready to jump, then jump
        if (readyToJump && jumping && grounded) jump();

        //ResetStuff when touching ground
        if (grounded)
        {
            readyToRun = true;
            doubleJumpsLeft = startDoubleJumps;
        }

        //Set max speed
        float maxSpeed = this.maxSpeed;

        //If sliding down a ramp, add force down so player stays grounded and also builds speed
        if (crouching && grounded && readyToJump)
        {
            rb.AddForce(Vector3.down * Time.deltaTime * 3000);
            return;
        }

        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        //Some multipliers
        float multiplier = 1f, multiplierV = 1f;

        // Movement in air
        if (!grounded)
        {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }

        // Movement while sliding
        if (grounded && crouching) multiplierV = 0f;

        //Apply forces to move player
        rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);
    }

    private void jump()
    {
        if (grounded)
        {
            readyToJump = false;

            //Add jump forces
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0)
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(resetJump), jumpCooldown);
        }
        if (!grounded)
        {
            readyToJump = false;

            //Add jump forces
            rb.AddForce(orientation.forward * jumpForce * 1f);
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            //Reset Velocity
            rb.velocity = Vector3.zero;

            //Disable dashForceCounter if doublejumping while dashing
            allowDashForceCounter = false;

            Invoke(nameof(resetJump), jumpCooldown);
        }

        //Walljump
        if (isWallRunning)
        {
            readyToJump = false;

            //normal jump
            if (isWallLeft && !Input.GetKey(KeyCode.D) || isWallRight && !Input.GetKey(KeyCode.A))
            {
                rb.AddForce(Vector2.up * jumpForce * 1.5f);
                rb.AddForce(normalVector * jumpForce * 0.5f);
            }

            //sidwards wallhop
            if (isWallRight || isWallLeft && Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) rb.AddForce(-orientation.up * jumpForce * 1f);
            if (isWallRight && Input.GetKey(KeyCode.A)) rb.AddForce(-orientation.right * jumpForce * 3.2f);
            if (isWallLeft && Input.GetKey(KeyCode.D)) rb.AddForce(orientation.right * jumpForce * 3.2f);

            //Always add forward force
            rb.AddForce(orientation.forward * jumpForce * 1f);

            //Disable dashForceCounter if doublejumping while dashing
            allowDashForceCounter = false;

            Invoke(nameof(resetJump), jumpCooldown);
        }
    }

    private void resetJump()
    {
        readyToJump = true;
    }

    private void wallRunInput() //make sure to call in void Update
    {
        //Wallrun
        if (Input.GetKey(KeyCode.D) && isWallRight) startWallrun();
        if (Input.GetKey(KeyCode.A) && isWallLeft) startWallrun();
    }

    private void startWallrun()
    {
        rb.useGravity = false;
        isWallRunning = true;
        allowDashForceCounter = false;

        if (rb.velocity.magnitude <= maxWallSpeed)
        {
            rb.AddForce(orientation.forward * wallrunForce * Time.deltaTime);

            //Make sure char sticks to wall
            if (isWallRight)
                rb.AddForce(orientation.right * wallrunForce / 5 * Time.deltaTime);
            else
                rb.AddForce(-orientation.right * wallrunForce / 5 * Time.deltaTime);
        }
    }

    private void stopWallRun()
    {
        isWallRunning = false;
        rb.useGravity = true;
    }

    private void checkForWall() //make sure to call in void Update
    {
        isWallRight = Physics.Raycast(transform.position, orientation.right, 1f, WallLayer);
        isWallLeft = Physics.Raycast(transform.position, -orientation.right, 1f, WallLayer);

        //leave wall run
        if (!isWallLeft && !isWallRight) stopWallRun();
        //reset double jump (if you have one :D)
        if (isWallLeft || isWallRight) doubleJumpsLeft = startDoubleJumps;
    }

    private void run()
    {
        //saves current velocity
        dashStartVector = orientation.forward;

        allowDashForceCounter = true;

        readyToRun = false;
        wTapTimes = 0;

        //Deactivate gravity
        rb.useGravity = false;

        //Add force
        rb.velocity = Vector3.zero;
        rb.AddForce(orientation.forward * dashForce);

        Invoke("activateGravity", dashTime);
    }

    private void activateGravity()
    {
        rb.useGravity = true;

        //Counter currentForce
        if (allowDashForceCounter)
        {
            rb.AddForce(dashStartVector * -dashForce * 0.5f);
        }
    }

    private void buildSpeed()
    {
        //If running builds up speed
        if (grounded && y >= 0.99f)
        {
            timePassedOnMaxSpeed += Time.deltaTime;
        }
        else
        {
            timePassedOnMaxSpeed = 0;
            maxSpeed = startMaxSpeed;
        }

        if (timePassedOnMaxSpeed >= timeBetweenMaxSpeed)
        {
            if (maxSpeed <= maxAcceleration)
            {
                maxSpeed += 5;
                rb.AddForce(orientation.forward * Time.deltaTime * accelerationForce);
            }
            timePassedOnMaxSpeed = 0;
        }
    }
   
    private void climb()
    {
        //Makes possible to climb even when falling down fast
        Vector3 vel = rb.velocity;
        if (rb.velocity.y < 0.5f && !alreadyStoppedAtLadder)
        {
            rb.velocity = new Vector3(vel.x, 0, vel.z);
            //Make sure char get's at wall
            alreadyStoppedAtLadder = true;
            rb.AddForce(orientation.forward * 500 * Time.deltaTime);
        }

        //Push character up
        if (rb.velocity.magnitude < maxClimbSpeed)
            rb.AddForce(orientation.up * climbForce * Time.deltaTime);

        //Doesn't Push into the wall
        if (!Input.GetKey(KeyCode.S)) y = 0;
    }

    private void lookAround()
    {
        float desiredX;
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, wallRunCameraTilt);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);

        //While Wallrunning
        //Tilts camera in .5 second
        if (Math.Abs(wallRunCameraTilt) < maxWallRunCameraTilt && isWallRunning && isWallRight)
            wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * 2;
        if (Math.Abs(wallRunCameraTilt) < maxWallRunCameraTilt && isWallRunning && isWallLeft)
            wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * 2;

        //Tilts camera back again
        if (wallRunCameraTilt > 0 && !isWallRight && !isWallLeft)
            wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * 2;
        if (wallRunCameraTilt < 0 && !isWallRight && !isWallLeft)
            wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * 2;
    }

    private void counterMovementUpdate(float x, float y, Vector2 mag)
    {
        if (!grounded || jumping) return;

        //Slow down sliding
        if (crouching)
        {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
            return;
        }

        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    public Vector2 findVelocityRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private bool isFloor(Vector3 v)
    {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private bool cancellingGrounded;

    private void OnCollisionStay(Collision other)
    {
        //Make sure we are only checking for walkable layers
        int layer = other.gameObject.layer;
        if (GroundLayer != (GroundLayer | (1 << layer))) return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 normal = other.contacts[i].normal;
            //FLOOR
            if (isFloor(normal))
            {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        //Invoke ground/wall cancel, since we can't check normals with CollisionExit
        float delay = 3f;
        if (!cancellingGrounded)
        {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    private void StopGrounded()
    {
        grounded = false;
    }
}
