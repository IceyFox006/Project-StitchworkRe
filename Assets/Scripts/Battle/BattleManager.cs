using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private static BattleManager inst;
    [SerializeField] private Transform _battleArea;

    #region GS
    public static BattleManager Inst { get => inst; set => inst = value; }
    public Transform BattleArea { get => _battleArea; set => _battleArea = value; }
    #endregion
    private void Awake()
    {
        inst = this;
    }

    [HideInInspector]
    public void StartBattle(List<EnemyFighter> enemyParty)
    {
        

    }
}
