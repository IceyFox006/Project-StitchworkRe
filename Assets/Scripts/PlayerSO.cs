using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Scriptable Objects/Player")]
public class PlayerSO : ScriptableObject
{
    [SerializeField] private string _name;

    [Tooltip("Max capacity is 8.")]
    [SerializeField] private PlayerFighter[] _party;

    #region GS
    public PlayerFighter[] Party { get => _party; set => _party = value; }
    #endregion
}
