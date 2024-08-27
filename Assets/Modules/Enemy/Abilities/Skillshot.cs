using System.Collections;
using System.Collections.Generic;
using Modules.Common;
using Modules.Loadout.Scripts.Guns;
using UnityEngine;
using static Enums;

public class Skillshot : MonoBehaviour
{
    [SerializeField] private int damageValue;
    [SerializeField] private LayerMask playerLmask;
    [SerializeField]private float laserRadius = 1;
    [SerializeField] private float laserDistance = 1000;
    
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
            yield return new WaitForSeconds(1f);

            SphereCastLaser();
            yield return new WaitForSeconds(enemyCooldown);
        }
    }

    IEnumerator PlayerSkillshot() {
        available = false;
        Instantiate(beam, transform.position, transform.rotation, transform);
        yield return new WaitForSeconds(playerCooldown);
        available = true;
    }

    void SphereCastLaser()
    {
        // Define the start point as the object's position
        Vector3 startPosition = transform.position;
        
        // Define the direction as the object's forward vector
        Vector3 direction = transform.forward;

        RaycastHit hitInfo;

        // Perform the SphereCast
        if (Physics.SphereCast(startPosition, laserRadius, direction, out hitInfo, laserDistance, playerLmask))
        {
            // Debug.Log("Laser hit: " + hitInfo.collider.name);

            DamageOnlyPlayer(hitInfo.transform);
            // Optional: Draw a gizmo for visualization in the editor
            Debug.DrawRay(startPosition, direction * hitInfo.distance, Color.red);
            
        }
        else
        {
            // Optional: Draw a gizmo for visualization when no hit occurs
            Debug.DrawRay(startPosition, direction * laserDistance, Color.green);
        }
    }
    
    protected void DamageOnlyPlayer(Transform other) {
        //Explicit call to TakeDamage
        if (other.TryGetComponent<ADamageable>(out ADamageable damageable)) {
            // Debug.Log("to player XXX  Found damageable " + other.gameObject);
            damageable.TakeLaserDamage(BulletBase.DamageType.Laser, damageValue);
        }
    }
}
