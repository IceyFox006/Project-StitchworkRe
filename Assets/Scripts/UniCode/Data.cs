using UnityEngine;

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
