using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public enum EventType
{
    PRESS,
    SELECT,
    ENTER_HOVER,
    EXIT_HOVER,
}

public class ButtonObject : MonoBehaviour
{
    [SerializeField] private bool _interactable = true;

    [SerializeField] protected VisualType _visual;
    private Visual curVisual;
    [SerializeField][ShowIf("_visual", VisualType.ADD_MATERIAL)]
        private AddMaterialVisual _addMaterialVisual;


    [SerializeField] private Navigation _navigation;

    [SerializeField] private UnityEvent _onPress;
    [SerializeField] private UnityEvent _onSelect;
    [SerializeField] private UnityEvent _onEnterHover;
    [SerializeField] private UnityEvent _onExitHover;

    #region
    public bool Interactable { get => _interactable; set => _interactable = value; }
    #endregion

    public void Initialize()
    {
        InitializeVisual();
    }

    private void OnMouseDown()
    {
        Select();
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

        curVisual.Reset();
        curVisual.Set(EventType.SELECT);

        ObjectEventSystem.Current.SelectedObjects.Add(this);
        _onSelect.Invoke();
    }

    public void EnterHover()
    {
        if (!_interactable) return;

        curVisual.Set(EventType.ENTER_HOVER);

        _onEnterHover.Invoke();
    }

    public void ExitHover()
    {
        if (!_interactable) return;

        curVisual.Reset();

        _onExitHover.Invoke();
    }
    #endregion
    #region Set
    private void InitializeVisual()
    {
        switch (_visual)
        {
            case VisualType.NONE: curVisual = new Visual(); break;
            case VisualType.ADD_MATERIAL: curVisual = _addMaterialVisual; break;
        }

        curVisual.Initialize(this);
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
        public virtual void Initialize(ButtonObject bo) { }
        public virtual void Reset() { }
        public virtual void Set(EventType eventType) { }
    }
    //-----------------------------------------------------------------------------------------------------------------
    [System.Serializable]
    public class AddMaterialVisual : Visual
    {
        [SerializeField] private Material _selectMaterial;
        [SerializeField] private Material _hoverMaterial;

        private MeshRenderer[] renderers;

        #region GS
        public Material HoverMaterial { get => _hoverMaterial; set => _hoverMaterial = value; }
        public Material SelectMaterial { get => _selectMaterial; set => _selectMaterial = value; }
        #endregion 

        public override void Initialize(ButtonObject bo)
        {
            renderers = bo.GetComponentsInChildren<MeshRenderer>();
        }
        public override void Reset()
        {
            RemoveMaterial();
        }
        public override void Set(EventType eventType)
        {
            switch (eventType)
            {
                case EventType.PRESS: break;
                case EventType.SELECT: AddMaterial(_selectMaterial); break;
                case EventType.ENTER_HOVER: AddMaterial(_hoverMaterial); break;
                case EventType.EXIT_HOVER: break;
            }
        }

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
