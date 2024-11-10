using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float gravityModifier = 1f;
    [SerializeField] private float maxJumpTime = 2f;
    [SerializeField] private float maxJumpHeight = 6f;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private bool isGrounded;
    [SerializeField] private LayerMask platformLayerMask;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private const float minMoveDistance = 0.001f;
    [SerializeField] private bool isAirborne = false;
    const float EXTRA_HEIGHT = .1f;

    private PlayerControls inputActions;
    private float groundedGravity = -.5f;
    private float gravity = -9.8f;
    private float initialJumpVelocity = 0f;
    private Vector2 movementInput;
    private Animator animator;
    private SpriteRenderer sprite;

    private void Awake()
    {
        inputActions = new PlayerControls();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        inputActions.Player.Movement.performed += OnMovement;
        inputActions.Player.Movement.canceled += OnMovement;

        SetupJumpVariables();
    }

    private void SetupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        Debug.Log(gravity);
        Debug.Log(initialJumpVelocity);
    }

    private void OnEnable()
    {
        inputActions.Enable();
        body = GetComponent<Rigidbody2D>();
        body.isKinematic = false;
    }

    private void OnDisable()
    {
        inputActions.Disable();
        body.isKinematic = true;
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        animator.SetBool("IsRunning", movementInput.x != 0);
        if(movementInput.x > 0) sprite.flipX = false;
        if(movementInput.x < 0) sprite.flipX = true;
    }

    private void HandleGravity()
    {
        if (IsGrounded())
        {
            if (isAirborne) 
            { 
                StartCoroutine(JumpSqueeze(1.4f, 0.8f, 0.05f));
                isAirborne = false;
            }
            if(animator.GetBool("IsFalling")) animator.SetBool("IsFalling", false);
            velocity.y = groundedGravity;
        }
        else
        {
            float previousYVelocity = velocity.y;
            float newGravity = gravity;
            if (previousYVelocity < 0) 
            {
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsFalling", true);
            }
            if(previousYVelocity < 0 || movementInput.y <= 0)
            {
                newGravity *= gravityModifier;
            }
            float newYVelocity = velocity.y + newGravity * Time.fixedDeltaTime;
            float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;
            velocity.y = nextYVelocity;
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, EXTRA_HEIGHT, platformLayerMask);
        //Color raycolor = raycastHit.collider != null ? Color.green : Color.red;
        //Debug.DrawRay(boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + EXTRA_HEIGHT), raycolor);
        //Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + EXTRA_HEIGHT), raycolor);
        return raycastHit.collider != null;
    }
    
    private void HandleJump()
    {
        if (IsGrounded() && movementInput.y > 0)
        {
            velocity.y = initialJumpVelocity;
            animator.SetBool("IsJumping", true);
            StartCoroutine(JumpSqueeze(0.5f, 1.4f, 0.1f));
            isAirborne = true;
        }
    }

    private void FixedUpdate()
    {
        HandleGravity();
        HandleJump();
        velocity.x = movementInput.x * maxSpeed;
        body.velocity = velocity;
    }

    IEnumerator JumpSqueeze(float xSqueeze, float ySqueeze, float seconds)
    {
        Vector3 originalSize = Vector3.one;
        Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            yield return null;
        }
        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            yield return null;
        }

    }
}
