using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{

    private bool isInInventory = false;

    public abstract void Interact();

    public void SetToInventory(bool _val)
    {
        isInInventory = _val;
    }

    public bool IsInInventory()
    {
        return isInInventory;
    }
}
