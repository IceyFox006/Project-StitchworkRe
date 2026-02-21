using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PMovementStateManager : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Camera _mainCamera;

    [Header("Ground")]
    private InputAction move;
    private Vector2 moveDirection;
    [SerializeField] private float _moveSpeed;

    private PMovementStateGround groundState = new PMovementStateGround();

    private PMovementBaseState currentState;

    #region Get/Setters
    public InputAction Move { get => move; set => move = value; }
    public Rigidbody Rb { get => _rb; set => _rb = value; }
    public Vector2 MoveDirection { get => moveDirection; set => moveDirection = value; }
    public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
    public Camera MainCamera { get => _mainCamera; set => _mainCamera = value; }
    #endregion

    private void Start()
    {
        move = InputSystem.actions.FindAction("MOVE");

        groundState.ConstructState(this);

        currentState = groundState;
        currentState.EnterState();
    }
    private void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }
    private void Update()
    {
        currentState.UpdateState();
    }

    [HideInInspector]
    public void SwitchState(PMovementBaseState state)
    {
        currentState.ExitState();
        currentState = state;
        currentState.EnterState();
    }
}
