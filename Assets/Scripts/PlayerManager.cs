using UnityEngine;

public class PlayerManager : Manager
{
    [SerializeField] private PlayerSO _data;
    private PMoveStateManager move;

    #region GS
    public PlayerSO Data { get => _data; set => _data = value; }
    public PMoveStateManager Move { get => move; set => move = value; }
    #endregion

    public override void Load()
    {
        base.Load();

        move = GetComponentInChildren<PMoveStateManager>();
        Move.Initialize();
    }
}
