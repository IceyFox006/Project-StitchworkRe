public abstract class PMovementBaseState
{
    public abstract void ConstructState(PMovementStateManager manager);
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void FixedUpdateState();
    public abstract void UpdateState();
}
