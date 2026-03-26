using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader inst;

    [Header("Don't Destroy On Load")]
    [SerializeField] private GameObject _objectEventSystem;
    [SerializeField] private GameObject _audioHandler;
    [SerializeField] private GameObject _raycastDrawer;

    [SerializeField] private Manager[] _managers;

    private void Awake()
    {
        inst = this;

        if (ObjectEventSystem.Current == null)
            Instantiate(_objectEventSystem, transform);

        //if (AudioHandler.Inst == null)
        //    Instantiate(_audioHandler, transform);

        if (RaycastDrawer.Inst == null)
            Instantiate(_raycastDrawer, transform);

        LoadManagers();
    }

    private void LoadManagers()
    {
        foreach (Manager manager in _managers)
            manager.Load();
    }
}
