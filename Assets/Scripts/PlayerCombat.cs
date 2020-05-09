using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform combat;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject blood;

    private AudioSource audioSource;

    private static float ATTACK_COOLDOWN = 0.7f;
    private float radiusCombat = 1.7f;
    private float cooldown = 0; 

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (cooldown > 0) {
            cooldown -= Time.deltaTime;
        }

        if(Input.GetMouseButtonDown(0) && cooldown <= 0)
        {
            animator.SetTrigger("hit");
            AttackClosest();
            cooldown = ATTACK_COOLDOWN;
        }           
    }

    void AttackClosest()
    {
        RaycastHit hit;
        Collider[] hitColliders = Physics.OverlapSphere(combat.position, radiusCombat, enemyMask);
        if (hitColliders.Length > 0)
        {
            foreach (Collider collider in hitColliders)
            {
                EnemyCombat enemyCombat = collider.GetComponent<EnemyCombat>();
                if (enemyCombat != null)
                {
                    audioSource.Play();
                    Vector3 particlesPosition = collider.ClosestPoint(combat.position);
                    StartCoroutine(AddBlood(particlesPosition));
                    enemyCombat.Hit();
                    break;
                }
            }
        }
    }

    IEnumerator AddBlood(Vector3 position)
    {
        yield return new WaitForSeconds(0.3f);
        GameObject _object = Instantiate(blood, position, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        Destroy(_object);
    }

    void OnDrawGizmosSelected()
    {
        if (combat == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(combat.position, radiusCombat);
    }
}
