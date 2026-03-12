using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class Data : MonoBehaviour
{
    public static Vector2[] directions = new Vector2[]{Vector2.up, Vector2.down, Vector2.right, Vector2.left };

    #region Enums
    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    #endregion
}

[System.Serializable]
public class MenuUI
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private Button _firstSelected;

    public void Enable()
    {
        _menu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_firstSelected.gameObject);
    }

    public void Disable(Button button = null)
    {
        _menu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(button.gameObject);
    }
}
