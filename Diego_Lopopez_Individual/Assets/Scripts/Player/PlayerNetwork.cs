using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float sprintSpeed;
    float speed;

    public float groundDrag;

    [Header("Ground check")]
    private float playerHeight = 2;
    public LayerMask whatIsGround;
    private bool grounded;

    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;

    [Header("Health")]
    public int maxHealth;
    [HideInInspector] public NetworkVariable<int> HealthPoint = new NetworkVariable<int>();

    [Header("Jump")]
    public float jumpForce;
    public float jumpCoolDown;
    public float airMultiplier;
    public bool readyToJump = true;

    [Header("keyBinds")]
    public KeyCode jumpkey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    public override void OnNetworkSpawn()
    {
        HealthPoint.Value = maxHealth;
        if (!IsOwner) return;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        speed = moveSpeed;
    }

    private void myInput()
    {
        if (!IsOwner) return;
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if(Input.GetKey(jumpkey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCoolDown);
        }

        if (Input.GetKeyDown(sprintKey) && grounded)
        {
            speed = sprintSpeed;
        }

        if (Input.GetKeyUp(sprintKey))
        {
            speed = moveSpeed;
        }
    }


    private void Update()
    {
        if (!IsOwner) return;
        myInput();
        speedControl();
        MovePlayer();

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void MovePlayer()
    {
        if (!IsOwner) return;
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * speed * 10, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * speed * 10 * airMultiplier, ForceMode.Force);
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
