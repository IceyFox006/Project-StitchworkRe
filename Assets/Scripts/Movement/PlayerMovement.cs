using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private Camera _mainCamera;

    [Header("Ground")]
    private InputAction move;
    private Vector2 moveDirection;
    [SerializeField] private float _moveSpeed;

    private bool inputEnabled = true;
    private PMoveGroundState groundState = new PMoveGroundState();

    private PMoveBaseState curState;

    #region Get/Setters
    public InputAction Move { get => move; set => move = value; }
    public Rigidbody Rb { get => rb; set => rb = value; }
    public Vector2 MoveDirection { get => moveDirection; set => moveDirection = value; }
    public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
    public Camera MainCamera { get => _mainCamera; set => _mainCamera = value; }
    public bool InputEnabled { get => inputEnabled; set => inputEnabled = value; }
    #endregion

    public void Initialize()
    {
        rb = GetComponent<Rigidbody>();

        move = InputSystem.actions.FindAction("MOVE");

        groundState.ConstructState(this);

        curState = groundState;
        curState.EnterState();
    }
    private void FixedUpdate()
    {
        curState.FixedUpdateState();
    }
    private void Update()
    {
        curState.UpdateState();
    }
    public void EnableInput()
    {
        inputEnabled = true;
        curState.EnableInput();
    }
    public void DisableInput()
    {
        inputEnabled = false;
        curState.DisableInput();
    }

    public void SwitchState(PMoveBaseState state)
    {
        curState.ExitState();
        curState = state;
        curState.EnterState();
    }
}
