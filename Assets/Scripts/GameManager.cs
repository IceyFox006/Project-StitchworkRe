using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private InputAction exit;
    private InputAction debug_reset;

    private void Start()
    {
        exit = InputSystem.actions.FindAction("EXIT");
        debug_reset = InputSystem.actions.FindAction("DEBUG_RESET");

        exit.performed += Exit_performed;
        debug_reset.performed += Debug_reset_performed;

        SwitchCode.DisableCursor();
    }

    private void Exit_performed(InputAction.CallbackContext obj)
    {
        SwitchCode.ExitApplication();
    }
    private void Debug_reset_performed(InputAction.CallbackContext obj)
    {
        SwitchCode.ResetScene();
    }
}
