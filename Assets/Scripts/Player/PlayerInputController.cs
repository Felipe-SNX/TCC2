using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private InputSystem_Actions controls;
    
    public Vector2 MoveVector { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool JumpCancelled { get; private set; }

    public Vector2 LookVector { get; private set; }
    public bool DashTriggered { get; private set; }

    private void Awake()
    {
        controls = new InputSystem_Actions();
        
        controls.Player.Jump.performed += _ => JumpTriggered = true;
        controls.Player.Jump.canceled += _ => JumpCancelled = true;

        controls.Player.Dash.performed += _ => DashTriggered = true;

        controls.Player.Look.performed += ctx => LookVector = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => LookVector = Vector2.zero;
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {
        MoveVector = controls.Player.Move.ReadValue<Vector2>();
    }

    public void ResetJumpInputs()
    {
        JumpTriggered = false;
        JumpCancelled = false;
    }

    public void ResetDashInput() => DashTriggered = false;
}