using UnityEngine.InputSystem;

public class GameManager : Manager
{
    private InputAction exit;
    private InputAction debug_reset;

    public override void Load()
    {
        base.Load();

        exit = InputSystem.actions.FindAction("EXIT");
        debug_reset = InputSystem.actions.FindAction("DEBUG_RESET");

        exit.performed += Exit_performed;
        debug_reset.performed += Debug_reset_performed;

        GenericMethods.HideCursor();
    }

    private void Exit_performed(InputAction.CallbackContext obj)
    {
        GenericMethods.ExitApplication();
    }
    private void Debug_reset_performed(InputAction.CallbackContext obj)
    {
        GenericMethods.ResetScene();
    }
}
