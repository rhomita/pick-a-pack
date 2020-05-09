using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Inventory)), RequireComponent(typeof(NavMeshAgent))]
public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private int HITS_TO_KNOCK_OUT = 1;

    private NavMeshAgent agent;
    private Inventory inventory;
    private int hits;

    private float KNOCKED_OUT_COOLDOWN;
    private float knockedOutCooldown = 0f;
    private bool isKnockedOut = false;

    void Start()
    {
        agent = transform.GetComponent<NavMeshAgent>();
        inventory = transform.GetComponent<Inventory>();

        HITS_TO_KNOCK_OUT = Random.Range(1, 3);
        hits = HITS_TO_KNOCK_OUT;

        KNOCKED_OUT_COOLDOWN = Random.Range(3, 6);
    }

    void Update()
    {
        if (!isKnockedOut) return;

        knockedOutCooldown -= Time.deltaTime;
        if (knockedOutCooldown <= 0)
        {
            agent.isStopped = false;
            isKnockedOut = false;
            animator.SetBool("isKnockedOut", false);
        }
    }

    public void Hit(int amount = 1)
    {
        hits -= amount;
        if (hits <= 0)
        {
            KnockOut();
            inventory.DropItem();
        }
    }

    private void KnockOut()
    {
        isKnockedOut = true;
        animator.SetBool("isKnockedOut", true);
        agent.isStopped = true;
        knockedOutCooldown = KNOCKED_OUT_COOLDOWN;
        hits = HITS_TO_KNOCK_OUT;
    }

    public void Steal(Inventory _inventory)
    {
        KnockOut();
        inventory.MoveItem(_inventory);
    }

    public bool IsKnockedOut()
    {
        return isKnockedOut;
    }
}
