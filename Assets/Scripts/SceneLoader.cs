using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Manager[] _managers;

    [Header("Don't destroy on load.")]
    [SerializeField] private GameObject _audioHandler;
    [SerializeField] private GameObject _raycastDrawer;

    private void Awake()
    {
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
