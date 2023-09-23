using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DriverDismountedBehaviour : MonoBehaviour
{
    private InputActions inputActions;

    private Vector2 direction;

    private const float speed = 1f;

    public Animator animator;

    private void Awake()
    {
        inputActions = new InputActions();
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
        Movement();
        Animation();
    }

    private void Movement()
    {
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void Animation()
    {
        animator.SetFloat("xVelocity", direction.x);
        animator.SetFloat("yVelocity", direction.y);
    }
}
