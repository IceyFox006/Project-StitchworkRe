using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerSO _data;

    #region GS
    public PlayerSO Data { get => _data; set => _data = value; }
    #endregion
}
