using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathBase : MonoBehaviour
{
    [SerializeField] GameObject collapseMeshes;
    [SerializeField] EnemyHealth enemyHealth;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (enemyHealth.health <= 0) {
            Instantiate(collapseMeshes, transform.position, transform.rotation);
            Destroy(gameObject);
        }

    }
}
