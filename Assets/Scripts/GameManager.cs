using UnityEngine.InputSystem;

public class GameManager : Manager
{
    private InputAction debug_reset;

    public override void Load()
    {
        base.Load();

        debug_reset = InputSystem.actions.FindAction("DEBUG_RESET");
        debug_reset.performed += Debug_reset_performed;

        GenericMethods.HideCursor();
    }

    private void Debug_reset_performed(InputAction.CallbackContext obj)
    {
        GenericMethods.ResetScene();
    }
}
