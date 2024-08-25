using System.Collections;
using System.Collections.Generic;
using Modules.Loadout.Scripts.Guns;
using UnityEngine;

public class GunJammer : MonoBehaviour
{
    [SerializeField] Enums.Team team;

    [Header("Enemy stats")]
    [SerializeField] float enemyCooldown;
    [SerializeField] float enemyJamDuration;
    [SerializeField] PlayerGun[] playerGuns;

    [Header("Player stats")]
    [SerializeField] float playerCooldown;
    [SerializeField] float playerJamDuration;
    [SerializeField] bool available;
    [SerializeField] SlowProjectile[] enemyGuns;

    void Start()
    {
        enemyGuns = FindObjectsOfType<SlowProjectile>();
        playerGuns = FindObjectsOfType<PlayerGun>();

        if (team == Enums.Team.Enemy)
        {
            StartCoroutine(EnemyJammerSystem());
        }

        available = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (available) {
            if (Input.GetKey(KeyCode.E)) {
                StartCoroutine(PlayerJammer());
            }
        }
    }

    IEnumerator EnemyJammerSystem()
    {
        while (true) {
            foreach (var gun in playerGuns) {
                gun.readyToFire = false;
            }
            yield return new WaitForSeconds(enemyJamDuration);

            foreach (var gun in playerGuns) {
                gun.readyToFire = true;
            }

            yield return new WaitForSeconds(enemyCooldown);
        }
    }

    IEnumerator PlayerJammer() {
        if (available) {
            
            available = false;
            
            foreach (var gun in enemyGuns) {
                gun.enabled = false;
            }

            yield return new WaitForSeconds(playerJamDuration);

            foreach (var gun in enemyGuns) {
                gun.enabled = true;
            }

            yield return new WaitForSeconds(playerCooldown);
            available = true;
        }
    }
}
