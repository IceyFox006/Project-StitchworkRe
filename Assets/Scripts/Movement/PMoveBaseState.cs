public abstract class PMoveBaseState
{
    public abstract void ConstructState(PMoveStateManager manager);
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void FixedUpdateState();
    public abstract void UpdateState();
    public abstract void EnableInput();
    public abstract void DisableInput();
}
