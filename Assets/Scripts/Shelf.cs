using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour
{
    [SerializeField] private List<Transform> slots;
    [SerializeField] private GameObject toiletPaperPack;

    private int quantity = 0;

    // Returns true when the toilet paper was added, otherwise return false.
    public bool AddToiletPaperPack()
    {
        if (quantity >= slots.Count) return false;

        int slotIndex = GetFreeSlot();
        Instantiate(toiletPaperPack, slots[slotIndex]);
        return true;
    }

    int GetFreeSlot(int i = 0)
    {
        if (i > 10) return 0; // Avoid infinite recursion, but it should not happen.

        int slotIndex = Random.Range(0, slots.Count - 1);
        if (slots[slotIndex].GetComponentInChildren<ToiletPaperPack>() != null)
        {
            return GetFreeSlot(++i);
        }
        return slotIndex;
    }
}
