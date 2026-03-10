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

    //Hides distracting UI, disables ineligible targets, sets ES selected to eligible target.
    public void EnterTargetSelection()
    {
        Debug.Log("STARTED TARGET SELECTION.");
        _playerMovesUiSP.parent.gameObject.SetActive(false); //Disable Moves Menu
        EnableEligableTargets();//Disable ineligible targets
        //Set the selected button to first eligible target

    }

    private void EnableEligableTargets()
    {
        ObjectEventSystem.Current.SwitchHover(eParty[0].Go.Button, true);
    }

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
        go.Initialize(fighter.Parts, fighter.Palettes);

        FighterUI ui = Instantiate(_fighterUiPfb, uiSP).GetComponent<FighterUI>();

        ActiveFighter actFighter = new ActiveFighter(fighter, ui, go);
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
    private MoveSO move; //Replace type with action (used for items and moves)
    private ActiveFighter user;
    private List<ActiveFighter> targets;

    public ActiveAction(MoveSO move, ActiveFighter user)
    {
        this.move = move;
        this.user = user;
    }
}
//=====================================================================================================================
public class ActionList
{
    
}

