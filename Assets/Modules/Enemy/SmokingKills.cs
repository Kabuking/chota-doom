using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokingKills : MonoBehaviour
{
    [SerializeField] EnemyHealth enemyHealth;
    [SerializeField] GameObject smoke;
    float maxHealth;
    bool isSmoking;
    // Start is called before the first frame update
    void Start()
    {
        isSmoking = false;
        maxHealth = enemyHealth.health;
    }

    // Update is called once per frame
    void Update()
    {
        if ((enemyHealth.health <= maxHealth * 0.3f) && !isSmoking) {
            isSmoking = true;
            Instantiate(smoke, transform.position, Quaternion.identity, transform);
        }
    }
}
