using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoveButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Image _icon;

    private BattleManager bm;
    private ActiveFighter actFighter;
    private MoveSO move;

    private List<ActiveFighter> targets;
    
    public void Initialize(BattleManager bm, ActiveFighter actFighter, MoveSO move)
    {
        this.bm = bm;
        this.actFighter = actFighter;
        this.move = move;

        _name.text = move.Name;
        _icon.sprite = move.Icon;
    }

    //Sets the battleManager's curAction and enters target selection.
    public void EnterMoveTargetSelection() //@UsedLocal
    {
        bm.CurAction = new ActiveAction(bm, move, actFighter);
        bm.EnterTargetSelection();
    }
}
