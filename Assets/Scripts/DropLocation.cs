using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLocation : MonoBehaviour
{

    private InfoUI infoUI;
    private Inventory playerInventory;
    private Collider currentCollider;

    void Start()
    {
        infoUI = GameManager.instance.GetInfoUI();
        playerInventory = GameManager.instance.GetPlayer().GetComponent<Inventory>();
    }

    void Update()
    {
        if (currentCollider != null && Input.GetKeyDown(KeyCode.E) && playerInventory.HasItem())
        {
            playerInventory.RemoveItem();
            infoUI.AddScore();
            infoUI.ClearActionText();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!collider.CompareTag("Player")) return;
        currentCollider = collider;
        if (playerInventory.HasItem())
        {
            infoUI.SetActionText("Press 'e' to drop the toilet paper pack.");
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider != currentCollider) return;
        currentCollider = null;
        infoUI.ClearActionText();
    }
}
