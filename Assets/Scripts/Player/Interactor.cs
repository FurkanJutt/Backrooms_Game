using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public Transform PlayerCameraRoot;
    public LayerMask InteractionLayer;
    private float InteractionPointDistance = 3f;

    //[Header("UI Controller")]
    //[SerializeField] private InventoryUIController inventoryUIController;

    public bool IsInteracting {  get; private set; }

    public void Interact()
    {
        //if (Physics.Raycast(PlayerCameraRoot.position, PlayerCameraRoot.forward, out RaycastHit raycastHit, InteractionPointDistance, InteractionLayer))
        //{
        //    if (raycastHit.transform.TryGetComponent(out IInteractable interactable))
        //    {
        //        if (interactable != null)
        //            StartInteraction(interactable);
        //    }
        //}
    }

    //void StartInteraction(IInteractable interactable)
    //{
    //    interactable.Interact(this, out bool interactionSuccessful);
    //    IsInteracting = true;
    //}

    void EndInteraction()
    {
        IsInteracting = false;
    }

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    public void OnInteract(InputValue key)
    {
        if (key.isPressed && !inventoryUIController.IsChestInventoryOpened())
            Interact();
        else if(inventoryUIController.IsChestInventoryOpened() && key.isPressed)
            inventoryUIController.CloseChestInventory();
    }
#endif
}
