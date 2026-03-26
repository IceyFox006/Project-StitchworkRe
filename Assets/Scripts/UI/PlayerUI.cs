using UnityEngine;
using UnityEngine.Events;

public enum NextAnim
{
    Stay = 0,
    Dormant = 1,
    Next = 2,
}

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Animator _overlayAnimator;
    private UnityEvent onEndTransition;

    #region GS
    public Animator OverlayAnimator { get => _overlayAnimator; set => _overlayAnimator = value; }
    public UnityEvent OnEndTransition { get => onEndTransition; set => onEndTransition = value; }
    #endregion

    public void Initialize()
    {
        onEndTransition = new UnityEvent();
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
