using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Inventory)), RequireComponent(typeof(EnemyCombat))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private LayerMask enemyMask;

    private float rotateSpeed = 25f;

    private Inventory myInventory;
    private EnemyCombat combat;
    private Transform target;
    private NavMeshAgent agent;
    private Transform shopEntrance;
    private Inventory playerInventory;

    private Vector3 initPosition;

    private bool goingBack = false;

    private float SEARCH_COOLDOWN_SECONDS;
    private float searchCooldown = 0;

    private float CHECK_VALID_COOLDOWN_SECONDS = 0.5f;
    private float checkValidCooldown = 0;

    void Start()
    {
        // Init.
        agent = transform.GetComponent<NavMeshAgent>();
        myInventory = transform.GetComponent<Inventory>();
        combat = transform.GetComponent<EnemyCombat>();
        initPosition = transform.position;
        shopEntrance = GameManager.instance.GetShopEntrance();
        playerInventory = GameManager.instance.GetPlayer().GetComponent<Inventory>();

        agent.speed = Random.Range(7, 10);

        SEARCH_COOLDOWN_SECONDS = Random.Range(2, 3);
        searchCooldown = SEARCH_COOLDOWN_SECONDS;

        //CHECK_VALID_COOLDOWN_SECONDS = Random.Range(2, 7);
        checkValidCooldown = CHECK_VALID_COOLDOWN_SECONDS;
    }

    // Update is called once per frame
    void Update()
    {
        if (searchCooldown > 0)
        {
            searchCooldown -= Time.deltaTime;
        }
        else
        {
            searchCooldown = 0;
        }

        if(checkValidCooldown > 0)
        {
            checkValidCooldown -= Time.deltaTime;
        } else
        {
            checkValidCooldown = 0;
        }
            

        if (combat.IsKnockedOut())
        {
            return;
        }

        bool isTargetValid = IsTargetStillValid();
        
        if (!isTargetValid)
        {
            target = null;
            goingBack = false;
        }

        bool hasToiletPaperPack = myInventory.HasItem();

        if (target != null)
        {
            if (goingBack && !hasToiletPaperPack)
            {
                target = null;
                goingBack = false;
                return;
            }

            bool arrived = GoTo(target);
            if (arrived) target = null;

            if (target == shopEntrance && Vector3.Magnitude(target.position - shopEntrance.position) < 5)
            {
                target = null;
            }

            return;
        }

        
        if (hasToiletPaperPack)
        {
            float distanceToHome = Vector3.Magnitude(initPosition - transform.position);
            if (distanceToHome > 1)
            {
                goingBack = true;
                agent.SetDestination(initPosition);
            }
            else
            {
                Destroy(gameObject);
            }
            return;
        }

        target = GetClosestToiletPaperPack();
        if (target == null)
        {
            target = shopEntrance;
        }
    }

    bool GoTo(Transform _target)
    {
        // Calculate distance to the target (player or toilet paper)
        Vector3 targetPosition = _target.transform.position;
        Vector3 myPosition = transform.position;

        targetPosition.y = 0;
        myPosition.y = 0;

        float distance = Vector3.Magnitude(targetPosition - myPosition);

        if ((distance - 1f) < agent.stoppingDistance)
        {
            FaceTarget();
            ToiletPaperPack toiletPaperPack = _target.GetComponent<ToiletPaperPack>();
            if (toiletPaperPack != null)
            {
                myInventory.AddItem(toiletPaperPack.transform);
                toiletPaperPack.Interact();
                return true;
            }

            Inventory otherInventory = _target.GetComponent<Inventory>();
            if(otherInventory != null && otherInventory.HasItem())
            {
                EnemyCombat enemyCombat = _target.GetComponent<EnemyCombat>();
                if (enemyCombat != null)
                {
                    enemyCombat.Steal(myInventory);
                } else
                {
                    otherInventory.DropItem();
                }
                return true;
            }
        }
        else
        {
            agent.SetDestination(targetPosition);
        }
        return false;
    }

    bool IsTargetStillValid()
    {
        if (target == null)
        {
            if (goingBack == true && !myInventory.HasItem()) return false;
            return true;
        }
        if (checkValidCooldown > 0) return true;

        checkValidCooldown = CHECK_VALID_COOLDOWN_SECONDS;

        if (target == shopEntrance)
        {
            return false;
        }

        Inventory inventory = target.GetComponent<Inventory>();
        if (inventory != null && inventory.HasItem())
        {
            return true;
        }

        ToiletPaperPack toiletPaperPack = target.GetComponent<ToiletPaperPack>();
        if (toiletPaperPack != null && !toiletPaperPack.IsInInventory())
        {
            return true;
        }

        return false;
    }

    // Ugly method that takes a random target.
    private Transform GetClosestToiletPaperPack()
    {
        if (searchCooldown > 0) return null;

        searchCooldown = SEARCH_COOLDOWN_SECONDS;

        Transform closest = null;
        float closestDistance = float.MaxValue;

        if (playerInventory.HasItem())
        {
            closestDistance = Vector3.Magnitude(playerInventory.transform.position - transform.position);
            closest = playerInventory.transform;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10, enemyMask);
        if (hitColliders.Length > 0 && hitColliders.Length < 10)
        {
            foreach (Collider collider in hitColliders)
            {
                Inventory inventory = collider.GetComponent<Inventory>();
                if (inventory != null && inventory.HasItem())
                {
                    float distance = Vector3.Magnitude(inventory.transform.position - transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closest = inventory.transform;
                    }
                }
            }
        }

        hitColliders = Physics.OverlapSphere(transform.position, 100, interactableMask);
        if (hitColliders.Length > 0)
        {
            foreach (Collider collider in hitColliders)
            {
                ToiletPaperPack toiletPaperPack = collider.GetComponent<ToiletPaperPack>();
                if (toiletPaperPack != null && !toiletPaperPack.IsInInventory())
                {
                    float distance = Vector3.Magnitude(toiletPaperPack.transform.position - transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closest = toiletPaperPack.transform;
                    }
                }
            }
        }

        return closest;
    }

    void FaceTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);
    }

    void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}
