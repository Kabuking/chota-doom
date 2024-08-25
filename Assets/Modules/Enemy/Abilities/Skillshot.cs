using System.Collections;
using System.Collections.Generic;
using Modules.Loadout.Scripts.Guns;
using UnityEngine;
using static Enums;

public class Skillshot : MonoBehaviour
{
    [SerializeField] Team team;
    [SerializeField] GameObject beam;

    [Header("Enemy stats")]
    [SerializeField] float enemyCooldown;

    [Header("Player stats")]
    [SerializeField] float playerCooldown;
    [SerializeField] bool available;

    void Start()
    {
        if (team == Enums.Team.Enemy) {
            StartCoroutine(EnemySkillshotSystem());
        }
    }

    void Update()
    {
        if (team == Enums.Team.Enemy) {
            return;
        }
        if (available) {
            if (Input.GetKey(KeyCode.E)) {
                StartCoroutine(PlayerSkillshot());
            }
        }
    }

    IEnumerator EnemySkillshotSystem() {
        while (true) {
            Instantiate(beam,transform.position, transform.rotation, transform);
            yield return new WaitForSeconds(enemyCooldown);
        }
    }

    IEnumerator PlayerSkillshot() {
        available = false;
        Instantiate(beam, transform.position, transform.rotation, transform);
        yield return new WaitForSeconds(playerCooldown);
        available = true;
    }

}
