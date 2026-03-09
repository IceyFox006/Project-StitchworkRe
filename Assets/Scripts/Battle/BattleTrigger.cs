using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BattleTrigger : MonoBehaviour
{
    [Tooltip("Max capacity is 8.")]
    [SerializeField] private EnemyFighter[] _enemyParty;

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.CompareTag("Player")) return;

        //Play battle enter animation
        collision.transform.position = BattleManager.Inst.BattleArea.position; //Teleport Player
        GenericMethods.ShowCursor();
        BattleManager.Inst.StartBattle(collision.GetComponent<PlayerManager>().Data.Party, _enemyParty);
    }
}
