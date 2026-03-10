using UnityEngine;
using UnityEngine.SceneManagement;

public class GenericMethods : MonoBehaviour
{
    public static void DestroyChildren(Transform parent)
    {
        while (parent.childCount > 0)
            Destroy(parent.GetChild(0));
    }

    #region Cursor
    public static void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public static void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    #endregion
    #region Scene
    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    public static void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public static void ExitApplication()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    #endregion
}
