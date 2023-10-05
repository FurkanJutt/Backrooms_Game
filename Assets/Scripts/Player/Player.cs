using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("")]
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private LayerMask layerMaskForCrosshair;

    [Header("UI")]
    [SerializeField] private RectTransform crosshair;
    private float enlargeSize = 1.8f;
    private float rayCastDistance = 3f;

    //[Header("Inventory System")]
    //[SerializeField] private InventoryUIController inventoryUIController;

    // Chached References
    //private PlayerInventoryHolder inventory;

    private void Awake()
    {
        //inventory = GetComponent<PlayerInventoryHolder>();
    }

    private void Update()
    {
        ResizeCrosshairForIntractableObject();
    }

    private void ResizeCrosshairForIntractableObject()
    {
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, rayCastDistance, layerMaskForCrosshair))
        {
            crosshair.localScale = Vector3.one * enlargeSize;
            return;
        }
        crosshair.localScale = Vector3.one;
    }

    public void PickUp()
    {
        //if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, rayCastDistance, pickUpLayerMask))
        //{
        //    if (raycastHit.transform.TryGetComponent(out ItemPickUp item))
        //    {
        //        if (inventory.AddToInventory(item.ItemData, 1))
        //        {
        //            Destroy(item.gameObject);
        //        }
        //    }
        //}
    }

    #region NEW_INPUT_SYSTEM_CALL_BACKS
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    public void OnPickUp(InputValue key)
    {
        if (key.isPressed)
            PickUp();
    }
#endif
    #endregion NEW_INPUT_SYSTEM_CALL_BACKS
}
