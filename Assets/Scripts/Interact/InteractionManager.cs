using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    private InputAction interact;
    [SerializeField] private float _interactRadius;
    [SerializeField] private float _interactDistance;

    private void Start()
    {
        interact = InputSystem.actions.FindAction("INTERACT");

        interact.performed += Interact_performed;
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        RaycastData interactData = RaycastDrawer.Instance.SpherecastAndDraw(transform.position, _interactRadius, transform.forward, _interactDistance, "Interact");
        if (interactData.Bool)
            Debug.Log("Interacted With: " + interactData.Hit.collider.gameObject.name);
    }
}
