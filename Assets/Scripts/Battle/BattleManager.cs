using UnityEngine;
using UnityEngine.Timeline;

public class BattleManager : MonoBehaviour
{
    private static BattleManager inst;

    [Header("Spawn Points")]
    [Tooltip("Where the player gets teleported to when a battle begins.")]
    [SerializeField] private Transform _battleArea;
    [Tooltip("Where player fighter UI spawn in.")]
    [SerializeField] private Transform _playerFighterUISPs;
    [Tooltip("Where enemy fighter UI spawn in.")]
    [SerializeField] private Transform _enemyFighterUISPs;
    [Tooltip("Where player fighters spawn in.")]
    [SerializeField] private Transform[] _playerFighterSPs;
    [Tooltip("Where enemy fighters spawn in.")]
    [SerializeField] private Transform[] _enemyFighterSPs;

    [Header("Prefabs")]
    [SerializeField] private GameObject _playerFighterPfb;
    [SerializeField] private GameObject _enemyFighterPfb;
    [SerializeField] private GameObject _fighterUIPfb;

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

            InstantiatePlayerFighter(playerParty[pfID], _playerFighterSPs[pfID]);
        }

        //Enemy Party
        for (int efID = 0; efID < _enemyFighterSPs.Length; efID++)
        {
            if (efID >= playerParty.Length) break;

            InstantiateEnemyFighter(enemyParty[efID], _enemyFighterSPs[efID]);
        }
    }

    private void InstantiatePlayerFighter(PlayerFighter fighter, Transform spawnPoint)
    {
        GameObject go = Instantiate(_playerFighterPfb, spawnPoint);
        Instantiate(_fighterUIPfb, _playerFighterUISPs);
        InstantiateFighter(go, fighter);
    }

    private void InstantiateEnemyFighter(EnemyFighter fighter, Transform spawnPoint)
    {
        GameObject go = Instantiate(_enemyFighterPfb, spawnPoint);
        Instantiate(_fighterUIPfb, _enemyFighterUISPs);
        InstantiateFighter(go, fighter);
    }

    private void InstantiateFighter(GameObject go, Fighter fighter)
    {
        go.GetComponentInChildren<PartAssemble>().Initialize(fighter.Parts);
    }
}
