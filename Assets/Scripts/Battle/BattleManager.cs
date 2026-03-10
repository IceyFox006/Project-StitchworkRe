using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Manager
{
    private static BattleManager inst;

    public static float effectivenessMultiplier = 0.2f;
    public static float STABMultiplier = 1.2f;

    [Tooltip("Where the player gets teleported to when a battle begins.")]
    [SerializeField] private Transform _battleArea;
    [SerializeField] private GameObject _battleUI;

    [Header("Spawn Points")]
    [Tooltip("What player fighter UI spawn under.")]
    [SerializeField] private Transform _playerFighterUiSP;
    [Tooltip("What enemy fighter UI spawn under.")]
    [SerializeField] private Transform _enemyFighterUiSP;
    [Tooltip("Where player fighters spawn in.")]
    [SerializeField] private Transform[] _playerFighterSPs;
    [Tooltip("Where enemy fighters spawn in.")]
    [SerializeField] private Transform[] _enemyFighterSPs;
    [Tooltip("What player moves UI spawn under.")]
    [SerializeField] private Transform _playerMovesUiSP;

    [Header("Prefabs")]
    [SerializeField] private GameObject _fighterPfb;
    [SerializeField] private GameObject _fighterUiPfb;
    [SerializeField] private GameObject _playerMoveUiPfb;

    private List<ActiveFighter> pParty = new List<ActiveFighter>();
    private List<ActiveFighter> eParty = new List<ActiveFighter>();

    private ActionList actions = new ActionList();
    private ActiveFighter curFighter;
    private ActiveAction curAction;

    #region GS
    public static BattleManager Inst { get => inst; set => inst = value; }
    public Transform BattleArea { get => _battleArea; set => _battleArea = value; }
    public ActiveAction CurAction { get => curAction; set => curAction = value; }
    #endregion
    public override void Load()
    {
        base.Load();

        inst = this;
    }

    //Starts a battle.
    public void StartBattle(PlayerFighter[] playerParty, EnemyFighter[] enemyParty)
    {
        //SET-UP
        _battleUI.SetActive(true);

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
        _playerMovesUiSP.parent.gameObject.SetActive(false);    //Disable Moves Menu
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
        GenericMethods.DestroyChildren(_playerMovesUiSP);

        curFighter = actFighter;
        InstantiatePlayerFighterMovesUI(actFighter);
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

    //Spawns move buttons for player fighter.
    private void InstantiatePlayerFighterMovesUI(ActiveFighter actFighter)
    {
        GameObject clone;
        foreach (MoveSO move in actFighter.Data.Moves)
        {
            clone = Instantiate(_playerMoveUiPfb, _playerMovesUiSP);
            clone.GetComponent<PlayerMoveButton>().Initialize(this, actFighter, move);
        }
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
    private MoveSO action; //Replace type with action (used for items and moves)
    private ActiveFighter user;
    private List<ActiveFighter> targets;

    #region GS
    public List<ActiveFighter> Targets { get => targets; set => targets = value; }
    public MoveSO Action { get => action; set => action = value; }
    #endregion

    public ActiveAction(MoveSO action, ActiveFighter user, List<ActiveFighter> targets = null)
    {
        this.action = action;
        this.user = user;
        this.targets = targets;
    }

    public void AddTarget(ActiveFighter actFighter)
    {
        Debug.Log("Added " + actFighter.Data.Name + " as a target for " + user.Data.Name + "'s " + action.Name);
    }
}
//=====================================================================================================================
public class ActionList
{
    
}

