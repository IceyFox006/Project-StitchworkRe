using UnityEngine;

public class PMoveGroundState : PMoveBaseState
{
    private PlayerMovement m;

    public override void ConstructState(PlayerMovement manager)
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
    public override void EnableInput()
    {

    }
    public override void DisableInput()
    {

    }
    #endregion

    private void Move()
    {
        if (!m.InputEnabled) return;

        m.MoveDirection = m.Move.ReadValue<Vector2>();
        m.Rb.linearVelocity = m.transform.TransformDirection(new Vector3(m.MoveDirection.x * m.MoveSpeed, m.Rb.linearVelocity.y, m.MoveDirection.y * m.MoveSpeed));
    }

    private void Rotate()
    {
        if (!m.InputEnabled) return;

        Quaternion targetRotation = Quaternion.Euler(0, m.MainCamera.transform.eulerAngles.y, 0);
        m.transform.rotation = targetRotation;
    }
}
