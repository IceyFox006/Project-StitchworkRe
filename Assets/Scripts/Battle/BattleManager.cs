using System.Collections.Generic;
using Unity.VisualScripting;
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

    private ActionList actions;
    private ActiveFighter curFighter;
    private ActiveAction curAction;

    #region GS
    public Transform BattleArea { get => _battleArea; set => _battleArea = value; }
    public ActiveAction CurAction { get => curAction; set => curAction = value; }
    public BattleUI Ui { get => ui; set => ui = value; }
    public static BattleManager Inst { get => inst; set => inst = value; }
    public Camera BattleCamera { get => _battleCamera; set => _battleCamera = value; }
    public List<ActiveFighter> PParty { get => pParty; set => pParty = value; }
    public List<ActiveFighter> EParty { get => eParty; set => eParty = value; }
    public ActionList Actions { get => actions; set => actions = value; }
    public ActiveFighter CurFighter { get => curFighter; set => curFighter = value; }
    #endregion
    public override void Load()
    {
        base.Load();
        inst = this;

        ui = GetComponent<BattleUI>();
        ui.Initialize(this);

        actions = new ActionList(this);
    }

    //Starts a battle.
    public void StartBattle(PlayerFighter[] playerParty, EnemyFighter[] enemyParty)
    {
        //SET-UP
        ui.Ui.SetActive(true);
        ui.BattleMenu.Enable();

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

    }

    private void StartTurn()
    {

    }

    #region TargetSelection
    //Hides distracting UI, disables ineligible targets, sets ES selected to eligible target.
    public void EnterTargetSelection()
    {
        ui.BattleMenu.Disable();                                //Disable Moves Menu
        ObjectEventSystem.Current.Enable();                     //Enables input for object selecting.
        EnableEligableTargets();                                //Enables eligable target buttons and selects the first one.
    }

    //Enables the buttons of eligable targets.
    private void EnableEligableTargets()
    {
        switch (CurAction.Data.Target)
        {
            case TargetType.SELF:
                ObjectEventSystem.Current.SwitchHover(curAction.User.Go.Button, true);
                break;
            case TargetType.ALL: 
                EnableAllButtons();
                ObjectEventSystem.Current.SwitchHover(eParty[0].Go.Button);
                break;
            case TargetType.SINGLE_ENEMY:
            case TargetType.ALL_ENEMIES:
                EnablePartyButtons(eParty);
                ObjectEventSystem.Current.SwitchHover(eParty[0].Go.Button);
                break;
            case TargetType.SINGLE_ALLY:
            case TargetType.ALL_ALLIES:
                EnablePartyButtons(pParty);
                ObjectEventSystem.Current.SwitchHover(pParty[0].Go.Button);
                break;
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

            bo.Navigation.Right = party[DataMethods.NextIndex(i, party)].Go.Button;     //Set right nav.
            bo.Navigation.Left = party[DataMethods.PreviousIndex(i, party)].Go.Button;  //Set left nav.
        }
    }

    //Enables interaction on all fighters' buttons and sets their navigation.
    private void EnableAllButtons()
    {
        ButtonObject bo;
        for (int i = 0; i < pParty.Count; i++)
        {
            bo = pParty[i].Go.Button;
            bo.Interactable = true;

            bo.Navigation.Up = (i < eParty.Count)? eParty[i].Go.Button : null;             //Set up nav.
            bo.Navigation.Right = pParty[DataMethods.NextIndex(i, pParty)].Go.Button;       //Set right nav.
            bo.Navigation.Left = pParty[DataMethods.PreviousIndex(i, pParty)].Go.Button;    //Set left nav.
        }

        for (int i = 0; i < eParty.Count; i++)
        {
            bo = eParty[i].Go.Button;
            bo.Interactable = true;

            bo.Navigation.Down = (pParty[i] != null)? pParty[i].Go.Button : null;           //Set down nav.
            bo.Navigation.Right = eParty[DataMethods.NextIndex(i, eParty)].Go.Button;       //Set right nav.
            bo.Navigation.Left = eParty[DataMethods.PreviousIndex(i, eParty)].Go.Button;    //Set left nav.
        }
    }

    //Disables interaction on all fighters; buttons.
    public void DisableAllButtons()
    {
        foreach (ActiveFighter actFighter in pParty)
            actFighter.Go.Button.Interactable = false;
        foreach (ActiveFighter actFighter in eParty)
            actFighter.Go.Button.Interactable = false;
    }
    #endregion

    public int FindFighter(Fighter fighter, List<ActiveFighter> party)
    {
        for (int i = 0; i < party.Count; i++)
            if (fighter.EqualTo(party[i].Data)) return i;
        return -1;
    }

    //Switches current fighter and reloads player move menu.
    public void SwitchCurrentFighter(ActiveFighter actFighter)
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

    public void AddHP(float amount)
    {
        data.SetHP(data.CurrentHP + amount);
    }

    public string AsString()
    {
        return GetType() + ": " + data.Name + "\n" + fluxStats.AsString();
    }
}
//=====================================================================================================================
public class ActiveAction
{
    private BattleManager bm;

    private ActionSO action;
    private ActiveFighter user;
    private List<ActiveFighter> targets = new List<ActiveFighter>();

    #region GS
    public List<ActiveFighter> Targets { get => targets; set => targets = value; }
    public ActionSO Data { get => action; set => action = value; }
    public ActiveFighter User { get => user; set => user = value; }
    #endregion

    public ActiveAction(BattleManager bm, ActionSO action, ActiveFighter user)
    {
        this.bm = bm;
        this.action = action;
        this.user = user;
    }


    public void UseAction()
    {
        action.Use(user, targets);
        PlayFighterAnimation();
    }
    #region Animation
    private void PlayFighterAnimation()
    {
        user.Go.Animator.Play("ATTACK");//Play animation.
    }
    #endregion
    #region Target
    //Adds actFighter as a target and if there are enough targets brings up the confirm action menu.
    public void AddTarget(ActiveFighter actFighter)
    {
        targets.Add(actFighter);
        if (ValidTargets())
        {
            bm.Ui.ConfirmActionMenu.Enable();
            ObjectEventSystem.Current.DisableInput();
        }
    }

    //Returns true if the number of targets is equal to the required amount.
    private bool ValidTargets()
    {
        switch (action.Target)
        {
            case TargetType.SELF:
            case TargetType.SINGLE_ENEMY:
            case TargetType.SINGLE_ALLY:
                return (targets.Count == 1);
            case TargetType.ALL_ENEMIES:
                return (targets.Count == bm.EParty.Count);
            case TargetType.ALL_ALLIES:
                return (targets.Count == bm.PParty.Count);
            case TargetType.ALL:
                return (targets.Count == (bm.EParty.Count + bm.PParty.Count));
            default: return false;
        }
    }
    #endregion
    public string AsString()
    {
        return user.AsString() + "\n" + action.AsString();
    }
}
//=====================================================================================================================
public class ActionList
{
    private BattleManager bm;

    private List<ActiveAction> actions = new List<ActiveAction>();

    public ActionList(BattleManager bm)
    {
        this.bm = bm;
    }
    public void Add(ActiveAction action)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            if (action.Data.Priority > actions[i].Data.Priority) //New action has higher priority.
            {
                actions.Insert(i, action);
                action = null;
                break;
            }
            else if (action.Data.Priority == actions[i].Data.Priority) //New action has equal priority; insert based off highest flux agility.
            {
                if (action.User.FluxStats.Agility > actions[i].User.FluxStats.Agility)
                {
                    actions.Insert(i, action);
                    action = null;
                    break;
                }
            }
        }
        if (action != null)
            actions.Add(action);
        Debug.Log(action.AsString());
    }
    public void UseFirstAction()
    {
        actions[0].UseAction();
        actions.RemoveAt(0);
    }

    #region Utility
    public string AsString()
    {
        string str = "";
        
        foreach (ActiveAction action in actions)
            str += action.AsString() + "\n";

        return str;
    }
    #endregion
}

