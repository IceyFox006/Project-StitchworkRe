using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlayerMoveButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Image _icon;
    private Button button;

    private BattleManager bm;
    private ActiveFighter actFighter;
    private MoveSO move;

    #region GS
    public Button Button { get => button; set => button = value; }
    #endregion

    public void Initialize(BattleManager bm, ActiveFighter actFighter, MoveSO move)
    {
        this.bm = bm;
        this.actFighter = actFighter;
        this.move = move;

        _name.text = move.Name;
        _icon.sprite = move.Icon;
        button = GetComponent<Button>();
    }

    //Sets the battleManager's curAction and enters target selection.
    public void EnterMoveTargetSelection() //@UsedLocal
    {
        bm.CurAction = new ActiveMove(bm, move, actFighter);
        bm.EnterTargetSelection();
    }
}
