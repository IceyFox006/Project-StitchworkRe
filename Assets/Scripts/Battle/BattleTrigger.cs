using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BattleTrigger : MonoBehaviour
{
    [Tooltip("Max capacity is 8.")]
    [SerializeField] private EnemyFighter[] _enemyParty;

    private PlayerManager pm;
    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.CompareTag("Player")) return;

        pm = collision.transform.parent.GetComponent<PlayerManager>();
        pm.Move.DisableInput();                                                             //Disable player movement.
        //Play battle enter animation (Switch camera -> Start Battle)
        GenericMethods.SwitchCamera(pm.Move.MainCamera, BattleManager.Inst.BattleCamera);   //Switch Camera
        BattleManager.Inst.StartBattle(pm.Data.Party, _enemyParty);                         //Start Battle 
        pm = null;
    }
}
