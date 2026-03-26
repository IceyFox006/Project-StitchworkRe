using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BattleTrigger : MonoBehaviour
{
    [Tooltip("Max capacity is 8.")]
    [SerializeField] private int _aiLevel;
    [SerializeField] private WildFighter[] _enemyParty;

    private PlayerManager pm;
    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.CompareTag("Player")) return;

        pm = collision.transform.parent.GetComponent<PlayerManager>();
        if (pm.InBattle) return;

        pm.InBattle = true;
        pm.Move.DisableInput();

        //Add Transition Actions
        pm.Ui.OnEndTransition.AddListener(() => {GenericMethods.SwitchCamera(pm.Move.MainCamera, BattleManager.Inst.BattleCamera);});
        pm.Ui.OnEndTransition.AddListener(() => {BattleManager.Inst.StartBattle(pm, _enemyParty, _aiLevel);});
        pm.Ui.OnEndTransition.AddListener(() => {GenericMethods.SetDefault(pm);});
        pm.Ui.PlayOverlay("START_BATTLE", NextAnim.Next);
    }
}
