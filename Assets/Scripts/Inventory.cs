using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform itemPlaceholder;
    private Transform item = null;

    void Update()
    {
        if (Input.GetKey(KeyCode.G))
        {
            DropItem();
        }
    }

    public void AddItem(Transform _item)
    {
        Interactable interactable = _item.GetComponent<Interactable>();
        if (interactable != null && interactable.IsInInventory()) return;

        if (HasItem())
        {
            DropItem();
        }
        
        Rigidbody rb = _item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        BoxCollider collider = _item.GetComponent<BoxCollider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        if(interactable != null)
        {
            interactable.SetToInventory(true);
        }


        item = _item;
        _item.parent = itemPlaceholder;
        _item.localPosition = Vector3.zero;
        _item.localRotation = Quaternion.identity;
    }

    public bool HasItem()
    {
        return item != null;
    }

    public void RemoveItem()
    {
        if (item == null) return;
        Destroy(item.gameObject);
        item = null;
    }

    public void DropItem()
    {
        if (!HasItem()) return;

        item.parent = null;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
        BoxCollider collider = item.GetComponent<BoxCollider>();
        if (collider != null)
        {
            collider.enabled = true;
        }

        Interactable interactable = item.GetComponent<Interactable>();
        if (interactable != null)
        {
            interactable.SetToInventory(false);
        }
        item = null;
    }

    public void MoveItem(Inventory targetInventory)
    {
        if (!HasItem()) return;

        Interactable interactable = item.GetComponent<Interactable>();
        if (interactable != null)
        {
            interactable.SetToInventory(false);
        }
        
        targetInventory.AddItem(item);
        item = null;
    }

}
