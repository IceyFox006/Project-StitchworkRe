using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public enum EventType
{
    PRESS,
    SELECT,
    DESELECT,
    ENTER_HOVER,
    EXIT_HOVER,
}

public class ButtonObject : MonoBehaviour
{
    private string id;

    [SerializeField] private bool _interactable = true;
    [SerializeField] private bool _deselectOnPress = true;
    private bool isSelected;

    [SerializeField] protected VisualType _visual;
    private EventVisual curVisual;
    [SerializeField][ShowIf("_visual", VisualType.ADD_MATERIAL)]
        private AddMaterialVisual _addMaterialVisual;


    [SerializeField] private EventNavigation _navigation;

    [SerializeField] private UnityEvent _onPress;
    [SerializeField] private UnityEvent _onSelect;
    [SerializeField] private UnityEvent _onDeselect;
    [SerializeField] private UnityEvent _onEnterHover;
    [SerializeField] private UnityEvent _onExitHover;

    #region
    public bool Interactable { get => _interactable; set => _interactable = value; }
    public EventNavigation Navigation { get => _navigation; set => _navigation = value; }
    public bool IsSelected { get => isSelected; set => isSelected = value; }
    public bool DeselectOnPress { get => _deselectOnPress; set => _deselectOnPress = value; }
    public string Id { get => id; set => id = value; }
    #endregion

    public void Initialize()
    {
        InitializeVisual();

        id = GenerateID();
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

    #region Events
    public void Press()
    {
        if (!_interactable) return;

        _onPress.Invoke();

        if (_deselectOnPress)
            Deselect();
    }

    public void Select()
    {
        if (!_interactable) return;

        curVisual.Reset();
        curVisual.Set(EventType.SELECT);

        isSelected = true;
        ObjectEventSystem.Current.SelectedObjects.Add(this);
        _onSelect.Invoke();
    }

    public void Deselect()
    {
        if (!_interactable) return;
        if (!isSelected) return;

        curVisual.Reset();
        curVisual.Set(EventType.ENTER_HOVER);

        isSelected = false;
        ObjectEventSystem.Current.SelectedObjects.RemoveAt(ObjectEventSystem.Current.FindSelected(this));
        _onDeselect.Invoke();
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

        if (!isSelected)
            curVisual.Reset();

        _onExitHover.Invoke();
    }
    #endregion
    #region SetUp
    private string GenerateID()
    {
        string ID = GetType().ToString() + "_";

        ID += transform.position.x.ToString() + transform.position.y.ToString() + transform.position.z.ToString();

        return ID;
    }
    private void InitializeVisual()
    {
        switch (_visual)
        {
            case VisualType.NONE: curVisual = new EventVisual(); break;
            case VisualType.ADD_MATERIAL: curVisual = _addMaterialVisual; break;
        }

        curVisual.Initialize(this);
    }
    #endregion
    #region Check
    public bool EqualTo(ButtonObject other)
    {
        return (id.Equals(other.Id));
    }
    #endregion

    //=================================================================================================================
    public enum VisualType
    {
        NONE,
        ADD_MATERIAL,
    }

    [System.Serializable]
    public class EventVisual
    {
        public virtual void Initialize(ButtonObject bo) { }
        public virtual void Reset() { }
        public virtual void Set(EventType eventType) { }
    }
    //-----------------------------------------------------------------------------------------------------------------
    [System.Serializable]
    public class AddMaterialVisual : EventVisual
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

        //Adds a material to all renderers at the end.
        private void AddMaterial(Material material)
        {
            foreach (MeshRenderer renderer in renderers)
                renderer.materials = DataMethods.AddToArray(renderer.materials, material);
        }

        //Removes the last material from all renderers.
        private void RemoveMaterial()
        {
            foreach (MeshRenderer renderer in renderers)
                renderer.materials = DataMethods.RemoveLastFromArray(renderer.materials);
        }
    }
    //=================================================================================================================
    public enum NavigationType
    {
        NONE,
        EXPLICIT,
    }

    [System.Serializable]
    public class EventNavigation
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

        #region GS
        public ButtonObject Up { get => _up; set => _up = value; }
        public ButtonObject Down { get => _down; set => _down = value; }
        public ButtonObject Right { get => _right; set => _right = value; }
        public ButtonObject Left { get => _left; set => _left = value; }
        #endregion
        public EventNavigation(ButtonObject up, ButtonObject down, ButtonObject right, ButtonObject left)
        {
            _up = up;
            _down = down;
            _right = right;
            _left = left;
        }
    }
}
