using UnityEngine;

public class PMovementStateGround : PMovementBaseState
{
    private PMovementStateManager m;

    public override void ConstructState(PMovementStateManager manager)
    {
        m = manager;
    }

    #region Overrides
    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override void FixedUpdateState()
    {
        Move();
    }

    public override void UpdateState()
    {
        Rotate();
    }
    #endregion

    private void Move()
    {
        m.MoveDirection = m.Move.ReadValue<Vector2>();
        m.Rb.linearVelocity = m.transform.TransformDirection(new Vector3(m.MoveDirection.x * m.MoveSpeed, m.Rb.linearVelocity.y, m.MoveDirection.y * m.MoveSpeed));
    }

    private void Rotate()
    {
        Quaternion targetRotation = Quaternion.Euler(0, m.MainCamera.transform.eulerAngles.y, 0);
        m.transform.rotation = targetRotation;
    }
}
