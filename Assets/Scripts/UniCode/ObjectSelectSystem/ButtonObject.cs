using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class ButtonObject : MonoBehaviour
{
    [SerializeField] private bool _interactable = true;
    private bool isSelected;

    [SerializeField] private Visual _visual;
    [SerializeField] private Navigation _navigation;

    [SerializeField] private UnityEvent _onPress;
    [SerializeField] private UnityEvent _onSelect;
    [SerializeField] private UnityEvent _onEnterHover;
    [SerializeField] private UnityEvent _onExitHover;

    private MeshRenderer[] renderers;

    #region
    public bool Interactable { get => _interactable; set => _interactable = value; }
    public bool IsSelected { get => isSelected; set => isSelected = value; }
    #endregion

    public void Initialize()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
    }

    private void OnMouseEnter()
    {
        EnterHover();
    }

    private void OnMouseExit()
    {
        ExitHover();
    }

    #region SelectionEvent
    public void Press()
    {
        if (!_interactable) return;

        _onPress.Invoke();
    }

    public void Select()
    {
        if (!_interactable) return;

        isSelected = true;
        AddMaterial(_visual.SelectMaterial);
        _onSelect.Invoke();
    }

    public void EnterHover()
    {
        if (!_interactable) return;

        AddMaterial(_visual.HoverMaterial);
        _onEnterHover.Invoke();
    }

    public void ExitHover()
    {
        if (!_interactable) return;

        RemoveMaterial();
        _onExitHover.Invoke();
    }
    #endregion
    #region VisualChange
    private void AddMaterial(Material material)
    {
        foreach (MeshRenderer renderer in renderers)
            renderer.materials = DataMethods.AddToArray(renderer.materials, material);
    }
    private void RemoveMaterial()
    {
        foreach (MeshRenderer renderer in renderers)
            DataMethods.RemoveLastFromArray(renderer.materials);
    }
    #endregion


    //=================================================================================================================
    public enum VisualType
    {
        NONE,
        ADD_MATERIAL,
    }

    [System.Serializable]
    public class Visual
    {
        [SerializeField] private VisualType _visual;

        [SerializeField][AllowNesting][ShowIf("_visual", VisualType.ADD_MATERIAL)]
            private Material _hoverMaterial;
        [SerializeField][AllowNesting][ShowIf("_visual", VisualType.ADD_MATERIAL)]
            private Material _selectMaterial;

        #region GS
        public Material HoverMaterial { get => _hoverMaterial; set => _hoverMaterial = value; }
        public Material SelectMaterial { get => _selectMaterial; set => _selectMaterial = value; }
        #endregion 
    }
    //=================================================================================================================
    public enum NavigationType
    {
        NONE,
        EXPLICIT,
    }

    [System.Serializable]
    public class Navigation
    {
        [SerializeField] private NavigationType _type;

        [SerializeField][AllowNesting][ShowIf("_type", NavigationType.EXPLICIT)] 
            private ButtonObject _up;
        [SerializeField][AllowNesting][ShowIf("_type", NavigationType.EXPLICIT)] 
            private ButtonObject _down;
        [SerializeField][AllowNesting][ShowIf("_type", NavigationType.EXPLICIT)] 
            private ButtonObject _right;
        [SerializeField][AllowNesting][ShowIf("_type", NavigationType.EXPLICIT)]
            private ButtonObject _left;

        public Navigation(ButtonObject up, ButtonObject down, ButtonObject right, ButtonObject left)
        {
            _up = up;
            _down = down;
            _right = right;
            _left = left;
        }
    }
}
