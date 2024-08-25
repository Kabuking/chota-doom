using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour {
    public float explosionForce = 10f;  // Adjust the force of the explosion
    public float explosionRadius = 5f;  // Adjust the radius of the explosion

    int counter = 0;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.X)) { 
            Explode();
        }
    }

    void Explode() {
        
        // Loop through each child of the parent object
        foreach (Transform child in transform) {
            counter++;
            Rigidbody rb = child.GetComponent<Rigidbody>();

            if (rb != null) {
                // Detach the child from the parent
                Debug.Log("processing " +rb.gameObject.name);

                // Apply an explosive force to the child object
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        Debug.Log("counter: " + counter);
        // Optionally, destroy the parent object or disable it
        //Destroy(gameObject);
    }
}
