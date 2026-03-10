using System.Collections.Generic;
using UnityEngine;

public class ObjectEventSystem : MonoBehaviour
{
    private static ObjectEventSystem current;

    private List<ButtonObject> selectedObjects = new List<ButtonObject>();

    private ButtonObject curSelected;

    #region GS
    public static ObjectEventSystem Current { get => current; set => current = value; }
    public List<ButtonObject> SelectedObjects { get => selectedObjects; set => selectedObjects = value; }
    #endregion

    private void Awake()
    {
        current = this;    
    }

    public void PressAll()
    {
        foreach (ButtonObject bo in selectedObjects)
            bo.Press();
    }

    public void SwitchHover(ButtonObject bo, bool forceInteractable = false)
    {
        if (forceInteractable)
            bo.Interactable = true;

        if (curSelected != null)
            curSelected.ExitHover();

        curSelected = bo;
        curSelected.EnterHover();
    }
}
