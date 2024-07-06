using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] Transform bodyToStagger;
    [SerializeField] float staggerAmount;

    [Header("Serialized for debugging")]
    [SerializeField] float health;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyDamage")) // <- change to PlayerAttack after testing
        {
            TakeDamage(other.GetComponent<BulletBase>().damage);
            Stagger((other.GetComponent<Rigidbody>().velocity).normalized);
        }
    }

    public void TakeDamage(float dmg) { 
        health -= dmg;
        if(health <= 0)
        {
            Debug.Log("Me ded");
            gameObject.SetActive(false);
        }
    }

    public void Stagger(Vector3 direction_of_impact) {
        transform.position = transform.position + staggerAmount * direction_of_impact;     
    }

}
