using UnityEngine;

public class FighterGO : MonoBehaviour
{
    private ButtonObject button;
    private PartAssemble partAssemble;

    #region GS
    public ButtonObject Button { get => button; set => button = value; }
    #endregion

    public void Initialize(EntityParts parts, ColorPaletteSO[] palettes)
    {
        partAssemble = GetComponentInChildren<PartAssemble>();
        partAssemble.Initialize(parts, palettes);

        button = GetComponentInChildren<ButtonObject>();
        button.Initialize();
    }
}
