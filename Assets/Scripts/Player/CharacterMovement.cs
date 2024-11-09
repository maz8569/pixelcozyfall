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
    const float EXTRA_HEIGHT = .1f;

    private PlayerControls inputActions;
    private float groundedGravity = -.5f;
    private float gravity = -9.8f;
    private float initialJumpVelocity = 0f;
    private Vector2 movementInput;

    private void Awake()
    {
        inputActions = new PlayerControls();
        boxCollider = GetComponent<BoxCollider2D>();

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
    }

    private void HandleGravity()
    {
        if (IsGrounded())
        {
            velocity.y = groundedGravity;
        }
        else
        {
            float previousYVelocity = velocity.y;
            float newGravity = gravity;
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
        }
    }

    private void FixedUpdate()
    {
        HandleGravity();
        HandleJump();
        velocity.x = movementInput.x * maxSpeed;
        body.velocity = velocity;
    }
}
