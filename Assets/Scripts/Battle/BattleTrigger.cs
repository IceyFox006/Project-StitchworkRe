using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BattleTrigger : MonoBehaviour
{
    [SerializeField] private List<EnemyFighter> _enemyParty;

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.CompareTag("Player")) return;

        //Play battle enter animation
        collision.transform.position = BattleManager.Inst.transform.position; //Teleport Player
        BattleManager.Inst.StartBattle(_enemyParty);
    }
}
