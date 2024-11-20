using UnityEngine;


public class Inputs : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;

    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    [HideInInspector] public string currentControlScheme;

    private PlayerInputs inputs;

    private void Awake()
    {
        inputs = new PlayerInputs();
        currentControlScheme = inputs.KeyboardMouseScheme.name; //!
    }

    private void OnEnable() => inputs.Enable();

    private void OnDisable() => inputs.Disable();

    private void Update()
    {
        MoveInput(inputs.Player.Move.ReadValue<Vector2>());

        if (cursorInputForLook)
            LookInput(inputs.Player.Look.ReadValue<Vector2>());

        JumpInput(inputs.Player.Jump.IsPressed());
        SprintInput(inputs.Player.Sprint.IsPressed());
    }

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }

    public void SprintInput(bool newSprintState)
    {
        sprint = newSprintState;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}

