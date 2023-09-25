using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DriverDismountedBehaviour : MonoBehaviour
{
    private InputActions inputActions;

    // Speed of moving player
    private const float speed = 1f;

    public Animator animator;

    private bool isMoving = false;
    private Vector2 direction;

    private void Awake()
    {
        inputActions = new InputActions();
    }

    private void Start()
    {
        inputActions?.Enable();

        inputActions.Driver.Movement.performed += ctx => TriggerMovement(ctx.ReadValue<Vector2>());
        inputActions.Driver.Movement.canceled += ctx => isMoving = false;
    }

    public void OnEnable()
    {
        inputActions?.Enable();
    }

    public void OnDisable()
    {
        inputActions?.Disable();
    }

    private void Update()
    {
        if (isMoving)
        {
            Movement();
            Animation(direction);
        }
        else
        {
            // To trigger idle animation
            Animation(Vector2.zero);
        }
    }

    private void TriggerMovement(Vector2 direc)
    {
        direction = direc;
        isMoving = true;
    }

    private void Movement()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void Animation(Vector2 direc)
    {
        animator.SetFloat("xVelocity", direc.x);
        animator.SetFloat("yVelocity", direc.y);
    }
}
