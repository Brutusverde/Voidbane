using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    public PlayerOptions playerOptions;
    public AudioClip[] steps;
    public AudioClip[] waterSteps;
    public float timeBetweenSteps;
    public float timeBetweenStepsRun;
    public float timeBetweenStepsWater;
    public Rigidbody rb;
    private float playerHeight = 2.1f;

    public LayerMask whatIsGround;
    public bool grounded;
    public LayerMask whatIsWater;
    public bool watered;

    private AudioSource audioSource;
    private AudioClip clip;
    float horizontalInput;
    float verticalInput;
    private float timer;
    private bool reproducing;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        timer -= Time.deltaTime;

        if (grounded)
        {
            GroundSteps();
        }

        if (watered)
        {
            WaterSteps();
        }

    }

    void GroundSteps()
    {
        if (rb.velocity.magnitude <= 1) return;

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            if (horizontalInput != 0 && !reproducing || verticalInput != 0 && !reproducing)
            {
                timer = timeBetweenSteps;
                reproducing = true;
                int index = Random.Range(0, steps.Length);
                clip = steps[index];
                audioSource.clip = clip;
                audioSource.Play();
            }

            if (horizontalInput != 0 && reproducing || verticalInput != 0 && reproducing)
            {

                if (timer <= 0)
                {
                    reproducing = false;
                }
            }
        }
        else
        {
            if (horizontalInput != 0 && !reproducing || verticalInput != 0 && !reproducing)
            {
                int index = Random.Range(0, steps.Length);
                clip = steps[index];
                audioSource.clip = clip;
                audioSource.Play();
                timer = timeBetweenStepsRun;
                reproducing = true;

            }

            if (horizontalInput != 0 && reproducing || verticalInput != 0 && reproducing)
            {

                if (timer <= 0)
                {
                    reproducing = false;
                }
            }
        }
    }

    void WaterSteps()
    {
        if (rb.velocity.magnitude <= 1) return;

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            if (horizontalInput != 0 && !reproducing || verticalInput != 0 && !reproducing)
            {
                timer = timeBetweenStepsWater;
                reproducing = true;
                int index = Random.Range(0, waterSteps.Length);
                clip = waterSteps[index];
                audioSource.clip = clip;
                audioSource.Play();
            }

            if (horizontalInput != 0 && reproducing || verticalInput != 0 && reproducing)
            {

                if (timer <= 0)
                {
                    reproducing = false;
                }
            }
        }
        else
        {
            if (horizontalInput != 0 && !reproducing || verticalInput != 0 && !reproducing)
            {
                int index = Random.Range(0, waterSteps.Length);
                clip = waterSteps[index];
                audioSource.clip = clip;
                audioSource.Play();
                timer = timeBetweenStepsWater;
                reproducing = true;

            }

            if (horizontalInput != 0 && reproducing || verticalInput != 0 && reproducing)
            {

                if (timer <= 0)
                {
                    reproducing = false;
                }
            }
        }
    }

    void CheckGround()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        watered = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsWater);
    }
}
