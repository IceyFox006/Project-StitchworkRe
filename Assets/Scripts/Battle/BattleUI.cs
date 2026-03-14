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
        EventSystem.current.SetSelectedGameObject(_playerMovesUiSP.GetChild(0).gameObject);
    }

    //Despawns previous moves and respawns new moves for actFighter.
    public void ReloadPlayerFighterMovesUI(ActiveFighter actFighter)
    {
        GenericMethods.DestroyChildren(_playerMovesUiSP);
        InstantiatePlayerFighterMovesUI(actFighter);
    }
    #endregion

    #region Target
    public void ConfirmAction() //@UsedGlobal
    {
        ObjectEventSystem.Current.ClearSelected();  //Clear selectedObjects.
        bm.CurAction.PlayAnimation();                   //Use action.
        _confirmActionMenu.Disable();               //Closes confirm action menu.
    }
    public void CancelAction() //@UsedGlobal
    {
        ObjectEventSystem.Current.ClearSelected();  //Clear selectedObjects.
        bm.CurAction = null;                        //Reset curAction.
        _confirmActionMenu.Disable();               //Closes confirm action menu.
        _battleMenu.Enable();                       //Open battle menu.
    }
    #endregion
}
