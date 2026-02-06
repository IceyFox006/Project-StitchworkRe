using UnityEngine;

public class RaycastDrawer : MonoBehaviour
{
    public static RaycastData RaycastAndDraw(Vector3 origin, Vector3 direction, float distance, string layer)
    {
        RaycastHit hit;
        bool check = Physics.Raycast(origin, direction, out hit, distance, 1 << LayerMask.NameToLayer(layer));

        if (check)
            Debug.DrawLine(origin, origin + direction, Color.green);
        else
            Debug.DrawLine(origin, origin + direction, Color.red);

        return new RaycastData(check, hit);
    }
    
    public static RaycastData RaycastAndDraw(Ray ray, float distance, string layer)
    {
        RaycastHit hit;
        bool check = Physics.Raycast(ray, out hit, distance, 1 << LayerMask.NameToLayer(layer));

        if (check)
            Debug.DrawLine(ray.origin, ray.origin + ray.direction, Color.green);
        else
            Debug.DrawLine(ray.origin, ray.origin + ray.direction, Color.red);

        return new RaycastData(check, hit);
    }
}
public class RaycastData
{
    private bool raycastBool;
    private RaycastHit hit;

    public RaycastData(bool raycastBool, RaycastHit raycastHit)
    {
        this.raycastBool = raycastBool;
        this.hit = raycastHit;
    }

    public RaycastHit Hit { get => hit; set => hit = value; }
    public bool Bool { get => raycastBool; set => raycastBool = value; }
}
