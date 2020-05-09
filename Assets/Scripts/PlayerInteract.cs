using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform interact;
    [SerializeField] private LayerMask interactableMask;

    private Inventory inventory;
    private Interactable interactableObject = null;
    private float radiusInteract = 1f;

    private bool textSet = false;

    void Start()
    {
        inventory = transform.GetComponent<Inventory>();
    }

    void Update()
    {
        Check();
        if(Input.GetKey(KeyCode.E))
        {
            Interact();
        }
    }

    void Check()
    {
        RaycastHit hit;
        Collider[] hitColliders = Physics.OverlapSphere(interact.position, radiusInteract, interactableMask);
        if (hitColliders.Length > 0)
        {
            foreach (Collider collider in hitColliders)
            {
                Interactable interactable = collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    GameManager.instance.GetInfoUI().SetActionText("Press 'e' to take the Toilet Paper Pack.");
                    textSet = true;
                    interactableObject = interactable;
                    return;
                }
            }
        }
        if(textSet)
        {
            GameManager.instance.GetInfoUI().ClearActionText();
            textSet = false;
        }
        
        interactableObject = null;
    }

    void Interact()
    {
        if (interactableObject == null) return;

        ToiletPaperPack toiletPaperPack = interactableObject.GetComponent<ToiletPaperPack>();
        if (toiletPaperPack != null)
        {
            inventory.AddItem(toiletPaperPack.transform);
        }

        interactableObject.Interact();
        interactableObject = null;
    }

    void OnDrawGizmosSelected()
    {
        if (interact == null)
            return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(interact.position, radiusInteract);
    }
}
