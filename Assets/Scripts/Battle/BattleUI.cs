using UnityEngine;
using UnityEngine.EventSystems;

public class BattleUI : MonoBehaviour
{
    private BattleManager bm;

    [SerializeField] private GameObject _ui;

    [Header("Menu")]
    [SerializeField] private MenuUI _battleMenu;
    [SerializeField] private MenuUI _confirmActionMenu;

    [Header("Moves")]
    [Tooltip("What player moves UI spawn under.")]
    [SerializeField] private GameObject _playerMoveUiPfb;
    [SerializeField] private Transform _playerMovesUiSP;

    #region GS
    public MenuUI ConfirmActionMenu { get => _confirmActionMenu; set => _confirmActionMenu = value; }
    public GameObject Ui { get => _ui; set => _ui = value; }
    public MenuUI BattleMenu { get => _battleMenu; set => _battleMenu = value; }
    #endregion

    public void Initialize(BattleManager bm)
    {
        this.bm = bm;
    }

    public void SwitchCurrentFighter(int dir) //@UsedGlobal_Button
    {
        int index = bm.FindFighter(bm.CurFighter.Data, bm.PParty);
        index = DataMethods.ShiftIndex(index, bm.PParty, dir);
        Debug.Log(bm.PParty[index].Data.AsString());
        bm.SwitchCurrentFighter(bm.PParty[index]);
    }
    #region Move
    //Spawns move buttons for player fighter.
    public void InstantiatePlayerFighterMovesUI(ActiveFighter actFighter)
    {
        GameObject clone;
        foreach (MoveSO move in actFighter.Data.Moves)
        {
            clone = Instantiate(_playerMoveUiPfb, _playerMovesUiSP);
            clone.GetComponent<PlayerMoveButton>().Initialize(bm, actFighter, move);
        }
    }

    //Despawns previous moves and respawns new moves for actFighter.
    public void ReloadPlayerFighterMovesUI(ActiveFighter actFighter)
    {
        GenericMethods.DestroyChildren(_playerMovesUiSP);
        InstantiatePlayerFighterMovesUI(actFighter);
    }
    #endregion

    #region Action
    public void ConfirmAction() //@UsedGlobal_Button
    {
        ObjectEventSystem.Current.ClearSelected();  //Clear selectedObjects.
        bm.DisableAllButtons();                     //Disable all fighter buttons.
        bm.Actions.Add(bm.CurAction);
        //bm.CurAction.UseAction();
        //bm.CurAction.PlayFighterAnimation();        //Play fighter animation (starts action sequence).
        _confirmActionMenu.Disable();               //Closes confirm action menu.
    }
    public void CancelAction() //@UsedGlobal_Button
    {
        ObjectEventSystem.Current.ClearSelected();  //Clear selectedObjects.
        bm.DisableAllButtons();                     //Disable all fighter buttons.
        bm.CurAction = null;                        //Reset curAction.
        _confirmActionMenu.Disable();               //Closes confirm action menu.
        _battleMenu.Enable();                       //Open battle menu.
    }
    #endregion
}
