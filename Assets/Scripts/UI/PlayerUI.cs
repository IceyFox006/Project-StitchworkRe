using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public enum NextAnim
{
    Stay = 0,
    Dormant = 1,
    Next = 2,
}

public class PlayerUI : MonoBehaviour
{
    [Header("Overlay")]
    [SerializeField] 
        private Animator _overlayAnimator;
    private UnityEvent onEndTransition;

    [Header("Pause Menu")]
    private InputAction exit;
    [SerializeField]
        private MenuUI _pauseMenu;

    #region GS
    public Animator OverlayAnimator { get => _overlayAnimator; set => _overlayAnimator = value; }
    public UnityEvent OnEndTransition { get => onEndTransition; set => onEndTransition = value; }
    #endregion

    public void Initialize()
    {
        onEndTransition = new UnityEvent();

        exit = InputSystem.actions.FindAction("EXIT");
        exit.performed += Exit_performed;
    }
    private void Exit_performed(InputAction.CallbackContext obj)
    {
        GenericMethods.SetTimeScale(0);
        _pauseMenu.Enable();
    }

    public void PlayOverlay(string trigger, NextAnim nextAnim)
    {
        _overlayAnimator.SetInteger("NEXT_ANIM", (int)nextAnim);
        _overlayAnimator.SetTrigger(trigger);
    }
    public void EndTransition()
    {
        onEndTransition.Invoke();
        onEndTransition.RemoveAllListeners();
    }

}
