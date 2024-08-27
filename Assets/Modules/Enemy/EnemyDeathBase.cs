using System.Collections;
using System.Collections.Generic;
using Modules.Common.Abilities.Base.model;
using Modules.Level;
using UnityEngine;

public class EnemyDeathBase : MonoBehaviour
{
    [SerializeField] private AbilityTriggeredInputType _abilityTriggeredInputType;
    [SerializeField] GameObject collapseMeshes;
    [SerializeField] EnemyHealth enemyHealth;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (enemyHealth.health <= 0) {
            Instantiate(collapseMeshes, transform.position, transform.rotation);
            LevelEvents.LevelDefeated.Invoke(_abilityTriggeredInputType);
            
            Destroy(gameObject);
        }

    }
}
