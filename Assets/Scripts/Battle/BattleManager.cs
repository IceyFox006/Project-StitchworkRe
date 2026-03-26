using NaughtyAttributes;
using System.Collections;
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

    [SerializeField][Tooltip("Where the player gets teleported to when a battle begins.")]
        private Transform _battleArea;
    [SerializeField] 
        private Camera _battleCamera;

    [Header("Spawn Points")]
    [SerializeField][Tooltip("What player fighter UI spawn under.")]
        private Transform _playerFighterUiSP;
    [SerializeField][Tooltip("What enemy fighter UI spawn under.")]
        private Transform _enemyFighterUiSP;
    [SerializeField][Tooltip("Where player fighters spawn in.")]
        private Transform[] _playerFighterSPs;
    [SerializeField][Tooltip("Where enemy fighters spawn in.")]
        private Transform[] _enemyFighterSPs;

    [Header("Prefabs")]
    [SerializeField] 
        private GameObject _fighterPfb;
    [SerializeField] 
        private GameObject _fighterUiPfb;

    [Header("Visuals")]
    [SerializeField][MinValue(0)][Tooltip("How long after a party dies, until the battle ends; Time win animations play out.")]
        private float _timeAfterBattle;

    private PlayerManager pm;
    private List<ActiveFighter> pParty = new List<ActiveFighter>();

    private int aiLevel;
    private List<ActiveFighter> eParty = new List<ActiveFighter>();

    private ActionList actions;
    private bool isWaitingForNextAction;

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
    public int AiLevel { get => aiLevel; set => aiLevel = value; }
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
    public void StartBattle(PlayerManager pm, EnemyFighter[] enemyParty, int aiLevel)
    {
        this.pm = pm;

        //SET-UP
        ui.Ui.SetActive(true);
        ui.BattleMenu.Enable();

        //Player Party
        ActiveFighter clone;
        for (int pfID = 0; pfID < _playerFighterSPs.Length; pfID++)
        {
            if (pfID >= pm.Data.Party.Length) break;

            clone = InstantiateFighter(pm.Data.Party[pfID], _fighterPfb, _playerFighterSPs[pfID], _playerFighterUiSP);
            clone.Party = pParty;
            pParty.Add(clone);
        }

        //Enemy Party
        this.aiLevel = aiLevel;
        for (int efID = 0; efID < _enemyFighterSPs.Length; efID++)
        {
            if (efID >= enemyParty.Length) break;

            clone = InstantiateFighter(enemyParty[efID], _fighterPfb, _enemyFighterSPs[efID], _enemyFighterUiSP);
            clone.Party = EParty;
            eParty.Add(clone);
        }
        SwitchCurrentFighter(pParty[0]);
    }

    private IEnumerator EndBattle(List<ActiveFighter> winningParty)
    {
        foreach (ActiveFighter actFighter in winningParty)
        {
            actFighter.Go.OnWin();
            actFighter.Ui.OnWin();
        }

        yield return new WaitForSeconds(_timeAfterBattle);

        ui.Ui.SetActive(false);

        foreach (ActiveFighter actFighter in winningParty)
            Destroy(actFighter.Go.gameObject);

        actions.List.Clear();
        pParty.Clear();
        eParty.Clear();

        pm.InBattle = false;

        //Add Transition Actions
        pm.Ui.OnEndTransition.AddListener(() => pm.Move.EnableInput());
        pm.Ui.OnEndTransition.AddListener(() => GenericMethods.SwitchCamera(_battleCamera, pm.Move.MainCamera));
        pm.Ui.OnEndTransition.AddListener(() => GenericMethods.SetDefault(pm));
        pm.Ui.PlayOverlay("END_BATTLE", NextAnim.Next);
    }

    public void StartTurn()
    {
        foreach (ActiveFighter actFighter in pParty)
            actFighter.ResetTurnVariables();

        foreach(ActiveFighter actFighter in eParty)
            actFighter.ResetTurnVariables();

        SwitchCurrentFighter(pParty[0]);
    }

    public void DetermineEnemyActions()
    {
        foreach (ActiveEnemyFighter actEnemy in eParty)
            actEnemy.DetermineAction();
    }

    //Starts the countdown for the next action if it is not already being performed.
    public void StartNextActionWait(float time)
    {
        if (isWaitingForNextAction)
            return;

        StartCoroutine(NextActionWait(time));
    }

    //Performs the next action after the hp ui has run its duration.
    private IEnumerator NextActionWait(float time)
    {
        isWaitingForNextAction = true;

        yield return new WaitForSeconds(time);
        actions.NextAction();
        isWaitingForNextAction = false;
    }

    //If the party is dead, ends the battle.
    public bool CheckPartyDead(List<ActiveFighter> party)
    {
        if (party.Count > 0) return false;

        StartCoroutine(EndBattle(GetOppositeParty(party)));

        return true;
    }

    #region Fighter
    //Returns the index of the fighter with the same ID as the target in the list.
    public int FindFighter(Fighter target, List<ActiveFighter> list)
    {
        for (int i = 0; i < list.Count; i++)
            if (target.EqualTo(list[i].Data)) return i;
        return -1;
    }

    //Returns the index of the next fighter in list that has not acted yet.
    public int FindFirstUnactedFighter(int curFighterIndex, List<ActiveFighter> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            curFighterIndex = DataMethods.NextIndex(curFighterIndex, list);
            if (!list[curFighterIndex].HasActed) return curFighterIndex;
        }
        return -1;
    }

    //Switches current fighter and reloads player move menu.
    public void SwitchCurrentFighter(ActiveFighter actFighter)
    {
        curFighter = actFighter;

        ui.HasActedVisual.SetActive(curFighter.HasActed);
        ui.ReloadPlayerFighterMovesUI(curFighter);
    }
    #endregion
    #region Target Selection
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
            case TargetType.ALL_EXSELF:
                Debug.LogError("Unimplemented.");
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
    public void DisableAllFighterButtons()
    {
        foreach (ActiveFighter actFighter in pParty)
            actFighter.Go.Button.Interactable = false;
        foreach (ActiveFighter actFighter in eParty)
            actFighter.Go.Button.Interactable = false;
    }


    //Returns a list that contains all eligable targets. Used for enemy ai target selection.
    public List<ActiveFighter> GetEligableTargets(ActiveAction action)
    {
        List<ActiveFighter> possibleTargets = new List<ActiveFighter>();
        switch (action.Data.Target)
        {
            case TargetType.SELF: possibleTargets.Add(action.User); break;
            case TargetType.ALL:
                DataMethods.Combine(ref possibleTargets, pParty); 
                DataMethods.Combine(ref possibleTargets, eParty);
                break;
            case TargetType.ALL_EXSELF: 
                Debug.LogError("Unimplemented.");
                break;
            case TargetType.SINGLE_ENEMY:
            case TargetType.ALL_ENEMIES: 
                DataMethods.Combine(ref possibleTargets, GetOppositeParty(action.User.Party));
                break;
            case TargetType.SINGLE_ALLY:
            case TargetType.ALL_ALLIES: 
                DataMethods.Combine(ref possibleTargets, action.User.Party);
                break;
        }
        return possibleTargets;
    }


    #endregion

    #region Instantiate
    //Spawns fighter gameObject and fighterUI;
    private ActiveFighter InstantiateFighter(Fighter fighter, GameObject prefab, Transform goSP, Transform uiSP)
    {
        fighter.Initialize();

        FighterGO go = Instantiate(prefab, goSP).GetComponent<FighterGO>();
        FighterUI ui = Instantiate(_fighterUiPfb, uiSP).GetComponent<FighterUI>();

        ActiveFighter actFighter = new ActiveFighter(this, fighter, ui, go);
        go.Initialize(this, actFighter);
        ui.Initialize(this, actFighter);

        return actFighter;
    }
    private ActiveEnemyFighter InstantiateFighter(EnemyFighter fighter, GameObject prefab, Transform goSP, Transform uiSP)
    {
        fighter.Initialize();

        FighterGO go = Instantiate(prefab, goSP).GetComponent<FighterGO>();
        FighterUI ui = Instantiate(_fighterUiPfb, uiSP).GetComponent<FighterUI>();

        ActiveEnemyFighter actFighter = new ActiveEnemyFighter(this, fighter, ui, go);
        go.Initialize(this, actFighter);
        ui.Initialize(this, actFighter);

        return actFighter;
    }
    #endregion
    private List<ActiveFighter> GetOppositeParty(List<ActiveFighter> party)
    {
        return (party == pParty)? eParty : pParty;
    }

}

//=====================================================================================================================
public class ActiveFighter
{
    protected BattleManager bm;
    private List<ActiveFighter> party;

    protected Fighter data;
    private FighterUI ui;
    private FighterGO go;

    private Stats fluxStats;
    private Stats boostStats;

    private bool hasActed;
    private bool wasHurt;

    #region GS
    public Fighter Data { get => data; set => data = value; }
    public FighterUI Ui { get => ui; set => ui = value; }
    public Stats FluxStats { get => fluxStats; set => fluxStats = value; }
    public FighterGO Go { get => go; set => go = value; }
    public bool HasActed { get => hasActed; set => hasActed = value; }
    public bool WasHurt { get => wasHurt; set => wasHurt = value; }
    public List<ActiveFighter> Party { get => party; set => party = value; }
    #endregion

    public ActiveFighter(BattleManager bm, Fighter data, FighterUI ui, FighterGO go)
    {
        this.bm = bm;

        this.data = data;
        this.ui = ui;
        this.go = go;

        fluxStats = data.TotalStats;    //TotalStats multiplied by boostStats.
        boostStats = new Stats(1);      //Stat change multipliers during battles.
    }

    public void AddHP(float amount)
    {
        data.SetHP(data.CurHP + amount);

        wasHurt = amount < 0;
    }
    public void AddEnergy(float amount)
    {
        data.SetEnergy(data.CurEnergy + amount);
    }

    public void Die()
    {
        go.OnKill();
        ui.OnKill();
        party.RemoveAt(bm.FindFighter(data, party));
    }

    #region Refresh
    public void ReloadFluxStats()
    {
        fluxStats = Stats.Multiply(data.TotalStats, boostStats);
    }
    public void ResetTurnVariables()
    {
        hasActed = wasHurt = false;
    }
    #endregion
    #region Utility
    public string AsString()
    {
        return GetType() + ": " + data.Name + "\n" + fluxStats.AsString();
    }
    #endregion
}
//---------------------------------------------------------------------------------------------------------------------
public class ActiveEnemyFighter : ActiveFighter
{
    private EnemyFighter enemyData;
    public ActiveEnemyFighter(BattleManager bm, EnemyFighter data, FighterUI ui, FighterGO go) : base(bm, data, ui, go)
    {
        enemyData = data;
    }

    public void DetermineAction()
    {
        ActiveAction actAction = null;
        if (bm.AiLevel == 0)
            actAction = DetermineRandomMove();

        bm.Actions.Add(actAction);
    }

    private ActiveMove DetermineRandomMove()
    {
        ActiveMove actAction = new ActiveMove(bm, data.Moves[0], this);
        actAction.AddETarget(bm.PParty[0]);
        return actAction;
    }
    
}
//=====================================================================================================================
public class ActiveAction
{
    protected BattleManager bm;

    protected ActionSO data;
    protected ActiveFighter user;
    private List<ActiveFighter> targets = new List<ActiveFighter>();

    #region GS
    public List<ActiveFighter> Targets { get => targets; set => targets = value; }
    public ActionSO Data { get => data; set => data = value; }
    public ActiveFighter User { get => user; set => user = value; }
    #endregion

    public ActiveAction(BattleManager bm, ActionSO data, ActiveFighter user)
    {
        this.bm = bm;
        this.data = data;
        this.user = user;
    }

    public virtual void UseAction()
    {
        ValidateTargets();
        if (targets.Count == 0)
        {
            bm.Actions.NextAction();
            return;
        }

        bm.CurAction = this;
        data.Use(user, targets);
        user.Ui.UpdateEnergyVisuals();
    }
    #region Target
    //Adds actFighter as a target and if there are enough targets brings up the confirm action menu.
    public void AddPTarget(ActiveFighter actFighter)
    {
        targets.Add(actFighter);
        if (HasCorrectTargetCount())
        {
            bm.Ui.ConfirmActionMenu.Enable();
            ObjectEventSystem.Current.DisableInput();
        }
    }

    public void AddETarget(ActiveFighter actFighter)
    {
        targets.Add(actFighter);
    }
    public void AddETargets(List<ActiveFighter> actFighters)
    {
        foreach (ActiveFighter actFighter in actFighters)
            AddETarget(actFighter);
    }

    //Returns true if the number of targets is equal to the required amount.
    private bool HasCorrectTargetCount()
    {
        switch (data.Target)
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

    //Goes through all targets and if they cannot be found in their own party (dead), removes them from the target list.
    private void ValidateTargets()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (bm.FindFighter(targets[i].Data, targets[i].Party) == -1)
                targets.RemoveAt(i);
        }
    }
    #endregion
    #region Utility
    public string AsString()
    {
        return user.AsString() + "\n" + data.AsString();
    }
    #endregion
}
//---------------------------------------------------------------------------------------------------------------------
public class ActiveMove : ActiveAction
{
    private MoveSO moveData;
    public ActiveMove(BattleManager bm, MoveSO data, ActiveFighter user) : base(bm, data, user)
    {
        moveData = data;
    }

    public override void UseAction()
    {
        base.UseAction();
        PlayMoveAnimation();
    }

    private void PlayMoveAnimation()
    {
        user.Go.Animator.SetTrigger((moveData.DamageType != DamageType.Status)? "ATTACK" : "DEFEND");
    }
}
//=====================================================================================================================
public class ActionList
{
    private BattleManager bm;

    private List<ActiveAction> list = new List<ActiveAction>();

    #region GS
    public List<ActiveAction> List { get => list; set => list = value; }
    #endregion

    public ActionList(BattleManager bm)
    {
        this.bm = bm;
    }

    //Adds action to list in order of priority and speed (fasted first).
    public void Add(ActiveAction action)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (action.Data.Priority > list[i].Data.Priority) //New action has higher priority.
            {
                list.Insert(i, action);
                action = null;
                break;
            }
            else if (action.Data.Priority == list[i].Data.Priority) //New action has equal priority; insert based off highest flux agility.
            {
                if (action.User.FluxStats.Agility > list[i].User.FluxStats.Agility)
                {
                    list.Insert(i, action);
                    action = null;
                    break;
                }
            }
        }
        if (action != null)
            list.Add(action);
        Debug.Log(AsString());
    }

    public void NextAction()
    {
        if (bm.CheckPartyDead(bm.PParty) || bm.CheckPartyDead(bm.EParty)) return;

        if (list.Count > 0)
            UseFirstAction();
        else
        {
            bm.StartTurn();
            bm.Ui.BattleMenu.Enable();
        }

    }
    public void UseFirstAction()
    {
        DataMethods.RemoveAt(list, 0).UseAction();
    }

    #region Utility
    public string AsString()
    {
        string str = "-----\n";
        
        foreach (ActiveAction action in list)
            str += action.AsString() + "\n";

        return str + "\n-----";
    }
    #endregion
}

