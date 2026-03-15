using UnityEngine;
using UnityEngine.SceneManagement;

public class GenericMethods : MonoBehaviour
{
    #region Camera
    public static void SwitchCamera(Camera from, Camera to)
    {
        from.gameObject.SetActive(false);
        to.gameObject.SetActive(true);
    }
    #endregion
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
    #region Object
    public static void DestroyChildren(Transform parent)
    {
        int childCount = parent.childCount;
        for (int i = childCount - 1; i > -1; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
        //while (parent.childCount > 0)
        //    Destroy(parent.GetChild(0).gameObject);
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
