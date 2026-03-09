using UnityEngine;

public class Manager : MonoBehaviour
{
    public virtual void Load()
    {
        Debug.Log("Loading " + GetType() + "...");
    }
}
