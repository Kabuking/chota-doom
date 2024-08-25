using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomp : MonoBehaviour
{
    [SerializeField] Enums.Team team;
    [SerializeField] GameObject stompRing;

    [Header("Enemy stats")]
    [SerializeField] float enemyCooldown;

    [Header("Player stats")]
    [SerializeField] float playerCooldown;
    [SerializeField] bool available;
    // Start is called before the first frame update
    void Start()
    {
        if (team == Enums.Team.Enemy)
        {
            StartCoroutine(EnemyStompSystem());
        }

        available = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (team == Enums.Team.Enemy) {
            return;
        }

        if (available) {
            if (Input.GetKey(KeyCode.E)) {
                StartCoroutine(PlayerStomp());
            }
        }
    }

    IEnumerator EnemyStompSystem() {
        while (true) {
            Instantiate(stompRing, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(enemyCooldown);
        }
    }

    IEnumerator PlayerStomp() {
        available = false;
        Instantiate(stompRing, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(playerCooldown);
        available = true;
    }

}
