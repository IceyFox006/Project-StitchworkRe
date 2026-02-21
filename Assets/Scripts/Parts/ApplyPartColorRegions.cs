using UnityEngine;

public class ApplyPartColorRegions : MonoBehaviour
{
    [SerializeField] private ColorPaletteSO[] _palettes;

    private MeshRenderer[] renderers;

    //Changes the colors in the renderer's materials to match the palettes' colors.
    [HideInInspector]
    public void ApplyColorRegions()
    {
        if (renderers == null) 
            GetRenderers();

        foreach (MeshRenderer renderer in renderers)
        {
            for (int c = 0; c < _palettes.Length; c++)
            {
                Debug.Log(_palettes[c].Name);
                if (!DoesColorExist(renderer.material, "_COLOR" + c + "_" + 0)) break;

                for (int s = 0; s < _palettes[c].Colors.Length; s++)
                    renderer.material.SetColor("_COLOR" + c + "_" + s, _palettes[c].Colors[s]);
            }
        }
    }

    //Sets renderers to the child's renderers.
    private void GetRenderers()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
    }

    //Returns true if material has a color named name.
    private bool DoesColorExist(Material material, string name)
    {
        return material.HasColor(name);
    }
}
