using System.Collections;
using TMPro;
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
        SceneLoader.inst.StartCoroutine(DelayInput());
    }

    public void Disable(Button button = null)
    {
        _menu.SetActive(false);
        if (button != null)
            EventSystem.current.SetSelectedGameObject(button.gameObject);
    }

    public void SetSelectedToThis()
    {
        EventSystem.current.SetSelectedGameObject(_firstSelected.gameObject);
    }

    private IEnumerator DelayInput()
    {
        EventSystem.current.sendNavigationEvents = false;
        yield return new WaitForSecondsRealtime(0.1f);
        EventSystem.current.sendNavigationEvents = true;
    }
}
//---------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public class TextVisualUI
{
    [SerializeField] private GameObject _visual;
    [SerializeField] private TMP_Text _text;

    public void Enable(string text)
    {
        _visual.SetActive(true);
        _text.text = text;
    }

    public void Disable()
    {
        _visual.SetActive(false);
    }
}


