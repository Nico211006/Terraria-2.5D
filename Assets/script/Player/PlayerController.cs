using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 7.5f;

    [Header("Beschleunigung")]
    public float acceleration = 20f;
    public float deceleration = 25f;
    public float turnAcceleration = 12f;

    [Header("Jump")]
    public float jumpForce = 7f;
    public float fallMultiplier = 2.2f;
    public float lowJumpMultiplier = 2f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Optional Visual")]
    public Transform characterVisual;

    private Rigidbody rb;
    private float moveInput;
    private bool isGrounded;
    private bool isSprinting;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Vector3 velocity = rb.linearVelocity;
            velocity.y = jumpForce;
            rb.linearVelocity = velocity;
        }

        BetterJump();

        HandleFlip();
    }

    private void FixedUpdate()
    {
        float targetSpeed = isSprinting ? sprintSpeed : walkSpeed;
        float desiredVelocityX = moveInput * targetSpeed;
        float currentVelocityX = rb.linearVelocity.x;

        float accelRate;

        if (Mathf.Abs(moveInput) < 0.01f)
        {
            accelRate = deceleration;
        }
        else if (Mathf.Sign(moveInput) != Mathf.Sign(currentVelocityX) && Mathf.Abs(currentVelocityX) > 0.1f)
        {
            accelRate = turnAcceleration;
        }
        else
        {
            accelRate = acceleration;
        }

        float newVelocityX = Mathf.MoveTowards(
            currentVelocityX,
            desiredVelocityX,
            accelRate * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector3(newVelocityX, rb.linearVelocity.y, 0f);
    }

    private void BetterJump()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void HandleFlip()
    {
        if (characterVisual == null)
            return;

        if (moveInput > 0.05f)
        {
            characterVisual.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (moveInput < -0.05f)
        {
            characterVisual.localRotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}