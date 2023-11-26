using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float sprintSpeed;
    public float speed;

    public float waterSpeed;
    public float waterSprintSpeed;

    public float groundDrag;

    [Header("Ground check")]
    private float playerHeight = 2.1f;
    public LayerMask whatIsGround;
    public bool grounded;
    public LayerMask whatIsWater;
    public bool watered;

    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;

    [Header("Health")]
    public int maxHealth;
    public float maxStamina;
    public float looseStamina;
    public float recoverStamina;
    public float maxSanity;

    //[HideInInspector] 
    public NetworkVariable<int> HealthPoint = new NetworkVariable<int>();
    public NetworkVariable<float> StaminaPoint = new NetworkVariable<float>();
    public NetworkVariable<float> SanityPoint = new NetworkVariable<float>();

    [Header("Jump")]
    public float jumpForce;
    public float jumpCoolDown;
    public float airMultiplier;
    public bool readyToJump = true;

    [Header("keyBinds")]
    public KeyCode jumpkey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Others")]
    public Animator animator;
    public SkinnedMeshRenderer surface;
    public SkinnedMeshRenderer joints;

    private bool isRuning;
    public bool resting;
    public bool inLocker;

    public override void OnNetworkSpawn()
    {
        //Set player variables at max value
        HealthPoint.Value = maxHealth;
        StaminaPoint.Value = maxStamina;
        SanityPoint.Value = maxSanity;

        if (!IsOwner) return;
        surface.enabled = false;
        joints.enabled = false;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        speed = moveSpeed;
    }

    private void myInput()
    {
        if (!IsOwner) return;
        if (inLocker) return;
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        

        if (Input.GetKey(jumpkey) && readyToJump && watered)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCoolDown);
        }


        if (Input.GetKey(jumpkey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCoolDown);
        }

        if (Input.GetKeyDown(sprintKey) && grounded)
        {
            if (rb.velocity != new Vector3(0, 0, 0) && StaminaPoint.Value > 0)
            {
                speed = sprintSpeed;
                isRuning = true;
                animator.ResetTrigger("Walking");
                animator.SetTrigger("Running");
                animator.SetBool("RunForward", true);
            }    
        }


        if (Input.GetKeyDown(sprintKey) && watered)
        {
            if (rb.velocity != new Vector3(0, 0, 0) && StaminaPoint.Value > 0)
            {
                speed = waterSprintSpeed;
                isRuning = true;
                
            }
        }

        if (Input.GetKeyUp(sprintKey) && watered || StaminaPoint.Value <= 0 && watered)
        {
            isRuning = false;
            speed = waterSpeed;
        }

        if (Input.GetKeyUp(sprintKey) || StaminaPoint.Value <= 0)
        {
            isRuning = false;
            speed = moveSpeed;

            animator.ResetTrigger("Running");
            animator.SetBool("RunForward", false);
        }




        //Animation

        //Walking forward
        if (verticalInput > 0 && !isRuning )
        {
            animator.SetTrigger("Walking");
            animator.SetBool("WalkForward", true);
        }

        //Walking backwards
        if (verticalInput < 0 && !isRuning)
        {
            animator.SetTrigger("Walking");
            animator.SetBool("WalkBackwards", true);
        }

        //Walking Left
        if (horizontalInput < 0 && !isRuning)
        {
            animator.SetTrigger("Walking");
            animator.SetBool("WalkLeft", true);
        }

        //Walking Right
        if (horizontalInput > 0 && !isRuning)
        {
            animator.SetTrigger("Walking");
            animator.SetBool("WalkRight", true);
        }

        //Running forward
        if (verticalInput > 0 && isRuning)
        {
            animator.SetTrigger("Running");
            animator.SetBool("RunBackwards", true);
        }

        //Running backwards
        if (verticalInput < 0 && isRuning)
        {
            animator.SetTrigger("Running");
            animator.SetBool("RunBackwards", true);
        }

        if (horizontalInput == 0 && verticalInput == 0)
        {
            animator.SetBool("WalkForward", false);
            animator.SetBool("WalkBackwards", false);
            animator.SetBool("RunBackwards", false);
            animator.SetBool("RunForward", false);
            animator.SetBool("WalkLeft", false);
            animator.SetBool("WalkRight", false);
            animator.ResetTrigger("Walking");
            animator.ResetTrigger("Running");
        }
    }

    //Loose stamina when running
    [ServerRpc(RequireOwnership = false)]
    private void looseStaminaServerRPC()
    {
        StaminaPoint.Value -= looseStamina * Time.deltaTime;
        if (StaminaPoint.Value <= 0)
        {
            StaminaPoint.Value = 0;
        }
    }

    //Recover stamina when not running
    [ServerRpc(RequireOwnership = false)]
    private void recoverStaminaServerRPC()
    {
        StaminaPoint.Value += recoverStamina * Time.deltaTime;
        if(StaminaPoint.Value >= maxStamina)
        {
            StaminaPoint.Value = maxStamina;
        }
    }


    private void Update()
    {
        if (!IsOwner) return;
        myInput();
        speedControl();
        MovePlayer();

        if (isRuning)
        {
            looseStaminaServerRPC();
        }
        else
        {
            recoverStaminaServerRPC();
        }

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        watered = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsWater);

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        if (inLocker)
        {
            isRuning = false;
        }
    }

    private void MovePlayer()
    {
        if (!IsOwner) return;
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (watered)
        {
            rb.AddForce(moveDirection.normalized * waterSpeed * 1000 * Time.deltaTime, ForceMode.Force);
        }

        else if (grounded)
        {
            rb.AddForce(moveDirection.normalized * speed * 1000 * Time.deltaTime, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * speed * 1000 * Time.deltaTime * airMultiplier, ForceMode.Force);
        }
    }

    private void speedControl()
    {
        if (!IsOwner) return;
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
