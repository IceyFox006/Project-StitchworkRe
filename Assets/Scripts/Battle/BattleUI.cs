using UnityEngine;

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
    [SerializeField] private GameObject _hasActedVisual;
    [SerializeField] private TextVisualUI _curActionVisual;

    #region GS
    public MenuUI ConfirmActionMenu { get => _confirmActionMenu; set => _confirmActionMenu = value; }
    public GameObject Ui { get => _ui; set => _ui = value; }
    public MenuUI BattleMenu { get => _battleMenu; set => _battleMenu = value; }
    public GameObject HasActedVisual { get => _hasActedVisual; set => _hasActedVisual = value; }
    public TextVisualUI CurActionVisual { get => _curActionVisual; set => _curActionVisual = value; }
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
        PlayerMoveButton clone;
        foreach (MoveSO move in actFighter.Data.Moves)
        {
            clone = Instantiate(_playerMoveUiPfb, _playerMovesUiSP).GetComponent<PlayerMoveButton>();
            clone.Initialize(bm, actFighter, move);

            //Interactable
            if (actFighter.HasActed || actFighter.Data.CurEnergy < move.EnergyCost) //If already acted or not enough energy.
                clone.Button.interactable = false;
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
        bm.Actions.Add(bm.CurAction);
        bm.CurAction.User.HasActed = true;

        int nextIndex = bm.FindFirstUnactedFighter(bm.FindFighter(bm.CurFighter.Data, bm.PParty), bm.PParty);
        if (nextIndex > -1)
        {
            bm.SwitchCurrentFighter(bm.PParty[nextIndex]);
            CancelAction();
        }
        else
        {
            FinishChooseAction();
            bm.DetermineEnemyActions();
            bm.Actions.UseFirstAction();
        }
    }
    public void CancelAction() //@UsedGlobal_Button
    {
        FinishChooseAction();
        _battleMenu.Enable();                       //Open battle menu.
    }

    private void FinishChooseAction()
    {
        ObjectEventSystem.Current.ClearSelected();  //Clear selectedObjects.
        bm.DisableAllFighterButtons();              //Disable all fighter buttons.
        bm.CurAction = null;                        //Reset curAction.
        _confirmActionMenu.Disable();               //Closes confirm action menu.
    }
    #endregion
}
