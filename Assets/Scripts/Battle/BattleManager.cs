using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BattleUI))]
public class BattleManager : Manager
{
    private static BattleManager inst;

    public static float effectivenessMultiplier = 0.2f;
    public static float STABMultiplier = 1.2f;

    private BattleUI ui;

    [Tooltip("Where the player gets teleported to when a battle begins.")]
    [SerializeField] private Transform _battleArea;
    [SerializeField] private Camera _battleCamera;

    [Header("Spawn Points")]
    [Tooltip("What player fighter UI spawn under.")]
    [SerializeField] private Transform _playerFighterUiSP;
    [Tooltip("What enemy fighter UI spawn under.")]
    [SerializeField] private Transform _enemyFighterUiSP;
    [Tooltip("Where player fighters spawn in.")]
    [SerializeField] private Transform[] _playerFighterSPs;
    [Tooltip("Where enemy fighters spawn in.")]
    [SerializeField] private Transform[] _enemyFighterSPs;

    [Header("Prefabs")]
    [SerializeField] private GameObject _fighterPfb;
    [SerializeField] private GameObject _fighterUiPfb;


    private List<ActiveFighter> pParty = new List<ActiveFighter>();
    private List<ActiveFighter> eParty = new List<ActiveFighter>();

    private ActionList actions = new ActionList();
    private ActiveFighter curFighter;
    private ActiveAction curAction;

    #region GS
    public Transform BattleArea { get => _battleArea; set => _battleArea = value; }
    public ActiveAction CurAction { get => curAction; set => curAction = value; }
    public BattleUI Ui { get => ui; set => ui = value; }
    public static BattleManager Inst { get => inst; set => inst = value; }
    public Camera BattleCamera { get => _battleCamera; set => _battleCamera = value; }
    #endregion
    public override void Load()
    {
        base.Load();

        ui = GetComponent<BattleUI>();
        ui.Initialize(this);

        inst = this;
    }

    //Starts a battle.
    public void StartBattle(PlayerFighter[] playerParty, EnemyFighter[] enemyParty)
    {
        //SET-UP
        ui.Ui.SetActive(true);

        //Player Party
        for (int pfID = 0; pfID < _playerFighterSPs.Length; pfID++)
        {
            if (pfID >= playerParty.Length) break;

            pParty.Add(InstantiateFighter(playerParty[pfID], _fighterPfb, _playerFighterSPs[pfID], _playerFighterUiSP));
        }

        //Enemy Party
        for (int efID = 0; efID < _enemyFighterSPs.Length; efID++)
        {
            if (efID >= enemyParty.Length) break;

            eParty.Add(InstantiateFighter(enemyParty[efID], _fighterPfb, _enemyFighterSPs[efID], _enemyFighterUiSP));
        }

        SwitchCurrentFighter(pParty[0]);
    }

    private void EndBattle()
    {
        GenericMethods.HideCursor();
    }

    private void StartTurn()
    {

    }

    #region TargetSelection
    //Hides distracting UI, disables ineligible targets, sets ES selected to eligible target.
    public void EnterTargetSelection()
    {
        Debug.Log("STARTED TARGET SELECTION.");
        ui.DisableMovesMenu();                                 //Disable Moves Menu
        ObjectEventSystem.Current.Enable();                     //Enables input for object selecting.
        EnableEligableTargets();                                //Enables eligable target buttons and selects the first one.
    }

    //Enables the buttons of eligable targets.
    private void EnableEligableTargets()
    {
        switch (CurAction.Action.TargetType)
        {
            case TargetType.SELF: break;
            case TargetType.ALL: break;
            case TargetType.SINGLE_ENEMY:
                EnablePartyButtons(eParty);
                ObjectEventSystem.Current.SwitchHover(eParty[0].Go.Button, true);
                break;
            case TargetType.ALL_ENEMIES: break;
            case TargetType.SINGLE_ALLY: break;
            case TargetType.ALL_ALLIES: break;
        }
    }

    //Enables interaction on the party's buttons and sets their navigation.
    private void EnablePartyButtons(List<ActiveFighter> party)
    {
        ButtonObject bo;
        for (int i = 0; i < party.Count; i++)
        {
            bo = party[i].Go.Button;
            bo.Interactable = true;

            bo.Navigation.Right = (i + 1 < party.Count)? party[i + 1].Go.Button : party[0].Go.Button; //Set right nav.
            bo.Navigation.Left = (i - 1 > -1)? party[i - 1].Go.Button : party[party.Count - 1].Go.Button; //Set left nav.
        }
    }
    #endregion

    //Switches current fighter and reloads player move menu.
    private void SwitchCurrentFighter(ActiveFighter actFighter)
    {
        curFighter = actFighter;
        ui.ReloadPlayerFighterMovesUI(curFighter);
    }

    #region Instantiate
    //Spawns fighter gameObject and fighterUI;
    private ActiveFighter InstantiateFighter(Fighter fighter, GameObject prefab, Transform goSP, Transform uiSP)
    {
        fighter.Initialize();

        FighterGO go = Instantiate(prefab, goSP).GetComponent<FighterGO>();
        FighterUI ui = Instantiate(_fighterUiPfb, uiSP).GetComponent<FighterUI>();

        ActiveFighter actFighter = new ActiveFighter(fighter, ui, go);
        go.Initialize(this, actFighter);
        ui.Initialize(this, actFighter);

        return actFighter;
    }
    #endregion

}

//=====================================================================================================================
public class ActiveFighter
{
    private Fighter data;
    private FighterUI ui;
    private FighterGO go;

    private Stats fluxStats;
    private Stats boostStats;

    #region GS
    public Fighter Data { get => data; set => data = value; }
    public FighterUI Ui { get => ui; set => ui = value; }
    public Stats FluxStats { get => fluxStats; set => fluxStats = value; }
    public FighterGO Go { get => go; set => go = value; }
    #endregion

    public ActiveFighter(Fighter data, FighterUI ui, FighterGO go)
    {
        this.data = data;
        this.ui = ui;
        this.go = go;

        fluxStats = data.TotalStats; //TotalStats multiplied by boostStats.
        boostStats = new Stats(1); //Stat change multipliers during battles.
    }

    public void ReloadFluxStats()
    {
        fluxStats = Stats.Multiply(data.TotalStats, boostStats);
    }
}
//=====================================================================================================================
public class ActiveAction
{
    private BattleManager bm;

    private MoveSO action; //Replace type with action (used for items and moves)
    private ActiveFighter user;
    private List<ActiveFighter> targets = new List<ActiveFighter>();

    #region GS
    public List<ActiveFighter> Targets { get => targets; set => targets = value; }
    public MoveSO Action { get => action; set => action = value; }
    #endregion

    public ActiveAction(BattleManager bm, MoveSO action, ActiveFighter user)
    {
        this.bm = bm;
        this.action = action;
        this.user = user;
    }

    public void UseMove()
    {
        action.Use(user, targets);
    }

    public void AddTarget(ActiveFighter actFighter)
    {
        Debug.Log("ADDED " + actFighter.Data.Name + " AS A TARGET FOR " + user.Data.Name + "'S " + action.Name);
        targets.Add(actFighter);
        if (ValidTargets())
        {
            ObjectEventSystem.Current.DisableInput();
            bm.Ui.ConfirmActionMenu.Enable();
        }
    }

    private bool ValidTargets()
    {
        switch (action.TargetType)
        {
            case TargetType.SELF:
            case TargetType.SINGLE_ENEMY:
            case TargetType.SINGLE_ALLY:
                return (targets.Count == 1);
            default: return false;
        }
    }
}
//=====================================================================================================================
public class ActionList
{
    
}

