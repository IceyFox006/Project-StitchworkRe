using UnityEngine;

[System.Serializable]
public class Party
{
    [SerializeField] private Fighter[] _fighters;
    private int count;


    public int RefreshCount()
    {
        count = 0;
        for (int i = 0; i < _fighters.Length; i++)
        {
            if (_fighters[i] != null)
                count++;
        }
        return count;
    }
}
