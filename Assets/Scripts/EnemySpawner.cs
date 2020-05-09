using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;

    private float lastXposition;

    void Awake()
    {
        lastXposition = transform.position.x;
    }

    public void Spawn()
    {
        lastXposition++;
        Vector3 position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Instantiate(enemy, position, Quaternion.identity);
    }
}
