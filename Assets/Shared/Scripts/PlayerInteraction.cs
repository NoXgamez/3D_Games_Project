using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 500;
    [SerializeField] private LayerMask layerMask;

    private Camera playerCamera;
    private RaycastHit raycastHit;
    private IInteractable currentInteractable;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        PerformRayCast();
    }

    private void PerformRayCast()
    {
        if (Physics.Raycast(
            playerCamera.transform.position,
            playerCamera.transform.forward,
            out raycastHit,
            interactionDistance,
            layerMask))
        {
            raycastHit.collider.gameObject.TryGetComponent<IInteractable>(out currentInteractable);
        }
        else
        {
            currentInteractable = null;
        }
    }
    
}
