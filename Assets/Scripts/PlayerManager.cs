using UnityEngine;

public class PlayerManager : Manager
{
    [SerializeField] private PlayerSO _data;
    private PlayerMovement move;
    private PlayerUI ui;

    private bool inBattle;

    #region GS
    public PlayerSO Data { get => _data; set => _data = value; }
    public PlayerMovement Move { get => move; set => move = value; }
    public PlayerUI Ui { get => ui; set => ui = value; }
    public bool InBattle { get => inBattle; set => inBattle = value; }
    #endregion

    public override void Load()
    {
        base.Load();

        move = GetComponentInChildren<PlayerMovement>();
        move.Initialize();

        ui = GetComponentInChildren<PlayerUI>();
        ui.Initialize();
    }
}
