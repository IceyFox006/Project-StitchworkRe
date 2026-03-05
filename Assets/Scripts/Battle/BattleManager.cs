using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private static BattleManager inst;

    [Header("Spawn Points")]
    [Tooltip("Where the player gets teleported to when a battle begins.")]
    [SerializeField] private Transform _battleArea;
    [Tooltip("Where player fighter UI spawn in.")]
    [SerializeField] private Transform _playerFighterUiSP;
    [Tooltip("Where enemy fighter UI spawn in.")]
    [SerializeField] private Transform _enemyFighterUiSP;
    [Tooltip("Where player fighters spawn in.")]
    [SerializeField] private Transform[] _playerFighterSPs;
    [Tooltip("Where enemy fighters spawn in.")]
    [SerializeField] private Transform[] _enemyFighterSPs;

    [Header("Prefabs")]
    [SerializeField] private GameObject _playerFighterPfb;
    [SerializeField] private GameObject _enemyFighterPfb;
    [SerializeField] private GameObject _fighterUIPfb;

    private List<ActiveFighter> pParty = new List<ActiveFighter>();
    private List<ActiveFighter> eParty = new List<ActiveFighter>();

    #region GS
    public static BattleManager Inst { get => inst; set => inst = value; }
    public Transform BattleArea { get => _battleArea; set => _battleArea = value; }
    #endregion
    private void Awake()
    {
        inst = this;
    }

    [HideInInspector]
    public void StartBattle(PlayerFighter[] playerParty, EnemyFighter[] enemyParty)
    {
        //SET-UP
        //Player Party
        for (int pfID = 0; pfID < _playerFighterSPs.Length; pfID++)
        {
            if (pfID >= playerParty.Length) break;

            pParty.Add(InstantiateFighter(playerParty[pfID], _playerFighterPfb, _playerFighterSPs[pfID], _playerFighterUiSP));
        }

        //Enemy Party
        for (int efID = 0; efID < _enemyFighterSPs.Length; efID++)
        {
            if (efID >= enemyParty.Length) break;

            eParty.Add(InstantiateFighter(enemyParty[efID], _enemyFighterPfb, _enemyFighterSPs[efID], _enemyFighterUiSP));
        }
    }

    private ActiveFighter InstantiateFighter(Fighter fighter, GameObject prefab, Transform goSP, Transform uiSP)
    {
        fighter.Initialize();

        GameObject go = Instantiate(prefab, goSP);
        PartAssemble partAssemble = go.GetComponentInChildren<PartAssemble>();
        partAssemble.Initialize(fighter.Parts, fighter.Palettes);

        FighterUI ui = InstantiateFighterUI(uiSP, fighter);

        return new ActiveFighter(fighter, ui);
    }

    private FighterUI InstantiateFighterUI(Transform parent, Fighter fighter)
    {
        FighterUI ui = Instantiate(_fighterUIPfb, parent).GetComponent<FighterUI>();
        ui.Initialize(fighter);

        return ui;
    }
}

//=====================================================================================================================
public class ActiveFighter
{
    private Fighter data;
    private FighterUI ui;

    public ActiveFighter(Fighter data, FighterUI ui)
    {
        this.data = data;
        this.ui = ui;
    }
}

