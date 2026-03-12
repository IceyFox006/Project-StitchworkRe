using UnityEngine;
using UnityEngine.EventSystems;

public class BattleUI : MonoBehaviour
{
    private BattleManager bm;

    [SerializeField] private GameObject _ui;

    [Header("Moves")]
    [Tooltip("What player moves UI spawn under.")]
    [SerializeField] private GameObject _playerMoveUiPfb;
    [SerializeField] private Transform _playerMovesUiSP;
    [SerializeField] private MenuUI _confirmActionMenu;

    #region GS
    public MenuUI ConfirmActionMenu { get => _confirmActionMenu; set => _confirmActionMenu = value; }
    public GameObject Ui { get => _ui; set => _ui = value; }
    #endregion

    public void Initialize(BattleManager bm)
    {
        this.bm = bm;
    }



    #region Move
    public void DisableMovesMenu()
    {
        _playerMovesUiSP.parent.gameObject.SetActive(false);
    }

    //Spawns move buttons for player fighter.
    public void InstantiatePlayerFighterMovesUI(ActiveFighter actFighter)
    {
        GameObject clone;
        foreach (MoveSO move in actFighter.Data.Moves)
        {
            clone = Instantiate(_playerMoveUiPfb, _playerMovesUiSP);
            clone.GetComponent<PlayerMoveButton>().Initialize(bm, actFighter, move);
        }
        EventSystem.current.SetSelectedGameObject(_playerMovesUiSP.GetChild(0).gameObject);
    }

    //Despawns previous moves and respawns new moves for actFighter.
    public void ReloadPlayerFighterMovesUI(ActiveFighter actFighter)
    {
        GenericMethods.DestroyChildren(_playerMovesUiSP);
        InstantiatePlayerFighterMovesUI(actFighter);
    }
    #endregion
}
