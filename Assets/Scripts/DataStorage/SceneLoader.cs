using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Manager[] _managers;

    [Header("Don't Destroy On Load")]
    [SerializeField] private GameObject _objectSelectSystem;
    [SerializeField] private GameObject _audioHandler;
    [SerializeField] private GameObject _raycastDrawer;

    private void Awake()
    {
        if (ObjectSelectSystem.Current == null)
            Instantiate(_objectSelectSystem, transform);

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
