using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Data;

public class ObjectEventSystem : MonoBehaviour
{
    private static ObjectEventSystem current;

    [SerializeField] private InputActionAsset _actions;
    private InputAction move;
    private InputAction press;
    private InputAction select;
    private InputAction back;

    private List<ButtonObject> selectedObjects = new List<ButtonObject>();
    private ButtonObject curHover;

    #region GS
    public static ObjectEventSystem Current { get => current; set => current = value; }
    public List<ButtonObject> SelectedObjects { get => selectedObjects; set => selectedObjects = value; }
    #endregion

    private void Awake()
    {
        current = this;  
        
        move = _actions.FindActionMap("UI").FindAction("MOVE");
        press = _actions.FindActionMap("UI").FindAction("PRESS");
        select = _actions.FindActionMap("UI").FindAction("SELECT");
        back = _actions.FindActionMap("UI").FindAction("BACK");
    }
    private void OnDestroy()
    {
        DisableInput();
    }

    #region Enable
    public void Enable()
    {
        move.performed += Move_performed;
        press.performed += Press_performed;
        select.performed += Select_performed;
        back.performed += Back_performed;
    }

    public void Disable()
    {
        DisableInput();

        ClearSelected();
    }
    public void DisableInput()
    {
        move.performed -= Move_performed;
        press.performed -= Press_performed;
        select.performed -= Select_performed;
        back.performed -= Back_performed;
    }
    #endregion

    #region Input
    //Switches the curHover with the direction moved.
    private void Move_performed(InputAction.CallbackContext obj)
    {
        if (curHover == null) return;
        
        Vector2 direction = move.ReadValue<Vector2>();
        if (direction == Vector2.up && CanMoveTo(curHover.Navigation.Up))
            SwitchHover(curHover.Navigation.Up);
        else if (direction == Vector2.down && CanMoveTo(curHover.Navigation.Down))
            SwitchHover(curHover.Navigation.Down);
        else if (direction == Vector2.right && CanMoveTo(curHover.Navigation.Right))
            SwitchHover(curHover.Navigation.Right);
        else if (direction == Vector2.left && CanMoveTo(curHover.Navigation.Left))
            SwitchHover(curHover.Navigation.Left);
    }

    private void Press_performed(InputAction.CallbackContext obj)
    {
        PressAll();
    }

    private void Select_performed(InputAction.CallbackContext obj)
    {
        curHover.Select();
    }
    private void Back_performed(InputAction.CallbackContext obj)
    {
        Debug.Log("> UI INPUT BACK");
        if (curHover.IsSelected) curHover.Deselect();
    }
    #endregion

    //Presses all selected buttons.
    public void PressAll()
    {
        for (int i = selectedObjects.Count - 1; i > -1; i--)
            selectedObjects[i].Press();
    }

    //Disables visuals and clears curHover and selectedObjects.
    public void ClearSelected()
    {
        curHover = null;
        foreach (ButtonObject bo in selectedObjects)
        {
            bo.IsSelected = false;
            bo.CurVisual.Reset();
        }
        selectedObjects.Clear();
    }

    //Switches the current hover button.
    public void SwitchHover(ButtonObject bo, bool forceInteractable = false)
    {
        if (forceInteractable)
            bo.Interactable = true;

        if (curHover != null)
            curHover.ExitHover();

        curHover = bo;
        curHover.EnterHover();
    }

    #region Check
    //Returns true if the bo can be moved to.
    private bool CanMoveTo(ButtonObject bo)
    {
        return (bo != null && bo.Interactable);
    }

    //Returns the index of target in selectedObjects.
    public int FindSelected(ButtonObject target)
    {
        for (int i = 0; i < selectedObjects.Count; i++)
            if (selectedObjects[i].EqualTo(target)) return i;

        return -1;
    }
    #endregion
}
