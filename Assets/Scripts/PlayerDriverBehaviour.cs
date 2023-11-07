using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDriverBehaviour : DriverBehavior
{
    private InputActions inputActions;
    private void Awake()
    {
        inputActions = new InputActions();
    }

    protected override void Start()
    {
        base.Start();

        inputActions?.Enable();

        inputActions.Player.Movement.performed += ctx => TriggerMovement(ctx.ReadValue<Vector2>());
        inputActions.Player.Movement.canceled += ctx => StopMoving();
        inputActions.Player.Reverse.performed += ctx => isReversing = true;
        inputActions.Player.Reverse.canceled += ctx => isReversing = false;
    }

    public void OnEnable()
    {
        inputActions?.Enable();
    }

    public void OnDisable()
    {
        inputActions?.Disable();
    }
}
